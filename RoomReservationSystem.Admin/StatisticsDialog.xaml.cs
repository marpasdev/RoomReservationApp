using RoomReservationSystem.Shared.DTOs.Rooms;
using System.Windows;

namespace RoomReservationSystem.Admin
{
    /// <summary>
    /// Interaction logic for StatisticsDialog.xaml
    /// </summary>
    public partial class StatisticsDialog : Window
    {
        private readonly ApiClient apiClient;
        private readonly RoomDto room;

        public StatisticsDialog(RoomDto room, ApiClient apiClient)
        {
            InitializeComponent();
            this.room = room;
            this.apiClient = apiClient;

            RoomNameLabel.Content = room.Name;

            FromPicker.SelectedDate = DateTime.Today.AddMonths(-1);
            ToPicker.SelectedDate = DateTime.Today;
        }

        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            if (FromPicker.SelectedDate is null || ToPicker.SelectedDate is null)
            {
                MessageBox.Show("Please select a period.");
                return;
            }

            if (FromPicker.SelectedDate > ToPicker.SelectedDate)
            {
                MessageBox.Show("From date must be before To date.");
                return;
            }

            try
            {
                var stats = await apiClient.GetStatisticsAsync(
                    FromPicker.SelectedDate.Value,
                    ToPicker.SelectedDate.Value,
                    room.Id);

                if (stats is null)
                {
                    MessageBox.Show("No statistics found.");
                    return;
                }

                TotalReservationsLabel.Content = stats.TotalReservations;
                OccupancyHoursLabel.Content = $"{stats.OccupancyHours:F1} h";
                OccupancyPercentageLabel.Content = $"{stats.OccupancyPercentage:F1} %";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
            => Close();
    }
}
