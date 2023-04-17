using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace bloggers.DTO
{
    public class FriendDTO
    {
        public Guid Id { get; set; }

        public Guid BloggerId { get; set; }

        [Display(Name ="FriendId")]
        [Required(ErrorMessage = "Friend Required")]
        public Guid FriendId { get; set; }

        public string? FriendName { get; set; }

        public string? FriendEmail { get; set; }

        public string? FriendWebsite { get; set; }

    }
}