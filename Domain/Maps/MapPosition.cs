using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Maps
{
    public class MapPosition
    {
        [Key]
        public Int64 MapPositionID { get; set; }
        public Int64 MapListID { get; set; }
        [ForeignKey("MapListID")]
        public MapList MapList { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string UserID { get; set; }
        public string SystemIP { get; set; }
        public string SystemName { get; set; }
        public Int32 Rotation{get;set;}
        public Int32 SavedStatusID{get;set;}
        //1 for saved from mapList
        //2 for saved from mapPosition
        public Int32 Width { get; set; }
        public Int32 Height { get; set; }
    }
}