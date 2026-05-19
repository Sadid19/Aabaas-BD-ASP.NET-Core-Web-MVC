namespace BLL.DTOs
{
    public class HotelSearchFilter
    {
        public string City { get; set; }
        public int? StarRating { get; set; }
        public string RoomType { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
