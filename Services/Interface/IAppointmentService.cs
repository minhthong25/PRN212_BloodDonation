using Repository.Models;

namespace Services.Interface
{
    public interface IAppointmentService
    {
        List<Appointment> GetAllAppointments();
        void AddAppointment(Appointment appointment);
        void UpdateAppointment(Appointment appointment);
        void DeleteAppointment(int id);
        List<Appointment> GetAppointmentsByDonorId(int donorId);
    }
} 