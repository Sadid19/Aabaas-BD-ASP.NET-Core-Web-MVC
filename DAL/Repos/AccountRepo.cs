using DAL.EF;
using DAL.EF.Tables;
using System.Collections.Generic;

namespace DAL.Repos
{
    public class AccountRepo
    {
        AabaasBdContext db;

        public AccountRepo(AabaasBdContext db)
        {
            this.db = db;
        }

        public User GetByEmail(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return db.Users.FirstOrDefault(u => u.UserId == id);
        }

        public User Login(string email, string password)
        {
            return db.Users.FirstOrDefault(u => u.Email == email && u.UserPassword == password);
        }
        public bool EmailExists(string email)
        {
            return db.Users.Any(u => u.Email == email);
        }

        public User Register(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user;
        }
    }
}
