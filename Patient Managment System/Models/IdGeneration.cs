using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Managment_System.Models
{
    public class IdDefinitionDetail
    {
        public int id { get; set; }
        public string prefix { get; set; }
        public string prefix_separator { get; set; }
        public int length { get; set; }
        public string suffix { get; set; }
        public string suffix_separator { get; set; }
      
        public int type { get; set; }
    }
    public class ID
    {
        public string current_value { get; set; }
        public int defination { get; set; }
    }
}
