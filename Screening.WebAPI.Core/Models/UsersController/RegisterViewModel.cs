namespace Screening.WebAPI.Core.Models.UsersController
{
    public class RegisterViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string RoleName { get; set; }
    }
}
