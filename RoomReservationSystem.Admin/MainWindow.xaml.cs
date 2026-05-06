using RoomReservationSystem.Shared.DTOs.Reservations;
using RoomReservationSystem.Shared.DTOs.Rooms;
using RoomReservationSystem.Shared.DTOs.Users;
using RoomReservationSystem.Shared.Models;
using System.Windows;
using System.Windows.Controls;

namespace RoomReservationSystem.Admin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApiClient apiClient = App.ApiClient;

        public MainWindow()
        {
            InitializeComponent();
            LoadRoomsAsync();
            LoadReservationsAsync();
            LoadEquipmentAsync();
        }

        private async void LoadRoomsAsync()
        {
            try
            {
                var rooms = await apiClient.GetAllRoomsAsync();
                RoomsGrid.ItemsSource = rooms;
                RoomsStatus.Text = $"Total rooms: {rooms.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void RefreshRooms_Click(object sender, RoutedEventArgs e)
        => LoadRoomsAsync();

        private async void DeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            RoomDto? room = RoomsGrid.SelectedItem as RoomDto;
            if (room is null)
            {
                MessageBox.Show("Select a room to be deleted.");
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete room {room.Name}?",
                "Confirm", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                await apiClient.DeleteRoomAsync(room.Id);
                LoadRoomsAsync();
                LoadUsersAsync();
            }
        }

        private async void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            var allEquipment = await apiClient.GetAllEquipmentAsync();
            var dialog = new AddRoomDialog(allEquipment);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                await apiClient.CreateRoomAsync(dialog.Result!);
                LoadRoomsAsync();
            }
        }

        private async void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            var room = RoomsGrid.SelectedItem as RoomDto;
            if (room is null)
            {
                MessageBox.Show("Please select a room to edit.");
                return;
            }

            var allEquipment = await apiClient.GetAllEquipmentAsync();
            var dialog = new EditRoomDialog(room, allEquipment);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                await apiClient.UpdateRoomAsync(dialog.Result!);
                LoadRoomsAsync();
            }
        }

        private void ShowStatistics_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsGrid.SelectedItem is not RoomDto room)
            {
                MessageBox.Show("Please select a room.");
                return;
            }

            var dialog = new StatisticsDialog(room, apiClient);
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        private async void LoadUsersAsync()
        {
            try
            {
                var users = await apiClient.GetAllUsersAsync();
                UsersGrid.ItemsSource = users;
                UsersStatus.Text = $"Total users: {users.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private async void UsersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersGrid.SelectedItem is not UserDto user) return;
            try
            {
                var reservations = await apiClient.GetByUserAsync(user.Id);
                UserReservationsGrid.ItemsSource = reservations;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private async void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is not UserDto user)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete {user.UserName}?",
                "Confirm", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                await apiClient.DeleteUserAsync(user.Id);
                LoadUsersAsync();
            }
        }
        private void RefreshUsers_Click(object sender, RoutedEventArgs e)
            => LoadUsersAsync();

        private async void LoadReservationsAsync()
        {
            try
            {
                var reservations = await apiClient.GetAllReservations();
                ReservationsGrid.ItemsSource = reservations;
                ReservationsStatus.Text = $"Total reservations: {reservations.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private async void DeleteReservation_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationsGrid.SelectedItem is not ReservationDto reservation)
            {
                MessageBox.Show("Please select a reservation to delete.");
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete this reservation?",
                "Confirm", MessageBoxButton.YesNo);

            MessageBox.Show($"Deleting reservation Id: {reservation.Id}");

            if (result == MessageBoxResult.Yes)
            {
                await apiClient.DeleteReservationAsync(reservation.Id);
                LoadReservationsAsync();
            }
        }

        private void RefreshReservations_Click(object sender, RoutedEventArgs e)
            => LoadReservationsAsync();

        private async void LoadEquipmentAsync()
        {
            try
            {
                var equipment = await apiClient.GetAllEquipmentAsync();
                EquipmentGrid.ItemsSource = equipment;
                EquipmentStatus.Text = $"Total equipment: {equipment.Count}";
            }
            catch (Exception)
            {
                EquipmentStatus.Text = "Cannot connect to API.";
            }
        }

        private async void AddEquipment_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEquipmentDialog();
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                await apiClient.CreateEquipmentAsync(dialog.Result!);
                LoadEquipmentAsync();
            }
        }

        private async void DeleteEquipment_Click(object sender, RoutedEventArgs e)
        {
            var equipment = EquipmentGrid.SelectedItem as Equipment;
            if (equipment == null)
            {
                MessageBox.Show("Please select equipment to delete.");
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete {equipment.Name}?",
                "Confirm", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                await apiClient.DeleteEquipmentAsync(equipment.Id);
                LoadEquipmentAsync();
            }
        }

        private void RefreshEquipment_Click(object sender, RoutedEventArgs e)
            => LoadEquipmentAsync();
    }
}