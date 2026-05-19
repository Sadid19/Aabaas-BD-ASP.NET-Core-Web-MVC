namespace BLL.DTOs
{
    public class HotelDTO
    {
        public int HotelId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int? StarRating { get; set; }
        public decimal PricePerNight { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public string Description { get; set; }
    }
}
