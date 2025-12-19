using System;
using System.Collections.Generic;

namespace StrivoLabsTest.Data.Models;

public partial class ServiceToken
{
    public int Id { get; set; }

    public Guid ServiceId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid Uid { get; set; }

    public virtual Service Service { get; set; } = null!;
}
