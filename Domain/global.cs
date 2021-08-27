using Microsoft.EntityFrameworkCore;
using System;

namespace Domain
{
    [Keyless]
    public class global
    {
        public DateTime CurrentDate { get; set; }
    }
}
