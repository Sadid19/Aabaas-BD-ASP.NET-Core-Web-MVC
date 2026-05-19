using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DAL.Repos
{
    public class PackageRepo
    {
        AabaasBdContext db;

        public PackageRepo(AabaasBdContext db)
        {
            this.db = db;
        }

        public List<HotPackage> Get()
        {
            return db.HotPackages.AsNoTracking().Include(p => p.Hotel).ToList();
        }

        public HotPackage Get(int id)
        {
            return db.HotPackages.AsNoTracking().Include(p => p.Hotel).FirstOrDefault(p => p.PackageId == id);
        }

        public List<HotPackage> GetActive()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            return db.HotPackages.AsNoTracking().Include(p => p.Hotel).Where(p => p.ValidUntil >= today).ToList();
        }

        public List<HotPackage> GetByCity(string city)
        {
            return GetActive().Where(p => p.Hotel != null && p.Hotel.City == city).ToList();
        }

        public bool Create(HotPackage p)
        {
            db.HotPackages.Add(p);
            return db.SaveChanges() > 0;
        }

        public bool Update(HotPackage p)
        {
            HotPackage exobj = db.HotPackages.Find(p.PackageId);
            if(exobj == null)
            {
                return false;
            }

            exobj.HotelId = p.HotelId;
            exobj.Title = p.Title;
            exobj.Description = p.Description;
            exobj.Price = p.Price;
            exobj.ValidUntil = p.ValidUntil;
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            HotPackage exobj = db.HotPackages.Find(id);
            if(exobj == null)
            {
                return false;
            }

            db.HotPackages.Remove(exobj);
            return db.SaveChanges() > 0;
        }
    }
}
