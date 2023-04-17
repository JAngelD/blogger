using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bloggers.Interfaces;
using bloggers.Models;
using bloggers.DTO;
using bloggers.UnitOfWork.Interfaces;
using AutoMapper;
namespace bloggers.Services
{
    public class FriendService : IFriendService
    {
        private readonly IUnitOfWork _uok;
        private readonly IMapper _mapper;
        //private readonly BloggerTestContext _context;

        public FriendService(IUnitOfWork uok, IMapper mapper)
        {
            _uok = uok;
            _mapper = mapper;
        }
        public async Task<IEnumerable<FriendDTO>> GetFriends(Guid idBlogger)
        {
            var friend = await _uok.GetRepository<Friend>().GetAsync(x => 
                idBlogger == x.BloggerId,includeProperties: "FriendNavigation");
            return _mapper.Map<IEnumerable<FriendDTO>>(friend);
        }

        public async Task SaveFriend(FriendDTO friendDTO)
        {
            Friend friend = _mapper.Map<Friend>(friendDTO);
                if(friend.Id != Guid.Empty){
                    _uok.GetRepository<Friend>().Update(friend);
                }else{
                    await _uok.GetRepository<Friend>().AddAsync(friend);
                }
                await _uok.SaveChangesAsync();
        }
        public async Task DeleteFriend(Guid id)
        {
            Friend friend = await _uok.GetRepository<Friend>().GetByIdAsync(id);
            _uok.GetRepository<Friend>().Remove(friend);
            await _uok.SaveChangesAsync();
        }

        public async Task<IEnumerable<FriendDTO>> SearchFriends(Guid idBlogger)
        {
            var friend = await _uok.GetRepository<Friend>().GetAsync(x => 
                idBlogger == x.BloggerId || idBlogger == x.FriendId,includeProperties: "FriendNavigation");
            return _mapper.Map<IEnumerable<FriendDTO>>(friend);
        }
    }
}