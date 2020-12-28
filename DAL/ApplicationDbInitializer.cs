using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.DAL
{
    public class ApplicationDbInitializer
    {
        public static void SeedRoles(MindOfSpaceContext mindOfSpaceContext)
        {
            var roles = new List<string>{"Admin", "User"};
            foreach(var role in roles)
            {
                if(mindOfSpaceContext.Roles.Where(x => x.Name == role).FirstOrDefault() == null)
                {
                    var entity = new IdentityRole{Name = role, NormalizedName = role.ToUpper()};
                    mindOfSpaceContext.Roles.Add(entity);
                }
            }
            mindOfSpaceContext.SaveChanges();
        }


        public static void SeedUsers(UserManager<Player> userManager)
        {
            GenerateUser("PKCAdmin", "Start123!4thur", userManager, new List<string>{"User", "Admin"});
            GenerateUser("PKCUSer", "Start123!4thur", userManager, new List<string>{"User"});
        }

        public static Player GenerateUser(string username, string password, UserManager<Player> userManager, List<string> Roles)
        {
            var user = new Player {UserName = username};
            Task<IdentityResult> result = userManager.CreateAsync(user, password);
            result.Wait();

            if(result.Result.Succeeded)
            {
                var roleResult = userManager.AddToRolesAsync(user, Roles);
                roleResult.Wait();

                var useresult = userManager.FindByNameAsync(user.UserName);
                useresult.Wait();
                
                return useresult.Result;
            }
            return null;
        }
    }
}