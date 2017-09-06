using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Screening.WebAPI.Core
{
    public static class Extensions
    {
        public static SymmetricSecurityKey ToSymmetricSecurityKey(this string value)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(value));
        }
    }
}
