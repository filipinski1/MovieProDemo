
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieProDemo.Data;
using MovieProDemo.Models.Database;
using MovieProDemo.Models.Settings;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MovieProDemo.Services
{
    public class SeedService
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        public SeedService(IOptions<AppSettings> appSettings, ApplicationDbContext dbContext, UserManager<IdentityUser> usermanager, RoleManager<IdentityRole> rolemanager)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
            _usermanager = usermanager;
            _rolemanager = rolemanager;
        }
        public async Task ManageDataAsync()
        {
            await UpdateDatabaseAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedCollections();
        }

        private async Task UpdateDatabaseAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }
        private async Task SeedRolesAsync()
        {
            if (_dbContext.Roles.Any()) return;
            var adminRole = _appSettings.MovieProSettings.DefaultCredentials.Role;
            await _rolemanager.CreateAsync(new IdentityRole(adminRole));

        }
        private async Task SeedUsersAsync()
        {
            if (_usermanager.Users.Any()) return;
            var credentials = _appSettings.MovieProSettings.DefaultCredentials;
            var newUser = new IdentityUser()
            {

                Email = credentials.Email,
                UserName = credentials.Email,
                EmailConfirmed = true
            };
            await _usermanager.CreateAsync(newUser, credentials.Password);
            await _usermanager.AddToRoleAsync(newUser, credentials.Role);
        }
        private async Task SeedCollections()
        {
            if (_dbContext.Set<MovieProDemo.Models.Database.Collection>().Any()) return;

            _dbContext.Add(new MovieProDemo.Models.Database.Collection()
            {
                Name = _appSettings.MovieProSettings.DefaultCollection.Name,
                Description = _appSettings.MovieProSettings.DefaultCollection.Description
            });
            await _dbContext.SaveChangesAsync();
        }


    }
}
