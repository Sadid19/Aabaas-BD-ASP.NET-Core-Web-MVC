using System.ComponentModel.DataAnnotations;
using BLL.Validations;

namespace BLL.DTOs
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int HotelId { get; set; }

        [Required]
        [CheckInDateValidation]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }

        [Required]
        [CheckOutDateValidation]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }

        public decimal TotalCost { get; set; }
        public string Status { get; set; } = "Pending";

        public string? HotelName { get; set; }
        public string? HotelCity { get; set; }
        public string? UserEmail { get; set; }

        public decimal PricePerNight { get; set; }
        public int Nights { get; set; }
        public bool CanCancel { get; set; }
    }
}
