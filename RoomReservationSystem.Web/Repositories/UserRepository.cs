using RoomReservationSystem.Shared.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace RoomReservationSystem.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;

        public UserRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<int> CreateAsync(User user)
        {
            using var connection = new SqliteConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>("""
                INSERT INTO User (UserName, Email, PasswordHash, CreatedAt)
                VALUES (@UserName, @Email, @PasswordHash, @CreatedAt);
                SELECT last_insert_rowid();
                """, user);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.ExecuteAsync("""
                DELETE FROM User
                WHERE Id = @Id
                """, new { Id = id });
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = new SqliteConnection(connectionString);
            return await connection.QueryAsync<User>("SELECT * FROM User");
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = new SqliteConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>("""
                SELECT *
                FROM User
                WHERE Email = @Email
                """, new { Email = email });
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync("""
                SELECT *
                FROM User
                WHERE Id = @Id
                """, new { Id = id });
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            using var connection = new SqliteConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<User>("""
                SELECT *
                FROM User
                WHERE UserName = @UserName;
                """, new { UserName = userName });
        }

        public async Task UpdateAsync(User user)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.ExecuteAsync("""
                UPDATE User
                SET UserName = @UserName,
                    Email = @Email,
                    PasswordHash = @PasswordHash
                WHERE Id = @Id;
                """, user);
        }
    }
}
