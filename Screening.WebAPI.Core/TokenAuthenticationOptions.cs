using System;

namespace Screening.WebAPI.Core
{
    public class TokenAuthenticationOptions
    {
        public string TokenIssuer { get; set; }

        public string TokenAudience { get; set; }

        public string TokenSigningKey { get; set; }

        public TimeSpan TokenLifeTime { get; set; }
    }
}
