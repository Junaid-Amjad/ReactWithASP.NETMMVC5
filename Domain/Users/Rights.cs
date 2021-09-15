using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Users
{
    public class Rights
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int RightID { get; set; }
        public string RightName { get; set; }
        public string RightDescription { get; set; }
        public int ParentID { get; set; }
        public bool isActive { get; set; }
        public bool isCancel { get; set; }
        public long RightValue { get; set; }
    }
}