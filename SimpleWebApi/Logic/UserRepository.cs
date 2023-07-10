using Microsoft.EntityFrameworkCore;
using SimpleWebApi.Logic.Helpers;
using SimpleWebApi.Logic.Interfaces;
using SimpleWebApi.Models.Entities;

namespace SimpleWebApi.Logic
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public User GetUser(string email)
        {
            var user = _applicationDbContext.Users.Where(x => x.Email == email).Include(x => x.Roles).ThenInclude(x=>x.UserRole).FirstOrDefault();
            return user;
        }

        public bool IsValidUser(string email, string password)
        {
            var user = _applicationDbContext.Users.Where(x => x.Email == email).FirstOrDefault();

            if(user==null)
                return false;

            var hashedPassword = HashingHelper.HashPassword(password, user.Salt);

            if (hashedPassword != user.HashedPassword)
                return false;

            return true;
        }
    }
}
