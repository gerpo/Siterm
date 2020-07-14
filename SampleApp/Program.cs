using System;
using System.Linq;
using Siterm.Domain.Models;
using Siterm.EntityFramework;
using Siterm.EntityFramework.Services;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var userService = new GenericDataService<User>(new SitermDbContextFactory());
            userService.Create(new User {Email = "test@web.de"}).Wait();
            Console.WriteLine(userService.Update(3, new User {Email= "new@qwebeb.de"}).Result);
        }
    }
}
