using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GridLayoutDetail
    {
        [Key]
        public long GridLayoutDetailID { get; set; }
        public long GridLayoutMasterID { get; set; }
        public GridLayoutMaster GridLayoutMaster { get; set; }
        public string CameraIP { get; set; }
    }
}
