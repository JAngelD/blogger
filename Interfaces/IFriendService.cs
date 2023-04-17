using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bloggers.Models;
using bloggers.DTO;


namespace bloggers.Interfaces
{
    public interface IFriendService
    {
        Task<IEnumerable<FriendDTO>>GetFriends(Guid idBlogger);

        Task<IEnumerable<FriendDTO>>SearchFriends(Guid idBlogger);
        Task SaveFriend(FriendDTO friendDTO);
        Task DeleteFriend(Guid id);
    }
}