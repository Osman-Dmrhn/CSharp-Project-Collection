using BlogAngApi.Models;
using BlogAngApi.Db;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using BlogAngApi.Areas.AdminArea.Models;
using BlogAngApi.Service;

namespace BlogApplication.Areas.AdminArea.Service
{
    public class AdminService : IAdminServices
    {
        private readonly EfContext _efcontext;
        public AdminService(EfContext efcontext) {
            _efcontext = efcontext;
        }

        public List<Admin> GetAdmins() {
            var list = _efcontext.Adminler.OrderBy(x=>x.KullaniciAdi).ToList();
            return list; 
        }

        public bool addUser(Admin admin)
        {
            _efcontext.Adminler.Add(admin);
            _efcontext.SaveChanges();
            return true;
        }

        public bool AdminAuth(string email,string pass)
        {
            var boss= _efcontext.Adminler
                .FirstOrDefault(a => a.Email == email && a.Sifre == pass);
            if (boss!=null)
            {
                if (boss.Sifre == pass)
                {
                    return true;
                }
            }
            return false;           
        }

        public string GetRoleByEmail(string email)
        {
            string role = _efcontext.Adminler.Where(x=>x.Email==email).Select(z=>z.Rol).FirstOrDefault();
            Console.WriteLine(role);
            return role;
        }

        public Admin GetAdmin(Guid id)
        {
            var getir=_efcontext.Adminler.Find(id);
            return getir;
        }

        public bool removeUser(Guid id)
        {
            var getir=GetAdmin(id);
            if (getir != null)
            {
                _efcontext.Adminler.Remove(getir);
                _efcontext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool updateUser(Admin admin)
        {
            var gunc = GetAdmin(admin.Id);
            if(gunc != null)
            {
                gunc.Sifre = admin.Sifre;
                gunc.Rol=admin.Rol;
                gunc.KullaniciAdi = admin.KullaniciAdi;
                gunc.Email=admin.Email;
                _efcontext.SaveChanges();
                return true;
            }
            
            return false;
        }
    }
}
