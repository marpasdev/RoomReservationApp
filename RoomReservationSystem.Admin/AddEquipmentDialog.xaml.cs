using RoomReservationSystem.Shared.DTOs.Equipment;
using System.Windows;

namespace RoomReservationSystem.Admin
{
    /// <summary>
    /// Interaction logic for AddEquipmentDialog.xaml
    /// </summary>
    public partial class AddEquipmentDialog : Window
    {
        public CreateEquipmentRequest? Result { get; private set; }

        public AddEquipmentDialog()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Name is required.");
                return;
            }

            Result = new CreateEquipmentRequest() { Name = NameBox.Text };
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
