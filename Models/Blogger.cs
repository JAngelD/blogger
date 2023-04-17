using System;
using System.Collections.Generic;

namespace bloggers.Models;

public partial class Blogger
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Website { get; set; } = null!;

    public string? Picture { get; set; }

    public virtual ICollection<Friend> FriendBloggers { get; } = new List<Friend>();

    public virtual ICollection<Friend> FriendFriendNavigations { get; } = new List<Friend>();
}
