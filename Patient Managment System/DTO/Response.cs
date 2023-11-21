using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Managment_System.DTO
{
    public class Response
    {
        public bool IsPassed { get; set; }
        public string SuccessMessage { get; set; } 
        public string ErrorMessage { get; set; }=string.Empty;
    }
}
