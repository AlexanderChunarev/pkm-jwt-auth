using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PKM.DataAccess;
using PKM.Model;

namespace PKM
{
    public class HostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceFactory;

        public HostedService(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            if (!dbContext.Users.Any())
            {
                RegisterUser(dbContext, "John Doe", "12345");
                RegisterUser(dbContext, "James Bond", "12345$");
                RegisterUser(dbContext, "Ludvig Born", "12345&");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private static void RegisterUser(UsersDbContext dbContext, string fullname, string password)
        {
            if (dbContext.UserCredentials.Any(t => t.Login == fullname)) return;
            
            var guid = Guid.NewGuid();
            dbContext.Users.Add(new User
            {
                Guid = guid,
                FullName = fullname
            });
            dbContext.UserCredentials.Add(new UserCredentials
            {
                UserId = guid,
                Login = fullname,
                Password = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)))
            });
            dbContext.SaveChanges();
        }
    }
}