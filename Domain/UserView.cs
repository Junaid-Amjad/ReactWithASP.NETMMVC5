using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain
{
    public class UserView
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int UserViewID { get; set; }
        [Required]
        public string UserViewName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsCancel { get; set; }

    }
}