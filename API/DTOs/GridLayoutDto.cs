using Domain;
using System.Collections.Generic;

namespace API.DTOs
{
    public class GridLayoutDto
    {
        public GridLayoutMaster Master { get; set; }
        public List<GridLayoutDetail> Detail { get; set; }
    }
}