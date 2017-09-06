using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Screening.WebAPI.Core.Data;
using Screening.WebAPI.Core.Models.UsersController;

namespace Screening.WebAPI.Core.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly RoleManager<Role> roleManager;
        private readonly IOptions<TokenAuthenticationOptions> tokenAuthenticationOptions;
        private readonly UserManager<User> userManager;

        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager,
            IOptions<TokenAuthenticationOptions> tokenAuthenticationOptions)
        {
            if (userManager == null)
                throw new ArgumentNullException(nameof(userManager));

            if (roleManager == null)
                throw new ArgumentNullException(nameof(roleManager));

            if (tokenAuthenticationOptions == null)
                throw new ArgumentNullException(nameof(tokenAuthenticationOptions));

            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenAuthenticationOptions = tokenAuthenticationOptions;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var username = user.UserName;
            var roleName = string.Join(",", await userManager.GetRolesAsync(user));

            return Ok(new
            {
                Username = username,
                RoleName = roleName
            });
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<User> GetUsers(int start = 0, int count = 10)
        {
            return userManager.Users.Skip(start).Take(count);
        }

        // GET: api/Users/count
        [HttpGet("count")]
        public int GetUsersCount()
        {
            return userManager.Users.Count();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize("teacher")]
        public async Task<IActionResult> PutUser([FromRoute] string id, [FromBody] User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != user.Id)
                return BadRequest();

            try
            {
                await userManager.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExistsAsync(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostUser([FromBody] RegisterViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Password != model.ConfirmPassword)
                return BadRequest();

            var user = new User {UserName = model.Username};

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleName = model.RoleName;
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                result = await roleManager.CreateAsync(new Role {Name = roleName});
                if (!result.Succeeded)
                    return BadRequest(result.Errors);
            }

            result = await userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return CreatedAtAction("GetUser", new {id = user.Id}, user);
        }

        [HttpPost("token")]
        [Produces("application/json", "application/x-www-form-urlencoded")]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            LoginViewModel model = null;
            var request = HttpContext.Request;
            var contentType = request.ContentType.ToLower();
            if (contentType.StartsWith("application/json"))
            {
                string bodyStr;
                using (var streamReader = new StreamReader(request.Body))
                {
                    bodyStr = streamReader.ReadToEnd();
                }

                model = JsonConvert.DeserializeObject<LoginViewModel>(bodyStr);
            }
            else if (contentType.StartsWith("application/x-www-form-urlencoded"))
            {
                model = new LoginViewModel
                {
                    Username = request.Form["username"],
                    Password = request.Form["password"]
                };
            }
            else
            {
                return new UnsupportedMediaTypeResult();
            }


            var jwt = await GenerateToken(model.Username, model.Password);

            if (jwt == null)
                return BadRequest();

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwt),
                expiration = jwt.ValidTo
            });
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize("teacher")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            await userManager.DeleteAsync(user);

            return Ok(user);
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user != null && await userManager.CheckPasswordAsync(user, password))
                return
                    new ClaimsIdentity(
                        new GenericIdentity(username, "Token"),
                        new Claim[] { });

            // Credentials are invalid, or account doesn't exist
            return null;
        }

        private async Task<JwtSecurityToken> GenerateToken(string username, string password)
        {
            var identity = await GetIdentity(username, password);
            if (identity == null)
                return null;

            var now = DateTime.UtcNow;

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.Ticks.ToString(), ClaimValueTypes.Integer64)
            };

            var jwtOptionsValue = tokenAuthenticationOptions.Value;
            var jwt = new JwtSecurityToken(
                jwtOptionsValue.TokenIssuer,
                jwtOptionsValue.TokenAudience,
                notBefore: now,
                claims: claims,
                expires: now.Add(jwtOptionsValue.TokenLifeTime),
                signingCredentials: new SigningCredentials(jwtOptionsValue.TokenSigningKey.ToSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            return jwt;
        }

        private async Task<bool> UserExistsAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            return user != null;
        }
    }
}
