using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Managment_System.Models
{
    #region Patient Registration Models
    //person table in database
    public class Person
    {
        public string Id { get; set; }
        public string first_name { get; set; }
        public string middile_name { get; set; }
        public string last_name { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public DateTime date_registered { get; set; }
        public int type_Id { get; set; }
        public int category { get; set; }
        public int tax { get; set; }
        public bool active { get; set; }
        public string remark { get; set; }

    }
    //patient table in database
    public class Patient
    {
        public int id { get; set; }
        public string person_id { get; set; }

    }
    //address table in databse
    public class Address
    {
        public int id { get; set; }
        public int patient_id { get; set; }
        public string city { get; set; }
        public string subcity { get; set; }
        public string kebele { get; set; }
        public string house_no { get; set; }

    }
    //

    public class PatientDto
    {

        public string PersonID { get; set; }
        public int PatientID { get; set; }
        public int LocationID { get; set; }
        public string VisitLocation { get; set; }
        public DateTime VisitStartDate { get; set; }
        public DateTime? VisitEndDate { get; set; }
        public string VisitStatus { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateRegistered { get; set; }
        public int PatientAddressID { get; set; }
        public string City { get; set; }
        public string SubCity { get; set; }
        public string Kebele { get; set; }
        public string HouseNo { get; set; }
        public int VisitStatusID { get; set; }
        public string VochourCode { get; set; }
        public DateTime LastArrivalDate { get; set; }
        public DateTime LastInvoiceDate { get; set; }
        public string InvoiceType { get; set; }
        public string ItemID { get; set; }
        public int AssignmentType { get; set; }
        public string AssignedValue { get; set; }
        public string DeviceName { get; set; }
        public string color { get; set; }
        public bool Active { get; set; }
    }
    public class PatientDocument
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string VisitLocation { get; set; }
        public string VisitStatus { get; set; }
        public DateTime VisitStartDate { get; set; }
        public DateTime? VisitEndDate { get; set; }
        public string LastArrivalDate { get; set; }
        public string LastInvoiceDate { get; set; }
        public DateTime DateRegistered { get; set; }
        public string Address { get; set; }    
        public string DeviceName { get; set; }

    }
    public class PatientHistory
    {
        public string Id { get; set; }            
        public string FullName { get; set; }
        public string DateRegistered { get; set; }
        public string VisitLocation { get; set; }
        public string VisitStatus { get; set; }
        public string LastArrivalDate { get; set; }
        public string LastInvoiceDate { get; set; }


    }
    public class RegisterCodeHostory
    {
        public string Code { get; set; }
        public string GrandTotal { get; set; }
        public DateTime Date { get; set; }
        public string CardType { get; set; }
    }

    public class Regestrationfee
    {
        public string Type { get; set; }
        public string value { get; set; }
    }


    public class ComoBoxList
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
    public class Doctor
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class RegistrationItem
    {
        public int id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public bool is_batch { get; set; }
        public int uom { get; set; }
        public int Reference { get; set; }
        public int Price { get; set; }
        public string value { get; set; }
        public string item_Id { get; set; }


    }
    public class Defination
    {
        public int id { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public bool is_active { get; set; }
        public string remark { get; set; }



    }
    public class Voucher
    {
        public int id { get; set; }
        public string name { get; set; }
        public string parent { get; set; }
        public string abbrivation { get; set; }
        public bool is_active { get; set; }
        public string remark { get; set; }



    }
    public class MenuDefinition
    {
        public int id { get; set; }
        public string name { get; set; }
        public string parent { get; set; }
        public string abbreviation { get; set; }
        public bool is_active { get; set; }
        public string remark { get; set; }
    }
    public class Room
    {
        public int id { get; set; }
        public int type_id { get; set; }
        public string description { get; set; }
        public string floor { get; set; }
        public string remark { get; set; }
    }
    public class Configurations
    {
        public int id { get; set; }
        public string description { get; set; }
        public int type { get; set; }
        public string value { get; set; }
        public string remark { get; set; }

    }
    public class Operation
    {
        public int id { get; set; }
        public int operation { get; set; }
        public int type { get; set; }
        public string color { get; set; }
        public bool is_final { get; set; }
        public bool manual { get; set; }
        public string remark { get; set; }
    }

    public class Invoice
    {
        public string code { get; set; }
        public int type { get; set; }
        public DateTime date { get; set; }
        public int consignee { get; set; }
        public int period { get; set; }
        public bool is_final { get; set; }
        public bool is_void { get; set; }
        public decimal subtotal { get; set; }
        public decimal discount { get; set; }
        public decimal tax { get; set; }
        public decimal grand_total { get; set; }
        public int last_operation { get; set; }

    }
    public class Period
    {
        public int id { get; set; }
        public string description { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string remark { get; set; }
    }
    public class InvoiceOperation
    {
        public int code { get; set; }
        public int operation_id { get; set; }
        public string invoice_id { get; set; }
        public DateTime operation_datetime { get; set; }
        public string device { get; set; }
        public int UserName { get; set; }
    }
    public class InvoiceLine
    {
        public int id { get; set; }
        public string invoice { get; set; }
        public string itemId { get; set; }
        public decimal qty { get; set; }
        public decimal unit_amount { get; set; }
        public decimal total { get; set; }
        public decimal taxable_amount { get; set; }
        public decimal tax_amount { get; set; }

    }
    public class Visit
    {
        public int id { get; set; }
        public int location_id { get; set; }
        public int patient_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime? end_date { get; set; }
        public int status_id { get; set; }
    }
    public class PatientAssignment
    {
        public int id { get; set; }
        public int patient_id { get; set; }
        public int assignment_type { get; set; }
        public string assigned_to { get; set; }
        public string Invoice { get; set; }

    }
    public class UpdateVisit
    {
        public int patient_id { get; set; }
        public int location_id { get; set; }
        public DateTime end_date { get; set; }
        public int status_id { get; set; }
    }
    public class UpdateInvoice
    {
        public int consignee { get; set; }
        public string code { get; set; }

        public int last_operation { get; set; }
    }
    public class Organization
    {
        public string id { get; set; }
        public string brandName{get; set;}
    }
    public class NextofKin
    {
        public  int id { get; set; }
        public int patient_id { get; set; }
        public string kin_name { get; set; }
        public string kin_phone { get; set; }
        public string remark { get; set; }
    }
    public class OrganizationalCustomer
    {
        public int id { get; set; }
        public int patient_id { get; set; }
        public string organization_id { get; set; }
    }
    public class DepositHistory
    {
        public string Code { get; set; }
        public string GrandTotal { get; set; }
        public string Date { get; set; }
        public int InvoiceType { get; set; }
        public int PatientID { get; set; }
    }
    public class DepositHistoryDocument
    {
        public string DepositID { get; set; }  
        public string DepositAmount { get; set; }  
        public string DepositedDate { get; set; }
    }
    public class RegisterationInvoiceHistory
    {
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public decimal GrandTotal { get; set; }
        public string CardType { get; set; }
        public int PatientID { get; set; }
        public int InvoiceType { get; set; }
    }
    public class PatientKinData
    {
        public string kin_name { get; set; }
        public string kin_phone { get; set; }              
        public int patient_id { get; set; }
    }
    public class PatientInfoDisplay
    {
        public string  Address { get; set; }
        public string Orgaanization { get; set; }
        public string KinName { get; set; }
        public string KinPhone { get; set; }
    }
    public class PatientRelation
    {
        public string brandName { get; set; }
        public int patient_id { get; set; }
       
    }


    public class SaveImageUrl
    {
        public int patient_id { get; set; }
        public string image_url { get; set;}
    }

    #endregion


}
