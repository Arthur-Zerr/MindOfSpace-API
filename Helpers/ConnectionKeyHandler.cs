// using System.Text;
// using System;
// using System.Threading.Tasks;
// using MindOfSpace_Api.DAL;
// using MindOfSpace_Api.Enums;
// using MindOfSpace_Api.Models;
// using System.Linq;
// using Microsoft.EntityFrameworkCore;

// namespace MindOfSpace_Api.Helpers
// {
//     //String ConnectionKey Form: MOS_Username_PlayerId_RoleNumber_Date
//     // 0 : Identifire
//     // 1 : Username
//     // 2 : PlayerID
//     // 3 : RoleNumber
//     // 4 : Created Date

//     public class ConnectionKeyHandler
//     {
//         private readonly MindOfSpaceContext mindOfSpaceContext;

//         public ConnectionKeyHandler(MindOfSpaceContext mindOfSpaceContext)
//         {
//             this.mindOfSpaceContext = mindOfSpaceContext;
//         }

//         public async Task<string> CreateKey(Player player)
//         {
//             var key =  "MOS" + "_" + player.Username + "_" + player.Id + "_" + player.Role + "_" + DateTimeOffset.UtcNow;
            
//             var oldKeys = await mindOfSpaceContext.PlayerConnectionKeys.Where(x => x.PlayerId == player.Id && x.IsDeleted != true).ToListAsync();
//             foreach (var keys in oldKeys)
//             {
//                 keys.IsDeleted = true;
//             }
//             mindOfSpaceContext.PlayerConnectionKeys.Add(new PlayerConnectionKey {PlayerId = player.Id, Player = player, ConnectionKey = key});
//             if(await mindOfSpaceContext.SaveChangesAsync() > 0 ? true : false)
//                 return key;

//             return "";
//         }

//         public async Task<bool> ValidCurrentKey(Player player, string key) 
//         {
//             var lastKeys = await mindOfSpaceContext.PlayerConnectionKeys.Where(x => x.PlayerId == player.Id && x.IsDeleted != true).ToListAsync();
//             var lastKey =  lastKeys.LastOrDefault();

//             if(key == lastKey.ConnectionKey)
//                 return true;

//             return false;
//         }

//         public bool ValidKey(string key)
//         {
//             var splitKey = SplitKey(key);

//             if(splitKey[0].StartsWith("MOS"))
//                 return false;

//             var testPlayerid = 0;
//             if(!int.TryParse(splitKey[2], out testPlayerid))
//                 return false;
            
//             var testRoleNumber = 0;
//             var testRole = Roles.None;
//             if(!int.TryParse(splitKey[3], out testRoleNumber))
//                 return false;
            
//             if(!Enum.TryParse<Roles>(splitKey[3], out testRole))
//                 return false;
            
//             var testDate = new DateTimeOffset();
//             if(!DateTimeOffset.TryParse(splitKey[4], out testDate))
//                 return false;

//             return true;
//         }

//         public string GetUsername(string key) => SplitKey(key) != null ? SplitKey(key)[1] : "";

//         public int GetPlayerId(string key) => SplitKey(key) != null ? int.Parse(SplitKey(key)[2]) : -1;

//         public Roles GetRole(string key) => SplitKey(key) != null ? Enum.Parse<Roles>(SplitKey(key)[3]) : Roles.None;

//         public DateTimeOffset GetCreatedDate(string key) => SplitKey(key) != null ? DateTimeOffset.Parse(SplitKey(key)[4]) : new DateTimeOffset();

//         private string[] SplitKey(string key) => (key.StartsWith("MOS") && key.Split("_").Length == 5) ? key.Split("_") : null;
//     }
// }