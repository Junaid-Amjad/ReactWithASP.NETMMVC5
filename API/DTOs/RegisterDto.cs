using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {

  //      [Required]
        public string DisplayName { get; set; }
//        [Required]
        [EmailAddress]
        public string Email { get; set; }
        //[Required]
        // [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$",ErrorMessage="Password must be complex")]
        [RegularExpression("^(.{0,5}|[^0-9]*|[^A-Z]*|[a-zA-Z0-9]*)$",ErrorMessage ="Password must have atleast 6 character with 1 upper case and 1 lower case")]
        public string Password { get; set; }
//        [Required]
        public string UserName { get; set; }
        public string ContactNo {get;set;}
        public string UserID { get; set; }
        public string UserIP { get; set; }
        public string UserSystem { get; set; }
        public string ImagePath{get;set;}
        public int UserViewID{get;set;}

    }
}