using BlogAngApi.Areas.AdminArea.Models;
using BlogAngApi.Models;

namespace BlogAngApi.Service
{
    public interface IAdminServices
    {
        bool AdminAuth(string kull, string pas);

        List<Admin> GetAdmins();
        bool addUser(Admin admin);

        string GetRoleByEmail(string email);

        Admin GetAdmin(Guid id);
        bool removeUser(Guid id);
        bool updateUser(Admin admin);

        
    }
}
