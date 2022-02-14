using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Simple_911.Models;

namespace Simple_911.Data
{
    public class DataUtility
    {

        public static async Task ManageDataAsync(IHost host)
        {
            using var svcScope = host.Services.CreateScope();
            var svcProvider = svcScope.ServiceProvider;
            //Service: An instance of RoleManager
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            //Service: An instance of RoleManager
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //Service: An instance of the UserManager
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<SimpleUser>>();
            //Migration: This is the programmatic equivalent to Update-Database
            await dbContextSvc.Database.MigrateAsync();


            //Seed Methods
            await SeedRolesAsync(roleManagerSvc);
            await SeedDefaultPriorityAsync(dbContextSvc);
            await SeedDefaultStatusAsync(dbContextSvc);
            await SeedDefaultTypeAsync(dbContextSvc);
            await SeedDefaultUsersAsync(userManagerSvc);
        }


        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Manager"));
            await roleManager.CreateAsync(new IdentityRole("Dispatcher"));
            await roleManager.CreateAsync(new IdentityRole("Call Taker"));
            await roleManager.CreateAsync(new IdentityRole("Ground Unit"));
            await roleManager.CreateAsync(new IdentityRole("Not Verified"));
        }

        public static async Task SeedDefaultPriorityAsync(ApplicationDbContext context)
        {
            try
            {
                IList<Models.Priority> priorities = new List<Priority>() {
                                                    new Priority() { Name = "Code 2" },
                                                    new Priority() { Name = "Code 3" },
                };

                var dbPriorities = context.Priorities.Select(c => c.Name).ToList();
                await context.Priorities.AddRangeAsync(priorities.Where(c => !dbPriorities.Contains(c.Name)));
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Priorities.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefaultStatusAsync(ApplicationDbContext context)
        {
            try
            {
                IList<Models.Status> statuses = new List<Status>() {
                                                    new Status() { Name = "Pending" },
                                                    new Status() { Name = "Dispatched" },
                                                    new Status() { Name = "Enroute" },
                                                    new Status() { Name = "Onscene" },
                                                    new Status() { Name = "Transporting" },
                                                    new Status() { Name = "At Hospital" },
                                                    new Status() { Name = "In Service" },
                                                    new Status() { Name = "Out of Service" },
                                                    new Status() { Name = "Assisting Unit" },
                };

                var dbStatuses = context.Statuses.Select(c => c.Name).ToList();
                await context.Statuses.AddRangeAsync(statuses.Where(c => !dbStatuses.Contains(c.Name)));
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Statuses.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefaultTypeAsync(ApplicationDbContext context)
        {
            try
            {
                IList<Models.CallType> callTypes = new List<CallType>() {
                                                    new CallType() { Name = "ARREST", Description = "Cardiac Arrest"},
                                                    new CallType() { Name = "ASSIST", Description = "Invalid Assist"},
                                                    new CallType() { Name = "ATTEMPT", Description = "Suicide Attempt / Possible Suicide"},
                                                    new CallType() { Name = "BREATH", Description = "Difficulty Breathing"},
                                                    new CallType() { Name = "CHOKE", Description = "Person Choking"},
                                                    new CallType() { Name = "HEART", Description = "Heart Problems"},
                                                    new CallType() { Name = "ILL", Description = "Ill Person"},
                                                    new CallType() { Name = "INJ ACC", Description = "Injrury Accident"},
                                                    new CallType() { Name = "INJURY", Description = "Injured Person"},
                                                    new CallType() { Name = "OVER", Description = "Possible Overdose"},
                                                    new CallType() { Name = "STROKE", Description = "Possible Stroke"},
                                                    new CallType() { Name = "UNC", Description = "Unconscious Person"},

                };

                var dbTypes = context.CallTypes.Select(c => c.Name).ToList();
                await context.CallTypes.AddRangeAsync(callTypes.Where(c => !dbTypes.Contains(c.Name)));
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Call Types.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefaultUsersAsync(UserManager<SimpleUser> userManager)
        {
            //Seed Default Admin
            var defaultUser = new SimpleUser
            {
                UnitNumber = "ADMIN",
                UserName = "ADMIN",
                FirstName = "Mike",
                LastName = "Peachman",
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.UnitNumber);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Default Admin User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            //Seed Default Manager
            defaultUser = new SimpleUser
            {
                UnitNumber = "MANAGER",
                UserName = "MANAGER",
                FirstName = "Hank",
                LastName = "Ralford",
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.UnitNumber);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, "Manager");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Default Manager.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            //Seed Default Dispatcher
            defaultUser = new SimpleUser
            {
                UnitNumber = "DIS01",
                UserName = "DIS01",
                FirstName = "Joe",
                LastName = "Shmoe",
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.UnitNumber);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, "Dispatcher");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Default Dispatcher.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            //Seed Default Call Taker
            defaultUser = new SimpleUser
            {
                UnitNumber = "CT01",
                UserName = "CT01",
                FirstName = "Jane",
                LastName = "Doe",
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.UnitNumber);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, "Call Taker");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Default Call Taker.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            //Seed Default Ground Unit
            defaultUser = new SimpleUser
            {
                UnitNumber = "M94",
                UserName = "M94",
                FirstName = "MAUMEE",
                LastName = "MEDIC94",
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.UnitNumber);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, "Ground Unit");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Default Ground Unit.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            defaultUser = new SimpleUser
            {
                UnitNumber = "M95",
                UserName = "M95",
                FirstName = "MAUMEE",
                LastName = "MEDIC95",
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.UnitNumber);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, "Ground Unit");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Default Ground Unit.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            defaultUser = new SimpleUser
            {
                UnitNumber = "M96",
                UserName = "M96",
                FirstName = "MAUMEE",
                LastName = "MEDIC96",
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.UnitNumber);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, "Ground Unit");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Default Ground Unit.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            defaultUser = new SimpleUser
            {
                UnitNumber = "E94",
                UserName = "E94",
                FirstName = "MAUMEE",
                LastName = "ENGINE94",
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.UnitNumber);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, "Ground Unit");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Default Ground Unit.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            defaultUser = new SimpleUser
            {
                UnitNumber = "C94",
                UserName = "C94",
                FirstName = "MAUMEE",
                LastName = "CHIEF94",
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.UnitNumber);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, "Ground Unit");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Default Ground Unit.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            defaultUser = new SimpleUser
            {
                UnitNumber = "LS7",
                UserName = "LS7",
                FirstName = "COUNTY",
                LastName = "LIFESQUAD7",
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.UnitNumber);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, "Ground Unit");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Default Ground Unit.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }
    }
}
