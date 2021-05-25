using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Activity
    {
        [Key]
        public Guid ActivityID { get; set; }
        public String Title { get; set; }
        public DateTime Date { get; set; }
        public String Description { get; set; }
        public String Category { get; set; }
        public String City { get; set; }
        public String Venue { get; set; }   
    }
}