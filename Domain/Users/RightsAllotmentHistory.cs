using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Users
{
    public class RightsAllotmentHistory
    {
        [Key]
        public Int64 RightsAllotmentHID{get;set;}
        [ScaffoldColumn(true)]
        [StringLength(450, ErrorMessage = "Length should be less than 450")]
        public String UserID { get; set; }
        public Int64 RightsAllotmentMID {get;set;}
        public Int32 RightID{get;set;}
        [Required]
        [ScaffoldColumn(true)]
        [StringLength(450, ErrorMessage = "Length should be less than 450")]
        public String EnteredUserID{get;set;}
        [Required]
        public DateTime EntryDate{get;set;}
        [Required]
        public string UserIP{  get;set; }
        [Required]
        public string UserSystem{get;set;}
    }
}