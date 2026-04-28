using RoomReservationSystem.Shared.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace RoomReservationSystem.Web.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly string connectionString;

        public ReservationRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<int> CreateAsync(Reservation reservation)
        {
            using var connection = new SqliteConnection(connectionString);

            return await connection.ExecuteScalarAsync<int>("""
                INSERT INTO Reservation (RoomId, Start, End, Purpose, PersonCount, BookerId, Status, CreatedAt)
                VALUES (@RoomId, @Start, @End, @Purpose, @PersonCount, @BookerId, @Status, @CreatedAt);
                SELECT last_insert_rowid();
                """, reservation);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.ExecuteAsync("""
                DELETE FROM Reservation
                WHERE Id = @Id;
                """, new { Id = id });
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            using var connection = new SqliteConnection(connectionString);

            return await connection.QueryAsync<Reservation>("""
                SELECT *
                FROM Reservation;
                """);
        }

        public async Task<IEnumerable<Reservation>> GetByUserAsync(int userId)
        {
            using var connection = new SqliteConnection(connectionString);

            return await connection.QueryAsync<Reservation>("""
                SELECT *
                FROM Reservation
                WHERE BookerId = @BookerId;
                """, new { BookerId = userId });
        }

        public async Task<Reservation?> GetCurrentAsync(int roomId)
        {
            using var connection = new SqliteConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Reservation>("""
                SELECT *
                FROM Reservation
                WHERE RoomId = @RoomId and (Start <= @Now and End > @Now);
                """, new { RoomId = roomId, Now = DateTime.UtcNow });
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.ExecuteAsync("""
                UPDATE Reservation
                SET RoomId = @RoomId,
                    Start = @Start,
                    End = @End,
                    Purpose = @Purpose,
                    PersonCount = @PersonCount, 
                    Status = @Status
                WHERE Id = @Id;
                """, reservation);
        }
    }
}
