using System;
using System.Collections.Generic;

namespace bloggers.Models;

public partial class Friend
{
    public Guid Id { get; set; }

    public Guid BloggerId { get; set; }

    public Guid FriendId { get; set; }

    public virtual Blogger Blogger { get; set; } = null!;

    public virtual Blogger FriendNavigation { get; set; } = null!;
}
