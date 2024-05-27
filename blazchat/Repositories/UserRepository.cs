using blazchat.Context;
using blazchat.Entities;
using blazchat.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blazchat.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected AplicationDbContext _context;
        protected DbSet<User> _dbSet;

        public UserRepository(AplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public async Task<User> CreateUser(User user)
        {
            if (user.Id == Guid.Empty)
            {
                user.Id = Guid.NewGuid();
            }

            _dbSet.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public Task<List<User>> GetUsers()
        {
            return _dbSet.ToListAsync();
        }
    }
}
