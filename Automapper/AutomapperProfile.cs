using AutoMapper;
using bloggers.DTO;
using bloggers.Models;


public class AutomapperProfile: Profile
    {
        public AutomapperProfile(){
            CreateMap<Blogger, BloggerDTO>().ReverseMap();
            CreateMap<Friend, FriendDTO>().AfterMap((friend,frienddto) => {
                    Blogger b = friend.FriendNavigation;
                    frienddto.FriendEmail = b.Email;
                    frienddto.FriendName = b.Name;
                    frienddto.FriendWebsite = b.Website;
            }).ReverseMap();
        }
    }