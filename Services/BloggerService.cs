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
    public class BloggerService : IBloggerService
    {
        private readonly IUnitOfWork _uok;
        private readonly IMapper _mapper;
        //private readonly BloggerTestContext _context;

        public BloggerService(IUnitOfWork uok, IMapper mapper)
        {
            _uok = uok;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BloggerDTO>> GetBloggers()
        {
            IEnumerable<Blogger> bloggers = await _uok.GetRepository<Blogger>().GetAllAsync();
            return _mapper.Map<IEnumerable<BloggerDTO>>(bloggers);
        }

        public Task<IEnumerable<Blogger>> GetFriends(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveBlogger(BloggerDTO bloggerDTO)
        {
                Blogger blogger = _mapper.Map<Blogger>(bloggerDTO);
                if(blogger.Id != Guid.Empty){
                    _uok.GetRepository<Blogger>().Update(blogger);
                }else{
                    await _uok.GetRepository<Blogger>().AddAsync(blogger);
                }
                await _uok.SaveChangesAsync();
        }

        public async Task<IEnumerable<BloggerDTO>> SearchBlogger(SearchBloggers search)
        {
            IEnumerable<Blogger> blogger = await _uok.GetRepository<Blogger>().GetAsync(x => 
                (search.BloggerSearch != null ? x.Email.Contains(search.BloggerSearch) : true) ||
                (search.BloggerSearch != null ? x.Website.Contains(search.BloggerSearch) : true));
            return _mapper.Map<IEnumerable<BloggerDTO>>(blogger);
        }
        public async Task<BloggerDTO> GetBlogger(Guid idBlogger)
        {
            Blogger blogger = await _uok.GetRepository<Blogger>().GetByIdAsync(idBlogger);
            return _mapper.Map<BloggerDTO>(blogger);
        }
        public async Task DeleteBlogger(Guid idBlogger)
        {
            Blogger blogger = await _uok.GetRepository<Blogger>().GetByIdAsync(idBlogger);
            _uok.GetRepository<Blogger>().Remove(blogger);
            await _uok.SaveChangesAsync();
        }
    }
}