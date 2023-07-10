using SimpleWebApi.Models.Entities;

namespace SimpleWebApi.Logic.Interfaces
{
    public interface IUserRepository
    {
        bool IsValidUser(string email, string password);
        User GetUser(string email);
    }
}
