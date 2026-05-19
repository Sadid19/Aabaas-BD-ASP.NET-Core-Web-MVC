namespace BLL.DTOs
{
    public class HotPackageDTO
    {
        public int PackageId { get; set; }
        public int HotelId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime ValidUntil { get; set; }
        public string? HotelName { get; set; }
        public string? HotelCity { get; set; }
    }
}
