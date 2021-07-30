using System.Collections.Generic;
using Domain;

namespace Application.DTO.Grid
{
    public class GridLayoutDto
    {
        public GridLayoutMaster Master { get; set; }
        public List<GridLayoutDetail> Detail { get; set; }

    }
}