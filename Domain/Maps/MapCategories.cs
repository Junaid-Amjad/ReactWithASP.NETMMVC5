using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Maps
{
    public class MapCategories
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int MapCategoriesID { get; set; }
        public string MapCategoriesName { get; set; }
        public string MapCategoriesRemarks { get; set; }
        public bool IsActive { get; set; }
        public bool IsCancel { get; set; }
    }
}