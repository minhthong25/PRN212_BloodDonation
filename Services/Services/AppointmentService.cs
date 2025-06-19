using System;
using System.Collections.Generic;
using System.Linq;
using Repository.Interface;
using Repository.Models;
using Repository.Repository;
using Services.Interface;

namespace Services.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IGenericRepository<Appointment> _appointmentRepository;

        public AppointmentService()
        {
            _appointmentRepository = new GenericRepository<Appointment>();
        }

        public List<Appointment> GetAllAppointments()
        {
                return _appointmentRepository.GetAll().ToList();
        }

        public void AddAppointment(Appointment appointment)
        {
            try
            {
                if (appointment == null)
                    throw new ArgumentNullException(nameof(appointment));

                // Validate appointment date is in the future
                if (appointment.AppointmentDate <= DateTime.Now)
                    throw new ArgumentException("Appointment date must be in the future");

                _appointmentRepository.Add(appointment);
                _appointmentRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding appointment", ex);
            }
        }

        public void UpdateAppointment(Appointment appointment)
        {
            try
            {
                if (appointment == null)
                    throw new ArgumentNullException(nameof(appointment));

                var existingAppointment = _appointmentRepository.FirstOrDefault(a => a.AppointmentId == appointment.AppointmentId);
                if (existingAppointment == null)
                {
                    throw new Exception("Appointment not found");
                }

                // Validate appointment date is in the future
                if (appointment.AppointmentDate <= DateTime.Now)
                    throw new ArgumentException("Appointment date must be in the future");

                // Update only allowed fields
                existingAppointment.AppointmentDate = appointment.AppointmentDate;
                existingAppointment.IsCompleted = appointment.IsCompleted;
                existingAppointment.DonorId = appointment.DonorId;
                existingAppointment.LocationId = appointment.LocationId;

                _appointmentRepository.Update(existingAppointment);
                _appointmentRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating appointment", ex);
            }
        }

        public void DeleteAppointment(int id)
        {
            try
            {
                var appointment = _appointmentRepository.FirstOrDefault(a => a.AppointmentId == id);
                if (appointment != null)
                {
                    _appointmentRepository.Delete(appointment);
                    _appointmentRepository.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting appointment with ID {id}", ex);
            }
        }

        public Appointment? GetAppointmentByDate(DateTime date)
        {
            try
            {
                return _appointmentRepository.FirstOrDefault(a => a.AppointmentDate.Date == date.Date);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving appointment for date {date}", ex);
            }
        }

        public List<Appointment> GetAppointmentsByDonorId(int donorId)
        {
            try
            {
                return _appointmentRepository.GetAll().Where(a => a.DonorId == donorId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving appointments for donor with ID {donorId}", ex);
            }
        }
    }
}
