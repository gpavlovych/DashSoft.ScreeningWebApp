using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Screening.WebAPI.Core.Data;
using Screening.WebAPI.Core.Models.MarksController;

namespace Screening.WebAPI.Core.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Marks")]
    public class MarksController : Controller
    {
        private readonly ScreeningWebApiCoreContext context;
        private readonly UserManager<User> userManager;

        public MarksController(ScreeningWebApiCoreContext context, UserManager<User> userManager)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (userManager == null)
                throw new ArgumentNullException(nameof(userManager));

            this.context = context;
            this.userManager = userManager;
        }

        // GET: api/Marks
        [HttpGet]
        public IEnumerable<Mark> GetMarks(int start = 0, int count = 10)
        {
            return context.Marks.Skip(start).Take(count);
        }

        // GET: api/Marks/count
        [HttpGet("count")]
        public int GetMarksCount()
        {
            return context.Marks.Count();
        }

        // GET: api/Marks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMark([FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mark = await context.Marks.SingleOrDefaultAsync(m => m.Id == id);

            if (mark == null)
                return NotFound();

            return Ok(mark);
        }

        // PUT: api/Marks/5
        [Authorize("teacher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMark([FromRoute] long id, [FromBody] UpdateMarkViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != model.Id)
                return BadRequest();

            var mark = context.Marks.FirstOrDefault(it => it.Id == id);
            if (mark == null)
                return NotFound();

            mark.Updated = DateTime.UtcNow;
            mark.Value = model.Value;
            context.Entry(mark).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarkExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST: api/Marks
        [Authorize("teacher")]
        [HttpPost]
        public async Task<IActionResult> PostMark([FromBody] CreateMarkViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var teacher = await userManager.GetUserAsync(User);

            var entity = new Mark
            {
                Created = DateTime.UtcNow,
                StudentId = model.StudentId,
                Lesson = model.LessonName,
                TeacherName = teacher.UserName
            };

            context.Marks.Add(entity);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetMark", new {id = entity.Id}, entity);
        }

        // DELETE: api/Marks/5
        [Authorize("teacher")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMark([FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mark = await context.Marks.SingleOrDefaultAsync(m => m.Id == id);
            if (mark == null)
                return NotFound();

            context.Marks.Remove(mark);
            await context.SaveChangesAsync();

            return Ok(mark);
        }

        private bool MarkExists(long id)
        {
            return context.Marks.Any(e => e.Id == id);
        }
    }
}
