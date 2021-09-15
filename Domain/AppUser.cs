using Microsoft.AspNetCore.Identity;
using System;

namespace Domain
{
    public class AppUser:IdentityUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public bool IsActive { get; set; }
        public bool IsCancel { get; set; }
        public DateTime EntryDate { get; set; }
        public string UserID { get; set; }
        public string SystemIP { get; set; }
        public string SystemName { get; set; }
        public int UserViewID{get;set;}
        public string ImagePath {get;set;}
    }
}