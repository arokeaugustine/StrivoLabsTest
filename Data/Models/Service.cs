using System;
using System.Collections.Generic;

namespace StrivoLabsTest.Data.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid ServiceId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ServiceToken> ServiceTokens { get; set; } = new List<ServiceToken>();

    public virtual ICollection<Subscriber> Subscribers { get; set; } = new List<Subscriber>();
}
