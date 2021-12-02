using System.Threading.Tasks;

namespace PKM.Service
{
    public interface IAuthenticationService
    {
        public string Login(string login, string password);
    }
}