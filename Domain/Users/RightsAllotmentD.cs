using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Users
{
    public class RightsAllotmentD
    {
        [Key]
        public Int64 RightsAllotmentDID { get; set; }
        [Required]
        public Int64 RightsAllotmentMID{get;set;}
        [ForeignKey("RightsAllotmentMID")]
        public RightsAllotmentM RightsAllotmentM { get; set; }
        public Int32 RightID { get; set; }

    }
}
