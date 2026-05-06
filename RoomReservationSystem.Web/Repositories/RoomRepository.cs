using Dapper;
using Microsoft.Data.Sqlite;
using RoomReservationSystem.Shared.Models;

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
                FROM Room
                ORDER BY Name ASC
                """);
        }

        public async Task<IEnumerable<Room>> GetAvailableAsync(DateTime start, DateTime end, int minCapacity)
        {
            using var connection = new SqliteConnection(connectionString);
            return await connection.QueryAsync<Room>("""
                SELECT *
                FROM Room
                WHERE Capacity >= @MinCapacity and Id not in (
                    SELECT RoomId
                    FROM Reservation
                    WHERE Status = 0 and (Start < @End and End > @Start)
                )
                ORDER BY Name ASC
                """, new { Start = start, End = end, MinCapacity = minCapacity });
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Room>("""
                SELECT *
                FROM Room
                WHERE Id = @Id
                ORDER BY Name ASC
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
