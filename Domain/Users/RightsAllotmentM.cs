using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Users
{
    public class RightsAllotmentM
    {
        [Key]
        public Int64 RightsAllotmentID{get;set;}
        [ScaffoldColumn(true)]
        [StringLength(450, ErrorMessage = "Length should be less than 450")]
        public String UserID { get; set; }
        public DateTime EntryDateTime { get; set; }
        [ScaffoldColumn(true)]
        [StringLength(450,ErrorMessage ="Length should be less than 450")]
        public String AssignByUserID { get; set; }
        public String AssignBySystem { get; set; }
        public String AssignByIP { get; set; }
        [ScaffoldColumn(true)]
        [StringLength(450, ErrorMessage = "Length should be less than 450")]
        public String UpdateUserID { get; set; }
        public String? UpdateSystemIP { get; set; }
        public String? UpdateSystemName { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsCancel { get; set; }
    }
}