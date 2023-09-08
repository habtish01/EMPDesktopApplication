using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using System.Configuration;
using Patient_Managment_System.Models;
using System.Net;
using static DevExpress.Utils.Svg.CommonSvgImages;

namespace Patient_Managment_System.Data_Access_Layer
{
    public class DataAccessLayer
    {
        string connectionString = ConfigurationManager.ConnectionStrings["HahcConnection"].ConnectionString;
        List<Patient> patients = new List<Patient>();

        public bool InsertPerson(Person person)
        {
            
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();          
            string PersonRegistration = "INSERT INTO[general].[person] ([Id],[first_name],[middile_name],[last_name],[gender],[age],[date_registered],[type_Id],[phone],[active]) values (@id,@firstname,@middlename,@lastname,@gender,@age,@dateregistered,@typeid,@phone,@active)";
            string PatientRegistration = "INSERT INTO[general].[patient] ([person_id]) values (@id)";
            SqlCommand command = new SqlCommand(PersonRegistration, connection);
            SqlCommand command1 = new SqlCommand(PatientRegistration, connection);

            // Add parameters 
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
            //add parameters for patient table
            command1.Parameters.AddWithValue("@id", person.Id);
            int rowsAffected = command.ExecuteNonQuery();
            int rowsAffected1 = command1.ExecuteNonQuery();
            connection.Close();
            if(rowsAffected > 0 && rowsAffected1>0)
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
            SqlCommand sqlCommand = new SqlCommand(InsertAddress, connection);
            string filterId = "select id from general.patient where person_id=@id";
            SqlCommand sqlCommand1=new SqlCommand(filterId, connection);
            sqlCommand1.Parameters.AddWithValue("@id", address.PatientId);
            SqlDataReader reader = sqlCommand1.ExecuteReader();
            if(reader.HasRows) 
                {
                    reader.Read();
                   var PatientId = reader["id"].ToString();
                   reader.Close();
                        //Add parametres
                        sqlCommand.Parameters.AddWithValue("@id", Convert.ToInt32(PatientId));
                        sqlCommand.Parameters.AddWithValue("@city", address.City);
                        sqlCommand.Parameters.AddWithValue("@subcity", address.SubCity);
                        sqlCommand.Parameters.AddWithValue("@kebele", address.Kebele);
                        sqlCommand.Parameters.AddWithValue("@houseno", address.HouseNo);

                        int rowsAffected = sqlCommand.ExecuteNonQuery();
                        if (rowsAffected > 0) return true;
                        return false;
                }
                    return false;
        }

        public bool InsertVisitType(ComoBoxList type,string id)
        {
            var selectedId = type.Id;
            var selectedVisitType = type.Description;
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
                            Description = reader.GetString(1),
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


        //public List<ComoBoxList> LoadListForAssignmentValueCoboBox(int id)
        //{

        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    List<ComoBoxList> dataList = new List<ComoBoxList>();

        //    string query = "SELECT id,description FROM general.finance_type";
        //    using (SqlCommand command = new SqlCommand(query, connection))
        //    {
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                ComoBoxList visit = new ComoBoxList
        //                {
        //                    Id = reader.GetInt32(0),
        //                    Description = reader.GetString(1),
        //                };
        //                dataList.Add(visit);
        //            }
        //        }
        //    }
        //    connection.Close();

        //    return dataList;
        //}

        //public bool UpdateStatus(string id)
        //{
        //    SqlConnection connection= new SqlConnection(connectionString);
        //    connection.Open();
        //    string updateQuery = "UPDATE general.visit SET status_id =@status WHERE patient_id = @id";
        //    SqlCommand command = new SqlCommand(updateQuery, connection);
        //    command.Parameters.AddWithValue("@status", 2);
        //    command.Parameters.AddWithValue("@id", Convert.ToInt32(id));
        //    int rowsAffected=command.ExecuteNonQuery();
        //    connection.Close();

        //    if (rowsAffected>0)return true; else return false;
        //}

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

            string query = "SELECT first_name,middile_name,last_name,gender,phone FROM general.person where type_id=@id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                
                command.Parameters.AddWithValue("@id", 1);
                //SqlDataReader reader = command.ExecuteReader();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Patient patient = new Patient
                        {
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
    }
}
