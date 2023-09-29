using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Managment_System.Models
{
    public class Appointment
    {
        public string PateintId { get; set; }
        public int LocationId { get; set; }
        public string Provider { get; set; }
        public int ServiceId { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; }
    }
    public class AppointmentSummary
    {
        public string PatientId { get; set; }
        public string VisitLocation { get; set; }
        public string ServiceType { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Provider { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
    }
}
