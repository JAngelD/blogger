using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bloggers.Models;
using bloggers.DTO;

namespace bloggers.Interfaces
{
    public interface IBloggerService
    {
    Task<IEnumerable<BloggerDTO>>GetBloggers();
    Task<BloggerDTO> GetBlogger(Guid idBlogger);
    Task DeleteBlogger(Guid idBlogger);
    Task<IEnumerable<Blogger>>GetFriends(int? id);
    Task SaveBlogger(BloggerDTO bloggerDTO);
    Task<IEnumerable<BloggerDTO>> SearchBlogger(SearchBloggers search);
    }
}