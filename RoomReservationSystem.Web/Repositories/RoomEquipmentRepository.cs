using RoomReservationSystem.Shared.Models;
using Microsoft.Data.Sqlite;
using Dapper;
using RoomReservationSystem.Shared.DTOs.Equipment;

namespace RoomReservationSystem.Web.Repositories
{
    public class RoomEquipmentRepository : IRoomEquipmentRepository
    {
        private readonly string connectionString;

        public RoomEquipmentRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task CreateAsync(RoomEquipment roomEquipment)
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.ExecuteAsync("""
                INSERT INTO RoomEquipment (RoomId, EquipmentId, Quantity)
                VALUES (@RoomId, @EquipmentId, @Quantity);
                """, roomEquipment);
        }

        public async Task DeleteAsync(int roomId, int equipmentId)
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.ExecuteAsync("""
                DELETE FROM RoomEquipment
                WHERE RoomId = @RoomId and EquipmentId = @EquipmentId;
                """, new { RoomId = roomId, EquipmentId = equipmentId });
        }

        public async Task DeleteByRoomAsync(int roomId)
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.ExecuteAsync("""
                DELETE FROM RoomEquipment
                WHERE RoomId = @RoomId;
                """, new { RoomId = roomId });
        }

        public async Task<IEnumerable<EquipmentDto>> GetByRoomAsync(int roomId)
        {
            using var connection = new SqliteConnection(connectionString);

            // TODO
            return await connection.QueryAsync<EquipmentDto>("""
                SELECT e.Id as Id, e.Name as Name, re.Quantity as Quantity
                FROM RoomEquipment re
                JOIN Equipment e ON e.Id = re.EquipmentId 
                WHERE RoomId = @RoomId;
                """, new { RoomId = roomId });
        }

        public async Task UpdateAsync(RoomEquipment roomEquipment)
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
