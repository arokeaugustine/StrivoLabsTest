using System;
using System.Collections.Generic;

namespace StrivoLabsTest.Data.Models;

public partial class Subscriber
{
    public int Id { get; set; }

    public Guid ServiceId { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public bool IsSubscribed { get; set; }

    public DateTime? SubscribedAt { get; set; }

    public DateTime? UnsubscribedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Service Service { get; set; } = null!;
}
