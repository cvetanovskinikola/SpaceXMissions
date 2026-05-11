using Microsoft.EntityFrameworkCore;
using SpaceXProject.API.Data;
using SpaceXProject.API.Models;
using SpaceXProject.API.Repositories.Interfaces;

namespace SpaceXProject.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<bool> EmailExistsAsync(string email)
            => _context.Users.AnyAsync(u => u.Email == email);

        public Task<User?> GetByEmailAsync(string email)
            => _context.Users.SingleOrDefaultAsync(u => u.Email == email);

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }

}
