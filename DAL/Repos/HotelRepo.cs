using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DAL.Repos
{
    public class HotelRepo
    {
        AabaasBdContext db;

        public HotelRepo(AabaasBdContext db)
        {
            this.db = db;
        }

        public List<Hotel> Get()
        {
            return db.Hotels.AsNoTracking().ToList();
        }

        public Hotel Get(int id)
        {
            return db.Hotels.Find(id);
        }

        public List<Hotel> GetFiltered(string city, int? starRating, string roomType, decimal? minPrice, decimal? maxPrice)
        {
            List<Hotel> all = Get();
            List<Hotel> result = new List<Hotel>();

            for(int i = 0; i < all.Count; i++)
            {
                Hotel h = all[i];
                bool ok = true;

                if(city != null && city.Length > 0)
                {
                    if(h.City == null || h.City.IndexOf(city, System.StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        ok = false;
                    }

                }

                if(ok && starRating.HasValue)
                {
                    if (h.StarRating != starRating.Value)
                    {
                        ok = false;
                    }
                }

                if(ok && roomType != null && roomType.Length > 0)
                {
                    if (h.RoomType != roomType)
                    {
                        ok = false;
                    }
                }

                if(ok && minPrice.HasValue)
                {
                    if(h.PricePerNight < minPrice.Value)
                    {
                        ok = false;
                    }
                }

                if(ok && maxPrice.HasValue)
                {
                    if (h.PricePerNight > maxPrice.Value)
                    {
                        ok = false;
                    }
                }

                if(ok)
                {
                    result.Add(h);
                }
            }

            return result;
        }

        public bool Create(Hotel h)
        {
            db.Hotels.Add(h);
            return db.SaveChanges() > 0;
        }

        public bool Update(Hotel h)
        {
            Hotel exobj = Get(h.HotelId);
            if(exobj == null)
            {
                return false;
            }

            db.Entry(exobj).CurrentValues.SetValues(h);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            Hotel exobj = Get(id);
            if(exobj == null)
            {
                return false;
            }

            bool hasBookings = db.Bookings.Any(b => b.HotelId == id);
            if(hasBookings)
            {
                return false;
            }

            db.Hotels.Remove(exobj);
            return db.SaveChanges() > 0;
        }
    }
}
