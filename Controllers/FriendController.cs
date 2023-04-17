using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bloggers.Interfaces;
using bloggers.Models;
using bloggers.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bloggers.Controllers
{
    ////[Route("[controller]")]
    public class FriendController : Controller
    {
        private readonly IFriendService _friend;
        private readonly IBloggerService _blogger;

        public FriendController(IFriendService friend,IBloggerService blogger)
        {
            _friend = friend;
            _blogger = blogger;
        }

        public async Task<IActionResult> Index()
        {
             var bloggers = (from blogger in await _blogger.GetBloggers()
                                select new SelectListItem()
                                {
                                    Text = blogger.Name,
                                    Value = blogger.Id.ToString(),
                                }).ToList();
            //var bloggers = new SelectList(await _blogger.GetBloggers());
            ViewBag.FriendId = bloggers;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveFriend([FromForm]FriendDTO friendDTO)
        {
            await _friend.SaveFriend(friendDTO);
            return Ok(new {success = true});
        }
        [HttpGet]
        [Route("Friend/GetFriends/{idBlogger}")]
        public async Task<IActionResult> GetFriends([FromRoute]Guid idBlogger){
            IEnumerable<FriendDTO> friend = await _friend.GetFriends(idBlogger);
            return Ok(friend);
        }
        [HttpGet]
        [Route("Friend/SearchFriends/{idBlogger}")]
        public async Task<IActionResult> SearchFriends([FromRoute]Guid idBlogger){
            IEnumerable<FriendDTO> friend = await _friend.SearchFriends(idBlogger);
            return Ok(friend);
        }
        [HttpGet]
        [Route("/Friend/DeleteFriend/{id}")]
        public async Task<IActionResult> DeleteFriend(Guid id)
        {
            //delete the blogger
            await _friend.DeleteFriend(id);
            return Ok();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}