using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Configuration;
using Patient_Managment_System.Models;
using System.Net;
using static DevExpress.Utils.Svg.CommonSvgImages;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using DevExpress.XtraScheduler;
using System.Windows.Controls.Primitives;

namespace Patient_Managment_System.Data_Access_Layer
{
    public class DataAccessLayer
    {
        string connectionString = ConfigurationManager.ConnectionStrings["HahcConnection"].ConnectionString;
 


        #region DataBase Access


        #region Patient Info
        public bool checkPersonExistance(string id)
        {

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = "select Id from general.person where Id=@id";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.Add(new SqlParameter("@id", id));
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.HasRows)
            {
                return true;
            }
            return false;
        }
        public bool UpdatePatient (Person person)
        {
            string updatePerson = "UPDATE [general].[person] SET [first_name] = @first_name,[middile_name] = @middile_name,[last_name] = @last_name,[gender] = @gender,[age] = @age ,[phone] = @phone WHERE Id=@id";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(updatePerson, connection);

            // Add parameters 
            command.Parameters.AddWithValue("@id", person.Id);
            command.Parameters.AddWithValue("@first_name", person.FirstName);
            command.Parameters.AddWithValue("@middile_name", person.MiddleName);
            command.Parameters.AddWithValue("@last_name", person.LastName);
            command.Parameters.AddWithValue("@gender", person.Gender);
            command.Parameters.AddWithValue("@age", person.Age);
            command.Parameters.AddWithValue("@phone", person.PhoneNumber);
            int rowsAffected = command.ExecuteNonQuery();

            connection.Close(); 
            if (rowsAffected > 0 )
            {
                return true;
            }
            return false;

        }
        public int getPatientID(string personId)
        {
            int PatientId = 0;  
            SqlConnection connection = new SqlConnection(connectionString);
            string filterId = "select id from general.patient where person_id=@id";
            connection.Open();
            SqlCommand sqlCommand1 = new SqlCommand(filterId, connection);
            sqlCommand1.Parameters.AddWithValue("@id", personId);
            SqlDataReader reader = sqlCommand1.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                 PatientId = int.Parse(reader["id"].ToString());
                reader.Close();
            }
            return PatientId;
      }

        public bool isAddressExist(int patientId)
        {
            string isAddressExit = "select patient_id from general.patientaddress where patient_id=@id";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(isAddressExit, connection);
            cmd.Parameters.AddWithValue("@id", patientId);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                return true;
            }
            return false;
        }


        public bool UpdateAddress(Address address)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string addressQuery = "UPDATE [general].[patientaddress] SET [subcity] = @subcity ,[city] = @city ,[kebele] = @kebele ,[house_no] = @house_no WHERE patient_id=@id";
            connection.Open();
            SqlCommand sqlCommand =new SqlCommand(addressQuery,connection);
           
                //Add parametres
                    sqlCommand.Parameters.AddWithValue("@id", address.PatientId);
                    sqlCommand.Parameters.AddWithValue("@city", address.City);
                    sqlCommand.Parameters.AddWithValue("@subcity", address.SubCity);
                    sqlCommand.Parameters.AddWithValue("@kebele", address.Kebele);
                    sqlCommand.Parameters.AddWithValue("@house_no", address.HouseNo);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    connection.Close(); 
                    if (rowsAffected > 0) return true;
                   
                   
                
            return false;
        }

        public bool InsertPerson(Person person)
        {
           
          
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command1;
            SqlCommand command;
            int rowsAffected1=0;
            int rowsAffected = 0;
            string PersonRegistration = "INSERT INTO[general].[person] ([Id],[first_name],[middile_name],[last_name],[gender],[age],[date_registered],[type_Id],[phone],[active]) values (@id,@firstname,@middlename,@lastname,@gender,@age,@dateregistered,@typeid,@phone,@active)";
            string PatientRegistration = "INSERT INTO[general].[patient] ([person_id]) values (@id)";
            
                //insert person registration
                command = new SqlCommand(PersonRegistration, connection);
                command.Parameters.AddWithValue("@id", person.Id);
                command.Parameters.AddWithValue("@firstname", person.FirstName);
                command.Parameters.AddWithValue("@middlename", person.MiddleName);
                command.Parameters.AddWithValue("@lastname", person.LastName);
                command.Parameters.AddWithValue("@gender", person.Gender);
                command.Parameters.AddWithValue("@age", person.Age);
                command.Parameters.AddWithValue("@dateregistered", person.DateRegistered);
                command.Parameters.AddWithValue("@typeid", 1);
                command.Parameters.AddWithValue("@phone", person.PhoneNumber);
                command.Parameters.AddWithValue("@active", true);
                rowsAffected = command.ExecuteNonQuery();

                //for patient Registration
                command1 = new SqlCommand(PatientRegistration, connection);
                command1.Parameters.AddWithValue("@id", person.Id);
                rowsAffected1 = command1.ExecuteNonQuery();

                if (rowsAffected > 0 && rowsAffected1 > 0)
                {
                    return true;
                }

            
          
            return false;
        }

        public bool InsertAddress(Address address)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string InsertAddress = "insert into[general].[patientaddress] ([patient_id],[city],[subcity],[kebele],[house_no]) values (@id,@city,@subcity,@kebele,@houseno)";
            connection.Open();           
           
                //insert
                SqlCommand sqlCommand = new SqlCommand(InsertAddress, connection);
               
                    //Add parametres
                    sqlCommand.Parameters.AddWithValue("@id", address.PatientId);
                    sqlCommand.Parameters.AddWithValue("@city", address.City);
                    sqlCommand.Parameters.AddWithValue("@subcity", address.SubCity);
                    sqlCommand.Parameters.AddWithValue("@kebele", address.Kebele);
                    sqlCommand.Parameters.AddWithValue("@houseno", address.HouseNo);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    if (rowsAffected > 0) return true;
                   
                
            return false;
        }

        #endregion
       public int getVisitLocation(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "select location_id from general.visit where patient_id=@id and status_id=@status_id";
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            connection.Open();
            sqlCommand.Parameters.AddWithValue("@id", id);
            sqlCommand.Parameters.AddWithValue("@status_id", 1);

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                var Id = int.Parse(reader["location_id"].ToString());
                reader.Close();
                return Id;  
            }
            return -1;
            }
        public bool checkVisitExistanceForPatient(string personId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string filterId = "select id from general.patient where person_id=@id";
            string query = "select patient_id from general.visit where patient_id=@id and status_id=@status_id";
            SqlCommand sqlCommand = new SqlCommand(filterId, connection);
            connection.Open();
            sqlCommand.Parameters.AddWithValue("@id", personId);
           
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                var Id = int.Parse(reader["id"].ToString());
                reader.Close();
               SqlCommand command=new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", Id);
                command.Parameters.AddWithValue("@status_id", 1);
                SqlDataReader reader1 = command.ExecuteReader();
                while (reader1.HasRows)
                {
                    return true;    
                }
               
            }
           
            return false;
        }
        public bool InsertVisitType(ComoBoxList type,string id)
        {
            var selectedId = type.Id;
            //var selectedVisitType = type.Description;
            var start_date=DateTime.Now;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string filterId = "select id from general.patient where person_id=@id";
            SqlCommand sqlCommand = new SqlCommand(filterId, connection);
            sqlCommand.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                var Id = reader["id"].ToString();
                reader.Close();


                string InsertVisit = "insert into general.visit ([patient_id],[location_id],[start_date],[status_id]) values (@id,@location_id,@start_date,@status_id)";
                SqlCommand sqlCommand1 = new SqlCommand(InsertVisit, connection);
                sqlCommand1.Parameters.AddWithValue("@id", Convert.ToInt32(Id));
                sqlCommand1.Parameters.AddWithValue("@location_id", selectedId);
                sqlCommand1.Parameters.AddWithValue("@start_date", start_date);
                sqlCommand1.Parameters.AddWithValue("@status_id", 1);
                int rowsAffected = sqlCommand1.ExecuteNonQuery();
                connection.Close();
                if (rowsAffected > 0) return true; return false;
            }
            return false;

        }
        public bool updateVisitType(ComoBoxList type, string personId)
        {
            var selectedId = type.Id;
         
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string filterId = "select id from general.patient where person_id=@id";
            SqlCommand sqlCommand = new SqlCommand(filterId, connection);
            sqlCommand.Parameters.AddWithValue("@id", personId);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                var patientId = reader["id"].ToString();
                reader.Close();


                string InsertVisit = "update  general.visit set [location_id]=@location_id where patient_id=@id";
                SqlCommand sqlCommand1 = new SqlCommand(InsertVisit, connection);
                sqlCommand1.Parameters.AddWithValue("@id", Convert.ToInt32(patientId));
                sqlCommand1.Parameters.AddWithValue("@location_id", selectedId);
                       
                int rowsAffected = sqlCommand1.ExecuteNonQuery();
                connection.Close();
                if (rowsAffected > 0) return true; return false;
            }
            return false;

        }
        public List<ComoBoxList> LoadListForVisitTypeCoboBox()
        {
            
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            List<ComoBoxList> dataList = new List<ComoBoxList>();

            string query = "SELECT id,description FROM general.location";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ComoBoxList visit = new ComoBoxList
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1)

                        };
                        dataList.Add(visit);
                    }
                }
            }
            connection.Close();

            return dataList;
        }

        public List<ComoBoxList> LoadListForFinanceTypeCoboBox()
        {

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            List<ComoBoxList> dataList = new List<ComoBoxList>();

            string query = "SELECT id,description FROM general.value_type";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ComoBoxList visit = new ComoBoxList
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1)
                        };
                        dataList.Add(visit);
                    }
                }
            }
            connection.Close();

            return dataList;
        }

        public List<ComoBoxList> LoadListForAssignmentTypeCoboBox()
        {

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            List<ComoBoxList> dataList = new List<ComoBoxList>();

            string query = "SELECT Id,Type FROM finance_type";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ComoBoxList visit = new ComoBoxList
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                        };
                        dataList.Add(visit);
                    }
                }
            }
            connection.Close();

            return dataList;
        }


        
        public bool UpdateStatus(string id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string filterId = "select id from general.patient where person_id=@id";
            SqlCommand sqlCommand = new SqlCommand(filterId, connection);
            sqlCommand.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                var PatientId = reader["id"].ToString();
                reader.Close();
                string updateQuery = "UPDATE general.visit SET status_id =@status WHERE patient_id = @id";
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@status", 2);
                command.Parameters.AddWithValue("@id", Convert.ToInt32(PatientId));
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0) return true; else return false;

            }
            return false;
        }

        public List<Patient> GetPatients() 
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            List<Patient> PatientdataList = new List<Patient>();

            string query = "SELECT id,first_name,middile_name,last_name,gender,phone FROM general.person where type_id=@id and active=@active ORDER BY id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                
                command.Parameters.AddWithValue("@id", 1);
                command.Parameters.AddWithValue("@active", true);
                //SqlDataReader reader = command.ExecuteReader();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Patient patient = new Patient
                        {
                            Id= reader["id"].ToString(),    
                            FirstName = reader["first_name"].ToString(),
                            MiddleName =reader["middile_name"].ToString(),
                            LastName = reader["last_name"].ToString(),
                            Gender = reader["gender"].ToString(),
                            PhoneNumber = reader["phone"].ToString(),

                        };
                        PatientdataList.Add(patient);
                    }
                }
            }
            connection.Close();

            return PatientdataList;
        }
        public int getPatientId(string personId)
        {
            int patientId=-1;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = "select id from general.patient where person_id=@id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", personId);
            SqlDataReader reader = command.ExecuteReader(); 
            while (reader.Read())
            {
                patientId = reader.GetInt32(0);    
            }
            return patientId;

        }
        public Address getPatientAdrressInfo(int patientID)
        {
          
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = "select city,subcity,kebele,house_no from general.patientaddress where patient_id=@id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", patientID);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Address address = new Address()
                {
                    City = reader["city"].ToString(),
                    SubCity = reader["subcity"].ToString(),
                    Kebele = reader["kebele"].ToString(),
                    HouseNo = reader["house_no"].ToString()
                };
                return address;
                
            }
            return null;

        }
        public Person getPatientInfo(string personID)
        {

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = "select age,date_registered,active from general.person where Id=@id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", personID);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Person person= new Person()
                {
                    Age = int.Parse(reader["age"].ToString()),
                    DateRegistered = DateTime.Parse(reader["date_registered"].ToString()),
                    Active = bool.Parse(reader["active"].ToString())
                };
                return person;

            }
            return null;

        }

        public bool DeletePatient(string patientID)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string deleteQuery = "update general.person set active=@active where Id=@id";
            SqlCommand command=new SqlCommand(deleteQuery, connection);
            command.Parameters.AddWithValue("@id", patientID);
            command.Parameters.AddWithValue("@active", false); 
            if (command.ExecuteNonQuery() > 0)
            {
                return true;    
            }
            return false;   
        }


        // for the appointments
        public List<Appointmentt> loadAppointmentSummary()
        {
           
            List<Appointmentt> appointments = new List<Appointmentt>();
            string query = "SELECT *from dbo.AppointmentList";                    

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader(); 
            while (reader.Read())
            {

                Appointmentt appointment = new Appointmentt
                {
                    FirstName = reader["first_name"].ToString(),
                    MiddleName=reader["middile_name"].ToString(),                                   
                    LastName = reader["last_name"].ToString(),
                    ServiceType = reader["service_description"].ToString(),
                    VistLocation =reader["description"].ToString(),
                    AppointmentNote = reader["note"].ToString(),
                    OrderdBy = reader["appointed_by"].ToString(),
                    OrderedDate = DateTime.Parse(reader["date"].ToString()),
                    Status = bool.Parse(reader["status"].ToString()),
                    Remark = reader["remark"].ToString(),
                };

                appointments.Add(appointment);
            }

            return appointments;

        }
       
    }
}
#endregion 
