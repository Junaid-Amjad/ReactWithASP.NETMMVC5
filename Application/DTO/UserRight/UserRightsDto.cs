using Domain.Users;
using System.Collections.Generic;

namespace Application.DTO.UserRight
{
    public class UserRightsDto
    {
        public RightsAllotmentM RightsAllotmentMaster { get; set; }
        public List<RightsAllotmentD> RightsAllotmentDetail { get; set; }
    }
}