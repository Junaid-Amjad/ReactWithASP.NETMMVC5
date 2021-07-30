using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GridLayoutMaster
    {
        [Key]
        public long GridLayoutMasterID { get; set; }
        public string LayoutName { get; set; }
        public int NoofColumns { get; set; }

    }
}