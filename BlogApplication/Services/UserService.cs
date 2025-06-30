using BlogApplication.Db;
using BlogApplication.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.Net.Mime.MediaTypeNames;

namespace BlogApplication.Services
{
    public class UserService:IUserService
    {
        private readonly EfContext _efContext;

        public UserService(EfContext efContext)
        {
            _efContext = new EfContext();
        }

        public User GetUser(Guid id)
        {
            var istenen = _efContext.Kullanıcılar.FirstOrDefault(x => x.Id == id);
            return istenen;
        }

        public User GetUserbyEmail(string email)
        {
            var istenen = _efContext.Kullanıcılar.FirstOrDefault(x => x.Email == email);
            return istenen;
        }

        public void AddUser(User user)
        {
            user.proImage = "../uploads/logo.jpg";
            _efContext.Kullanıcılar.Add(user);
            _efContext.SaveChanges();
        }

        public void RemoveUser(Guid id)
        {
            var userToRemove = _efContext.Kullanıcılar.Find(id);
            if (userToRemove is null)
                return;

            _efContext.Kullanıcılar.Remove(userToRemove);
            _efContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            var güncellenecek = _efContext.Kullanıcılar.Find(user.Id);
            if (güncellenecek != null)
            {
                güncellenecek.Username = user.Username;
                güncellenecek.Email = user.Email;

                _efContext.Kullanıcılar.Update(güncellenecek);
                try
                {
                    Console.WriteLine("Günc Başarılı");
                    _efContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {ex.Message}");
                }
                
            }         
        }

        public List<User> GetAll()
        {
            return _efContext.Kullanıcılar.OrderBy(u => u.Username).ToList();
        }

        public bool UserAuth(string mail,string pass)
        {
            var cont = _efContext.Kullanıcılar.FirstOrDefault(u => u.Email == mail);
            if (cont != null)
            {
                if (cont.PasswordHash == pass)
                {
                    return true;
                }
            }
            return false;
        }

        public void EditPass(User user,string pass)
        {
            var güncellenecek = _efContext.Kullanıcılar.Find(user.Id);
            if (güncellenecek != null)
            {
                güncellenecek.PasswordHash = pass;
            }
            _efContext.Kullanıcılar.Update(güncellenecek);
            _efContext.SaveChanges();
        }

        public void EditPhoto(User user)
        {
            var güncellenecek = _efContext.Kullanıcılar.Find(user.Id);
            if (güncellenecek != null)
            {
                güncellenecek.proImage = user.proImage;
            }
            _efContext.Kullanıcılar.Update(güncellenecek);
            _efContext.SaveChanges();
        }
    }
}
