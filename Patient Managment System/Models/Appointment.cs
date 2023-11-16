using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Managment_System.Models
{
    #region Appointment Model
    public class Appointmentt
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ServiceType { get; set; }
        public string VistLocation { get; set; }
        public string AppointmentNote { get; set; }
        public string OrderdBy { get; set; }
        public DateTime OrderedDate { get; set; }      
        public bool Status { get; set; }
        public string Remark { get; set; }
    }
    #endregion


}
