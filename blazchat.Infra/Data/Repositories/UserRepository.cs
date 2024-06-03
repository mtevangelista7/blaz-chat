using blazchat.Domain.Entities;
using blazchat.Infra.Data.Context;
using blazchat.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blazchat.Infra.Data.Repositories
{
    public class UserRepository(AplicationDbContext context) : EFRepository<User>(context), IUserRepository
    {
        public async Task<User> CreateUser(User user)
        {
            if (user.Id == Guid.Empty)
            {
                user.Id = Guid.NewGuid();
            }

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return (await context.Users.FirstOrDefaultAsync(x => x.Id == userId))!;
        }

        public Task<List<User>> GetUsers()
        {
            return context.Users.ToListAsync();
        }

        public async Task<User> GetByUsername(string username)
        {
            return (await context.Users.FirstOrDefaultAsync(x => x.Name == username))!;
        }
    }
}