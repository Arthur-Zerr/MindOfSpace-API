using System;

namespace MindOfSpace_Api.Dtos
{
    public class TokenInformationDto
    {
        public string JWTToken { get; set; }
        public DateTimeOffset? Expires { get; set; }
    }
}