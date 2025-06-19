using System;
using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interface;
using Repository.Repository;
using Services.Interface;
using Services.Services;

namespace BloodBankSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Register repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IBloodInventoryService, BloodInventoryService>();
            services.AddScoped<IBloodGroupService, BloodGroupService>();

            // Register windows
            services.AddTransient<MainWindow>();
            services.AddTransient<HomePage>();
            services.AddTransient<RegisterPage>();
            services.AddTransient<AdminDisplay.AdminDisplay>();
            services.AddTransient<AdminDisplay.BloodEvent>();
            services.AddTransient<AdminDisplay.BloodInformations>();
            services.AddTransient<AdminDisplay.UserInformation>();
            services.AddTransient<AdminDisplay.UserRequest>();
            services.AddTransient<UserDisplay.UserDisplay>();
            services.AddTransient<UserDisplay.UserInfo>();
            services.AddTransient<UserDisplay.DonorDisplay>();
            services.AddTransient<UserDisplay.RecipientDisplay>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Start with HomePage
                var homePage = serviceProvider.GetRequiredService<HomePage>();
                homePage.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting application: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}
