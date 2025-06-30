using BlogApplication.Areas.AdminArea.Models;
using BlogApplication.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

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

        public bool addUser(RegisterAdminModel admin)
        {
            Admin eklenecek = new Admin()
            {
                Email = admin.Email,
                Id = Guid.NewGuid(),
                KullaniciAdi = admin.KullaniciAdi,
                Sifre=admin.Sifre,
                Rol=admin.Rol       
            };
            _efcontext.Adminler.Add(eklenecek);
            _efcontext.SaveChanges();
            return true;
        }

        public bool AdminAuth(string kull,string pass)
        {
            var boss= _efcontext.Adminler
                .FirstOrDefault(a => a.KullaniciAdi == kull && a.Sifre == pass);
            if (boss!=null)
            {
                if (boss.Sifre == pass)
                {
                    return true;
                }
            }
            return false;           
        }

        public string GetRole(string kull)
        {
            string rol = _efcontext.Adminler.Where(x => x.KullaniciAdi == kull).Select(x => x.Rol).FirstOrDefault();
            return rol;
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
