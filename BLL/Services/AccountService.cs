using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;

namespace BLL.Services
{
    public class AccountService
    {
        AccountRepo repo;
        Mapper mapper;

        public AccountService(AccountRepo repo)
        {
            this.repo = repo;
            mapper = MapperConfig.GetMapper();
        }

        // Login: verify email and password, return user if valid
        public UserDTO Login(string email, string password)
        {
            if(email == null || password == null)
            {
                return null;
            }

            string cleanEmail = email.Trim().ToLower();
            string cleanPassword = password.Trim();

            if(cleanEmail == AdminSettings.Email.ToLower() && cleanPassword == AdminSettings.Password)
            {
                User admin = repo.GetByEmail(AdminSettings.Email);
                if(admin != null)
                {
                    return mapper.Map<UserDTO>(admin);
                }
            }

            User user = repo.Login(email, password);
            if(user == null)
            {
                return null;
            }

            return mapper.Map<UserDTO>(user);
        }

        public bool IsAdmin(string email)
        {
            return AdminSettings.IsAdminEmail(email);
        }

        public (bool Success, string Message, UserDTO User) Register(UserDTO dto)
        {

            if(repo.EmailExists(dto.Email))
            {
                return (false, "Email is already registered!", null);
            }

            User entity = mapper.Map<User>(dto);
            User created = repo.Register(entity);
            UserDTO result = mapper.Map<UserDTO>(created);
            return (true, "Registration is completed!", result);
        }

        public UserDTO GetById(int id)
        {
            User user = repo.GetById(id);
            if (user == null)
            {
                return null;
            }
            return mapper.Map<UserDTO>(user);
        }
    }
}
