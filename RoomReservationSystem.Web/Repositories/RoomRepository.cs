using RoomReservationSystem.Shared.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace RoomReservationSystem.Web.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly string connectionString;

        public RoomRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<int> CreateAsync(Room room)
        {
            using var connection = new SqliteConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>("""
                INSERT INTO Room (Name, Capacity, MaxReservationMinutes)
                VALUES (@Name, @Capacity, @MaxReservationMinutes);
                SELECT last_insert_rowid();
                """, room);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.ExecuteAsync("""
                DELETE FROM Room
                WHERE Id = @Id;
                """, new { Id = id });
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            using var connection = new SqliteConnection(connectionString);
            return await connection.QueryAsync<Room>("""
                SELECT *
                FROM Room;
                """);
        }

        public async Task<IEnumerable<Room>> GetAvailableAsync(DateTime from, DateTime to, int minCapacity)
        {
            using var connection = new SqliteConnection(connectionString);
            return await connection.QueryAsync<Room>("""
                SELECT *
                FROM Room
                WHERE Capacity >= @MinCapacity and Id not in (
                    SELECT RoomId
                    FROM Reservation
                    WHERE Status = 0 and (Start < @To and End > @From)
                );
                """, new { From = from, To = to, MinCapacity = minCapacity });
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Room>("""
                SELECT *
                FROM Room
                WHERE Id = @Id;
                """, new { Id = id });
        }

        public async Task UpdateAsync(Room room)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.ExecuteAsync("""
                UPDATE Room
                SET Name = @Name,
                    Capacity = @Capacity,
                    MaxReservationMinutes = @MaxReservationMinutes
                WHERE Id = @Id;
                """, room);
        }
    }
}
