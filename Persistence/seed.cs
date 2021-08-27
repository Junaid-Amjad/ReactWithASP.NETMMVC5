using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context,UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var users = new List<AppUser>{
                    new AppUser{DisplayName="Junaid", UserName="junaid",Email="junaid@test.com"},
                    new AppUser{DisplayName="Ali", UserName="ali",Email="ali@test.com"},
                    new AppUser{DisplayName="Zohaib", UserName="zohaib",Email="zohaib@test.com"},
                };
                foreach(var user in users)
                {
                    await userManager.CreateAsync(user,"Pa$$w0rd");
                }
            }

            if (!context.Activities.Any()) {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Title = "Past Activity 1",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Activity 2 months ago",
                    Category = "drinks",
                    City = "London",
                    Venue = "Pub",
                },
                new Activity
                {
                    Title = "Past Activity 2",
                    Date = DateTime.Now.AddMonths(-1),
                    Description = "Activity 1 month ago",
                    Category = "culture",
                    City = "Paris",
                    Venue = "Louvre",
                },
                new Activity
                {
                    Title = "Future Activity 1",
                    Date = DateTime.Now.AddMonths(1),
                    Description = "Activity 1 month in future",
                    Category = "culture",
                    City = "London",
                    Venue = "Natural History Museum",
                },
                new Activity
                {
                    Title = "Future Activity 2",
                    Date = DateTime.Now.AddMonths(2),
                    Description = "Activity 2 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "O2 Arena",
                },
                new Activity
                {
                    Title = "Future Activity 3",
                    Date = DateTime.Now.AddMonths(3),
                    Description = "Activity 3 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Another pub",
                },
                new Activity
                {
                    Title = "Future Activity 4",
                    Date = DateTime.Now.AddMonths(4),
                    Description = "Activity 4 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Yet another pub",
                },
                new Activity
                {
                    Title = "Future Activity 5",
                    Date = DateTime.Now.AddMonths(5),
                    Description = "Activity 5 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Just another pub",
                },
                new Activity
                {
                    Title = "Future Activity 6",
                    Date = DateTime.Now.AddMonths(6),
                    Description = "Activity 6 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "Roundhouse Camden",
                },
                new Activity
                {
                    Title = "Future Activity 7",
                    Date = DateTime.Now.AddMonths(7),
                    Description = "Activity 2 months ago",
                    Category = "travel",
                    City = "London",
                    Venue = "Somewhere on the Thames",
                },
                new Activity
                {
                    Title = "Future Activity 8",
                    Date = DateTime.Now.AddMonths(8),
                    Description = "Activity 8 months in future",
                    Category = "film",
                    City = "London",
                    Venue = "Cinema",
                }
            };

            await context.Activities.AddRangeAsync(activities);
            await context.SaveChangesAsync();
            }
            if (!context.Rights.Any())
            {
                var Rights = new List<Rights> { 
                    new Rights
                    {
                        RightID=1,RightName="Add/Edit",RightDescription="Add from iSpy",ParentID=0,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=2,RightName="IP Cameras",RightDescription="IP Cameras from iSpy",ParentID=1,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=3,RightName="Source_jpeg",RightDescription="Source_jpeg from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=4,RightName="Source_mjpeg",RightDescription="Source_mjpeg from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=5,RightName="Source_ffmpeg",RightDescription="Source_ffmpeg from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=6,RightName="Source_Local",RightDescription="Source_Local from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=7,RightName="Source_Desktop",RightDescription="Source_Desktop from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=8,RightName="Source_VLC",RightDescription="Source_VLC from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=9,RightName="Source_Ximea",RightDescription="Source_Ximea from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=10,RightName="Source_Kinect",RightDescription="Source_Kinect from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=11,RightName="Source_Custom",RightDescription="Source_Custom from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=12,RightName="Source_ONVIF",RightDescription="Source_ONVIF from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=13,RightName="Source_clone",RightDescription="Source_clone from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=14,RightName="Motion_Detection",RightDescription="Motion_Detection from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=15,RightName="Recording",RightDescription="Recording from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=16,RightName="PTZ",RightDescription="PTZ from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=17,RightName="Save_Frames",RightDescription="Save_Frames from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=18,RightName="Cloud",RightDescription="Cloud from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=19,RightName="Scheduling",RightDescription="Scheduling from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=20,RightName="Alerts",RightDescription="Alerts from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=21,RightName="Storage",RightDescription="Storage from iSpy",ParentID=2,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=22,RightName="Access_Media",RightDescription="Access_Media from iSpy",ParentID=0,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=23,RightName="Remote_Commands",RightDescription="Remote_Commands from iSpy",ParentID=0,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=24,RightName="Web_Settings",RightDescription="Web_Settings from iSpy",ParentID=0,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=25,RightName="Settings",RightDescription="Settings from iSpy",ParentID=0,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=26,RightName="Grid_Views",RightDescription="Grid_Views from iSpy",ParentID=0,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=27,RightName="Logs",RightDescription="Logs from iSpy",ParentID=0,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=28,RightName="View_File_Menu",RightDescription="View_File_Menu from iSpy",ParentID=0,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=29,RightName="View_Ispy_Link",RightDescription="View_Ispy_Link from iSpy",ParentID=0,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=30,RightName="View_Status_Bar",RightDescription="View_Status_Bar from iSpy",ParentID=0,isActive=true,isCancel=false
                    },
                    new Rights
                    {
                        RightID=31,RightName="View_Layout_options",RightDescription="View_Layout_options from iSpy",ParentID=0,isActive=true,isCancel=false
                    }

                };

                await context.Rights.AddRangeAsync(Rights);
                await context.SaveChangesAsync();
            }
        }
    }
}