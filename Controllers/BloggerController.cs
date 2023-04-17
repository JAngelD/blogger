using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bloggers.Interfaces;
using bloggers.DTO;
using Microsoft.EntityFrameworkCore;
using bloggers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

namespace bloggers.Controllers
{
    //[Route("[controller]")]
    public class BloggerController : Controller
    {
        private readonly IBloggerService _blogger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BloggerController(IBloggerService blogger, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _blogger = blogger;
        }

        public IActionResult Index()
        {   
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetBloggers()
        {
            IEnumerable<BloggerDTO> bloggers = await _blogger.GetBloggers();
            return Ok(bloggers);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBlogger([FromForm]BloggerDTO bloggerDTO)
        {
            if(ModelState.IsValid){
                //Save image to wwwroot/img
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(bloggerDTO.ImageFile.FileName);
                string extension = Path.GetExtension(bloggerDTO.ImageFile.FileName);
                bloggerDTO.Picture = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/img/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create)){
                    await bloggerDTO.ImageFile.CopyToAsync(fileStream);
                } 
            }
            await _blogger.SaveBlogger(bloggerDTO);
            return Ok(new {success = true});
        }

        [HttpPost]
        public async Task<IActionResult> SearchBlogger([FromForm]SearchBloggers search)
        {
            IEnumerable<BloggerDTO> blogger = await _blogger.SearchBlogger(search);
            return Ok(blogger);
        }
        
        [HttpGet]
        [Route("Blogger/GetBlogger/{idBlogger}")]
        public async Task<IActionResult> Getblogger([FromRoute]Guid idBlogger){
            BloggerDTO blogger = await _blogger.GetBlogger(idBlogger);
            return Ok(blogger);
        }

        [HttpGet]
        [Route("/Blogger/DeleteBlogger/{idBlogger}")]
        public async Task<IActionResult> DeleteBlogger(Guid idBlogger)
        {
            //search a blogger
            var blogger = await _blogger.GetBlogger(idBlogger);
            if(blogger.Picture != null){
                //delete from wwwroot/img
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath,"img",blogger.Picture);
                if(System.IO.File.Exists(imagePath)){
                    System.IO.File.Delete(imagePath);
                }
            }
            //delete the blogger
            await _blogger.DeleteBlogger(idBlogger);
            return Ok();
        }
        public async Task<IActionResult> List()
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}