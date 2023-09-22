using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Managment_System.Models
{
    public class IdGeneration
    {
        public string prefix { get; set; }
        public string prefix_separator { get; set; }
        public string suffix { get; set; }
        public string suffix_separator { get; set; }
        public int Length { get; set; }
    }
}
