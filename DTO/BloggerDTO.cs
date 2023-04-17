using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace bloggers.DTO
{
    public class BloggerDTO
    {
        public Guid Id { get; set; }

        [Display(Name ="Name")]
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; } = null!;

        [Display(Name ="Email")]
        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; } = null!;

        [Display(Name ="Website")]
        [Required(ErrorMessage = "Website Required")]
        public string Website { get; set; } = null!;

        public string? Picture { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        }
    public partial class SearchBloggers
    {
        [Display(Name = "Search")]
        public String BloggerSearch { get; set; }    
    }
}