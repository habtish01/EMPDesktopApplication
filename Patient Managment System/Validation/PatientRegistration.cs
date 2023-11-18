using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Patient_Managment_System.Validation
{
    public class patientRegistration
    {
      
        public  bool isFirstNameValid(string firstName)
        {
           
            if (String.IsNullOrEmpty(firstName))  
            {
                return false;
            }
            return true;
        }
        public bool isMiddleNameValid(string middleName)
        {
            if (String.IsNullOrEmpty(middleName))
            {
                return false;
            }
            return true;
        }
        public bool isLastNameValid(string lastName)
        {
            if ( String.IsNullOrEmpty(lastName))
            {
                return false;
            }
            return true;
        }

        public  bool isPhoneNumberValid(string phoneNumber)
        {
            string phonePattern = @"^\+\d{1,3} \(\d{3}\) \d{3}-\d{4}$";
            if (string.IsNullOrEmpty(phoneNumber) && !(Regex.IsMatch(phoneNumber, phonePattern)))
            {
               
                return false;
            }
            return true;
        }

        public  bool isAgeValid(string age)
        {
            if(String.IsNullOrEmpty(age))
            {
                
                return false;
            }
            if (!(int.TryParse(age, out int ag)))
            {
                return false;
            }
            if (ag <= 0) return false;
            return true;
        }
        public  bool isIdValid(string id)
        {
            if (String.IsNullOrEmpty(id)) { return false; }
            return true;
        }
        public  bool isGenderValid(string gender)
        {
            if (String.IsNullOrEmpty(gender)) { return false; }
            return true;
        }

        public  bool isRegistrationFeeTypeValid(string type)
        {
            if (String.IsNullOrEmpty(type)) { return false; }
            return true;
        }
        public bool isRegistrationFeeAmountValid(string amount)
        {
            if ( String.IsNullOrEmpty(amount)) { return false; }
            return true;
        }

        public  bool isFinanceTypeValid(string type)
        {
            if (String.IsNullOrEmpty(type)) { return false; }
            return true;
        }

        public bool isFinanceAmountValid(string amount)
        {
            if (String.IsNullOrEmpty(amount)) { return false; }
            return true;
        }

        public  bool isDoctorAssignmentTypeValid(string type)
        {
            if (String.IsNullOrEmpty(type)) { return false; }
            return true;
        }

        public bool isDoctorAssignmentValueValid(string value)
        {
            if ( String.IsNullOrEmpty(value)) { return false; }
            return true;
        }

      
    }
}
