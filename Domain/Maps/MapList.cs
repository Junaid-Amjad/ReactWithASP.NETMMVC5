using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Maps
{
    public class MapList
    {
        [Key]
        public Int64 MapListID { get; set; }
        public String MapListName { get; set; }
        public Int32 MapCategoriesID { get; set; }
        public String ImageSrc { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public Int64 ParentID { get; set; }
        public int LevelNo { get; set; }
        public Guid UserID { get; set; }
        public DateTime EntryDate { get; set; }
        public string SystemIP { get; set; }
        public string SystemName { get; set; }
        public bool IsActive { get; set; }
        public bool IsCancel { get; set; }
        public string CameraIP{get;set;}
        public DateTime UpdateDateTime { get; set; }
        public string UpdateUserID { get; set; }
        public string UpdateUserSystem { get; set; }
        public string UpdateUserIP { get; set; }

    }
}