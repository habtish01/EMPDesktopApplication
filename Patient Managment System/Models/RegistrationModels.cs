using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Managment_System.Models
{
    public class Person
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateRegistered { get; set; }
        public bool Active { get; set; }

    }
    public class Patient
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class Address
    {
        public int PatientId { get; set; }
        public string City { get; set; }
        public string SubCity { get; set; }
        public string Kebele { get; set; }
        public string HouseNo { get; set; }

    }

    public class Regestrationfee
    {
        public string Type { get; set; }
        public string value { get; set; }
    }

    public class Finance
    {
        public string Type { get; set; }
        public string value { get; set; }
    }

    public class DoctorAssign
    {
        public string Type { get; set; }
        public string value { get; set; }
    }

    public class ComoBoxList
    {
        public int Id { get; set; }  
        public string Description { get; set; }
    }
    public class patientInfo
    {
        public string PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int Phone { get; set; }
        public string Gender { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string City { get; set; }
        public string subCity { get; set; }
        public string Kebele { get; set; }
        public string HouseNo { get; set; }
        public int RegistrationType { get; set; }
        public int RegistrationFee  { get; set; }
        public int FinanceType { get; set; }
        public int FinanceAmount { get; set; }
        public int AssignmentType { get; set; }
        public int AssignmentValue { get; set; }
    }

}
