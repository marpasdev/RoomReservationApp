using RoomReservationSystem.Shared.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace RoomReservationSystem.Web.Repositories
{
    public class ReservationHistoryRepository : IReservationHistoryRepository
    {
        private readonly string connectionString;

        public ReservationHistoryRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<int> CreateAsync(ReservationHistory history)
        {
            using var connection = new SqliteConnection(connectionString);

            return await connection.ExecuteScalarAsync<int>("""
                INSERT INTO ReservationHistory (ReservationId, ChangedAt, OldStatus, NewStatus, ChangedById)
                VALUES (@ReservationId, @ChangedAt, @OldStatus, @NewStatus, @ChangedById);
                SELECT last_insert_rowid();
                """, history);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.ExecuteAsync("""
                DELETE FROM ReservationHistory
                WHERE Id = @Id;
                """, new { Id = id });
        }

        public async Task<IEnumerable<ReservationHistory>> GetAllAsync()
        {
            using var connection = new SqliteConnection(connectionString);

            return await connection.QueryAsync<ReservationHistory>("""
                SELECT *
                FROM ReservationHistory;
                """);
        }

        public async Task<IEnumerable<ReservationHistory>> GetByReservationIdAsync(int reservationId)
        {
            using var connection = new SqliteConnection(connectionString);

            return await connection.QueryAsync<ReservationHistory>("""
                SELECT *
                FROM ReservationHistory
                WHERE ReservationId = @ReservationId
                ORDER BY ChangedAt DESC;
                """, new { ReservationId = reservationId });
        }

        public async Task<IEnumerable<ReservationHistory>> GetByUserAsync(int userId)
        {
            using var connection = new SqliteConnection(connectionString);

            return await connection.QueryAsync<ReservationHistory>("""
                SELECT *
                FROM ReservationHistory
                WHERE ChangedById = @ChangedById;
                """, new { ChangedById = userId });
        }
    }
}
