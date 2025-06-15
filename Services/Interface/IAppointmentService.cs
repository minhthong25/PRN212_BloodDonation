using Repository.Models;

namespace Services.Interface
{
    public interface IAppointmentService
    {
        Appointment? GetAppointmentById(int id);
        void AddAppointment(Appointment appointment);
        void UpdateAppointment(Appointment appointment);
        void DeleteAppointment(int id);
    }
} 