using RoomReservationSystem.Shared.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace RoomReservationSystem.Web.Repositories
{
    public class RoomEquipmentRepository : IRoomEquipmentRepository
    {
        private readonly string connectionString;

        public RoomEquipmentRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task CreateRoomEquipmentAsync(RoomEquipment roomEquipment)
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.ExecuteAsync("""
                INSERT INTO RoomEquipment (RoomId, EquipmentId, Quantity)
                VALUES (@RoomId, @EquipmentId, @Quantity);
                """, roomEquipment);
        }

        public async Task DeleteRoomEquipmentAsync(int roomId, int equipmentId)
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.ExecuteAsync("""
                DELETE FROM RoomEquipment
                WHERE RoomId = @RoomId and EquipmentId = @EquipmentId;
                """, new { RoomId = roomId, EquipmentId = equipmentId });
        }

        public async Task<IEnumerable<RoomEquipment>> GetByRoomAsync(int roomId)
        {
            using var connection = new SqliteConnection(connectionString);

            return await connection.QueryAsync<RoomEquipment>("""
                SELECT *
                FROM RoomEquipment
                WHERE RoomId = @RoomId;
                """, new { RoomId = roomId });
        }

        public async Task UpdateRoomEquipmentAsync(RoomEquipment roomEquipment)
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.ExecuteAsync("""
                UPDATE RoomEquipment
                SET Quantity = @Quantity
                WHERE RoomId = @RoomId and EquipmentId = @EquipmentId;
                """, roomEquipment);
        }
    }
}
