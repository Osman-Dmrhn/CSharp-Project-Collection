using BlogAngApi.Models;

namespace BlogAngApi.Services
{
    public interface IUserService
    {
        List<User> GetAll();
        bool UserAuth(string mail, string pass);
        User GetUser(Guid id);

        User GetUserbyEmail(string email);
        void AddUser(User user);
        void RemoveUser(Guid id);
        void UpdateUser(User user);

        void EditPass(User user,string pass);

        void EditPhoto(User user);
    }
}
