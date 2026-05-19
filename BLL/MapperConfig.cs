using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;

namespace BLL
{
    public class MapperConfig
    {
        public static MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserDTO>().ReverseMap()
                .ForMember(d => d.UserId, o => o.Ignore());

            cfg.CreateMap<Hotel, HotelDTO>().ReverseMap()
                .ForMember(d => d.Bookings, o => o.Ignore())
                .ForMember(d => d.HotPackages, o => o.Ignore());

            cfg.CreateMap<Booking, BookingDTO>()
                .ForMember(d => d.CheckIn, o => o.MapFrom(s => s.CheckIn.ToDateTime(TimeOnly.MinValue)))
                .ForMember(d => d.CheckOut, o => o.MapFrom(s => s.CheckOut.ToDateTime(TimeOnly.MinValue)))
                .ForMember(d => d.HotelName, o => o.MapFrom(s => s.Hotel != null ? s.Hotel.Name : null))
                .ForMember(d => d.HotelCity, o => o.MapFrom(s => s.Hotel != null ? s.Hotel.City : null))
                .ForMember(d => d.UserEmail, o => o.MapFrom(s => s.User != null ? s.User.Email : null))
                .ForMember(d => d.PricePerNight, o => o.MapFrom(s => s.Hotel != null ? s.Hotel.PricePerNight : 0))
                .ForMember(d => d.Nights, o => o.MapFrom(s => s.CheckOut.DayNumber - s.CheckIn.DayNumber));

            cfg.CreateMap<BookingDTO, Booking>()
                .ForMember(d => d.CheckIn, o => o.MapFrom(s => DateOnly.FromDateTime(s.CheckIn)))
                .ForMember(d => d.CheckOut, o => o.MapFrom(s => DateOnly.FromDateTime(s.CheckOut)))
                .ForMember(d => d.Hotel, o => o.Ignore())
                .ForMember(d => d.User, o => o.Ignore());

            cfg.CreateMap<HotPackage, HotPackageDTO>()
                .ForMember(d => d.ValidUntil, o => o.MapFrom(s => s.ValidUntil.ToDateTime(TimeOnly.MinValue)))
                .ForMember(d => d.HotelName, o => o.MapFrom(s => s.Hotel != null ? s.Hotel.Name : null))
                .ForMember(d => d.HotelCity, o => o.MapFrom(s => s.Hotel != null ? s.Hotel.City : null));

            cfg.CreateMap<HotPackageDTO, HotPackage>()
                .ForMember(d => d.ValidUntil, o => o.MapFrom(s => DateOnly.FromDateTime(s.ValidUntil)))
                .ForMember(d => d.Hotel, o => o.Ignore());
        });

        public static Mapper GetMapper()
        {
            return new Mapper(config);
        }
    }
}
