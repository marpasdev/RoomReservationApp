using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Windows;

namespace RoomReservationSystem.Admin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ApiClient ApiClient { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            ApiClient = new ApiClient(config["ApiBaseUrl"]!);
            ApiClient.SetToken(config["AdminToken"]!);

        }

    }

}
