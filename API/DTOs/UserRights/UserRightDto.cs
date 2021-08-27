using Domain.Users;
using System.Collections.Generic;

namespace API.DTOs.UserRights
{
    public class UserRightDto
    {
        public RightsAllotmentM RightsAllotmentMaster { get; set; }
        public List<RightsAllotmentD> RightsAllotmentDetail { get; set; } 
    }
}