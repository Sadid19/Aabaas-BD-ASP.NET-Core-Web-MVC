using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DAL.Repos
{
    public class BookingRepo
    {
        AabaasBdContext db;

        public BookingRepo(AabaasBdContext db)
        {
            this.db = db;
        }

        public List<Booking> GetByUserId(int userId)
        {
            var list = db.Bookings.AsNoTracking().Include(b => b.Hotel).Include(b => b.User).Where(b =>b.UserId == userId).OrderByDescending(b => b.CheckIn).ToList();

            return list;
        }

        public Booking Get(int id)
        {
            return db.Bookings.Include(b => b.Hotel).Include(b => b.User).FirstOrDefault(b => b.BookingId == id);
        }

        public List<Booking> GetAllByUserId(int userId)
        {
            return db.Bookings.AsNoTracking().Include(b => b.Hotel).Where(b => b.UserId == userId).ToList();
        }

        public List<Booking> GetAll()
        {
            var list = db.Bookings.AsNoTracking().Include(b => b.Hotel).Include(b => b.User).OrderByDescending(b => b.CheckIn).ToList();

            return list;
        }

        public bool Create(Booking b)
        {
            db.Bookings.Add(b);
            return db.SaveChanges() > 0;
        }

        public bool UpdateStatus(int bookingId, string status)
        {
            Booking exobj = db.Bookings.Find(bookingId);

            if(exobj == null)
            {
                return false;
            }

            exobj.Status = status;
            return db.SaveChanges() > 0;
        }
    }
}
