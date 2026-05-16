using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class HotPackage
{
    public int PackageId { get; set; }

    public int HotelId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateOnly ValidUntil { get; set; }

    public virtual Hotel Hotel { get; set; } = null!;
}
