using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class Hotel
{
    public int HotelId { get; set; }

    public string Name { get; set; } = null!;

    public string City { get; set; } = null!;

    public int? StarRating { get; set; }

    public decimal PricePerNight { get; set; }

    public string RoomType { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<HotPackage> HotPackages { get; set; } = new List<HotPackage>();
}
