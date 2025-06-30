using BlogApplication.Areas.AdminArea.Models;

namespace BlogApplication.Areas.AdminArea.Service
{
    public interface IAdminServices
    {
        bool AdminAuth(string kull, string pas);

        string GetRole(string kull);

        List<Admin> GetAdmins();
        bool addUser(RegisterAdminModel admin);

        Admin GetAdmin(Guid id);
        bool removeUser(Guid id);
        bool updateUser(Admin admin);

        
    }
}
