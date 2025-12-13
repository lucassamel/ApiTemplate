using ApiTemplate.Domain.Entities;
using ApiTemplate.Domain.Interfaces;
using ApiTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiTemplate.Infrastructure.Repositories
{
    public class UserReposotiry(AppDbContext dbContext) : IUserRepository
    {
        public async Task<User> GetUserByUsername(string username)
        {
            try
            {
                return await dbContext
                    .Users
                    .AsNoTracking()
                    .Where(u => u.Username == username)
                    .Select(x => new User
                    {
                        Username = username,
                        Email = x.Email,
                        PasswordHash = x.PasswordHash,
                    })
                    .FirstOrDefaultAsync() ?? throw new Exception("Usuário não encontrado");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
