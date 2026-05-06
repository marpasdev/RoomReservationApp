using RoomReservationSystem.Shared.DTOs.Equipment;
using RoomReservationSystem.Shared.Models;
using RoomReservationSystem.Web.Repositories;

namespace RoomReservationSystem.Web.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository equipmentRepository;

        public EquipmentService(IEquipmentRepository equipmentRepository)
        {
            this.equipmentRepository = equipmentRepository;
        }

        public async Task<int> CreateAsync(CreateEquipmentRequest request)
        {
            return await equipmentRepository.CreateAsync(new Equipment()
            {
                Name = request.Name
            });
        }

        public async Task DeleteAsync(int id)
        {
            await equipmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Equipment>> GetAllAsync()
        {
            return await equipmentRepository.GetAllAsync();
        }

        public async Task UpdateAsync(UpdateEquipmentRequest request)
        {
            await equipmentRepository.UpdateAsync(new Equipment()
            {
                Id = request.Id,
                Name = request.Name
            });
        }
    }
}
