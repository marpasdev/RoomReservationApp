using RoomReservationSystem.Shared.DTOs.Rooms;
using RoomReservationSystem.Shared.Models;
using System.Windows;

namespace RoomReservationSystem.Admin
{
    public partial class AddRoomDialog : Window
    {
        public CreateRoomRequest? Result { get; private set; }
        private List<EquipmentSelectionItem> equipmentItems = new();

        public AddRoomDialog(List<Equipment> allEquipment)
        {
            InitializeComponent();
            equipmentItems = allEquipment.Select(e => new EquipmentSelectionItem()
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            EquipmentGrid.ItemsSource = equipmentItems;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Name is required.");
                return;
            }
            if (!int.TryParse(CapacityBox.Text, out int capacity) || capacity <= 0)
            {
                MessageBox.Show("Capacity must be a positive number.");
                return;
            }
            if (!int.TryParse(MaxDurationBox.Text, out int maxDuration) || maxDuration <= 0)
            {
                MessageBox.Show("Max duration must be a positive number.");
                return;
            }

            Result = new CreateRoomRequest()
            {
                Name = NameBox.Text,
                Capacity = capacity,
                MaxReservationMinutes = maxDuration,
                Equipment = equipmentItems
                    .Where(e => e.IsSelected)
                    .Select(e => new RoomEquipmentRequest()
                    {
                        EquipmentId = e.Id,
                        Quantity = e.Quantity
                    }).ToList()
            };

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
