using System;

namespace API.DTOs
{
    public class UserDto
    {
        public Guid id{get;set;}

        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public string ContactNo {get;set;}
        public string Email {get;set;}
    }
}