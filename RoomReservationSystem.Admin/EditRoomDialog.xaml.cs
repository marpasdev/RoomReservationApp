using RoomReservationSystem.Shared.DTOs.Rooms;
using RoomReservationSystem.Shared.Models;
using System.Windows;

namespace RoomReservationSystem.Admin
{
    public partial class EditRoomDialog : Window
    {
        public UpdateRoomRequest? Result { get; private set; }
        private readonly RoomDto room;
        private List<EquipmentSelectionItem> equipmentItems = new();

        public EditRoomDialog(RoomDto room, List<Equipment> allEquipment)
        {
            InitializeComponent();
            this.room = room;

            // předvyplň základní pole
            NameBox.Text = room.Name;
            CapacityBox.Text = room.Capacity.ToString();
            MaxDurationBox.Text = room.MaxReservationMinutes.ToString();

            // předvyplň vybavení
            equipmentItems = allEquipment.Select(e => new EquipmentSelectionItem()
            {
                Id = e.Id,
                Name = e.Name,
                IsSelected = room.Equipment.Any(re => re.Id == e.Id),
                Quantity = room.Equipment.FirstOrDefault(re => re.Id == e.Id)?.Quantity ?? 1
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

            Result = new UpdateRoomRequest()
            {
                Id = room.Id,
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
