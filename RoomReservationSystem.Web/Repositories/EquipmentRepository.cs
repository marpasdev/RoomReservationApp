using RoomReservationSystem.Shared.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace RoomReservationSystem.Web.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly string connectionString;

        public EquipmentRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<int> CreateAsync(Equipment equipment)
        {
            using var connection = new SqliteConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>("""
                INSERT INTO Equipment (Name)
                VALUES (@Name);
                SELECT last_insert_rowid();
                """, equipment);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.ExecuteAsync("""
                    DELETE FROM Equipment
                    WHERE Id = @Id;
                    """, new { Id = id });
        }

        public async Task<IEnumerable<Equipment>> GetAllAsync()
        {
            using var connection = new SqliteConnection(connectionString);

            return await connection.QueryAsync<Equipment>("""
                SELECT *
                FROM Equipment;
                """);
        }

        public async Task<Equipment?> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Equipment>("""
                SELECT *
                FROM Equipment
                WHERE Id = @Id;
                """, new { Id = id });
        }

        public async Task UpdateAsync(Equipment equipment)
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.ExecuteAsync("""
                UPDATE Equipment
                SET Name = @Name
                WHERE Id = @Id;
                """, equipment);
        }
    }
}
