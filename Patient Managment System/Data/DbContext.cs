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
using Patient_Managment_System.DTO;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Patient_Managment_System.Data_Access_Layer
{
    public class DbContext
    {
        string connectionString = ConfigurationManager.ConnectionStrings["HahcConnection"].ConnectionString;
 


        #region DataBase Access


        #region Patient Info
        //check person exist or not in Person Table
        public bool checkPersonExistance(string personId)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
           
                string sql = "select Id from general.person where Id=@id";
               var check = connection.QueryFirstOrDefault<string>(sql, new {id=personId});
                if (check != null)
                {
                    return true;
                }
              return false;
            
            }
            catch  
            {
                return false;   
            }
        }
        public Response updatePerson (Person person)
        {
            Response response = new Response(); 
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string sql = @"UPDATE [general].[person] SET [first_name] = @first_name,
                                                             [middile_name] = @middile_name,
                                                             [last_name] = @last_name,
                                                             [gender] = @gender,
                                                             [age] = @age ,          
                                                             [phone] = @phone 
                                                             WHERE Id=@Id";
                int rows = connection.Execute(sql,person);
                if(rows > 0)
                {
                    response.IsPassed= true;
                    response.SuccessMessage = "Patient Updated Successfully";
                    return response;    
                }
                else
                {
                    response.IsPassed=false;
                    response.ErrorMessage = "Patient Updation Failed";
                    return response;
                }
            }
            catch(Exception ex) 
            {
                response.IsPassed = false;  
                response.ErrorMessage ="For Patient Update Execution"+ ex.Message;
                return response; 
            } 
        }
        public int getPatientID(string personId)
        {
            int PatientId = 0;
            try
            {
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
            catch
            {
                  return PatientId;    
            }
      }

        public bool isAddressExist(int patientId)
        {
            try
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
            catch { return false; }
        }


        public Response updateAddress(Address address)
        {
            Response response = new Response();
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string sql = @"UPDATE [general].[patientaddress] SET [subcity] = @subcity,
                                                                              [city] = @city ,
                                                                              [kebele] = @kebele ,
                                                                              [house_no] = @house_no 
                                                                              WHERE patient_id=@patient_id";
                int rows = connection.Execute(sql, address);
                if(rows > 0)
                {
                    response.IsPassed= true;
                    response.SuccessMessage = "Address Update Successfully";
                    return response;    
                }
                else
                {
                    response.IsPassed= false;
                    response.ErrorMessage = "Address Updation Failed";
                    return response;
                }

            }
            catch(Exception ex)
            {
                response.IsPassed= false;   
                response.ErrorMessage = "For Address Update Execution\n"+ex.Message;
                return response;   
            }
        }

        //insert person query excution using dapper
        public Response insertPerson(Person person)
        {
            Response response = new Response();

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                             string sql = @"INSERT INTO[general].[person] 
                                                                        ([Id],
                                                                        [first_name],
                                                                        [middile_name],
                                                                        [last_name],
                                                                        [gender],
                                                                        [age],
                                                                        [date_registered],
                                                                        [type_Id],
                                                                        [category],
                                                                        [tax],
                                                                        [phone],
                                                                        [active],
                                                                        [remark]) 
                                                                 values 
                                                                        (@Id,
                                                                        @first_name,
                                                                        @middile_name,
                                                                        @last_name,
                                                                        @gender,
                                                                        @age,
                                                                        @date_registered,
                                                                        @type_Id,
                                                                        @category,
                                                                        @tax,
                                                                        @phone,
                                                                        @active,
                                                                        @remark)";
                int row = connection.Execute(sql,person);
                if (row > 0)
                {
                    response.IsPassed = true;
                    response.SuccessMessage = "Person Registered Successfully";
                    return response;    
                }
                else
                {
                    response.IsPassed = false;
                    response.ErrorMessage = "Person Registration Failed!";
                    return response;
                }
                              

            }
            catch (Exception ex) 
            {
                response.ErrorMessage = "For Person Registration "+ex.Message; 
                response.IsPassed = false;
                return response;   
            }
        }
        //insert patient sql excution using dapper
        public Response insertPatient(Patient patient)
        {
            Response response = new Response();

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string sql = @"Insert INTO [general].[patient] 
                                                     ([person_id]) 
                                               values 
                                                     (@person_id) SELECT SCOPE_IDENTITY()";
         
                var patientId=connection.Query<int>(sql, patient).FirstOrDefault();  
                if (patientId > 0)
                {
                    response.IsPassed = true;
                    response.SuccessMessage = "Patient Registered Successfully";
                    response.Data = patientId;
                    return response;    
                }
                else
                {
                    response.IsPassed = false;
                    response.ErrorMessage = "Patient Registration Failed";
                    return response;    
                }

            }
            catch(Exception ex) 
            {
                response.IsPassed = false;
                response.ErrorMessage ="For Patient Registration\n" +ex.Message;
                return response;
            }
         }
        //insert address sql excution using dapper
        public Response insertAddress(Address address)//insert address query
        {
            Response response=new Response();
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string sql = @"insert into[general].[patientaddress] 
                                                                    ([patient_id],
                                                                    [city],
                                                                    [subcity],
                                                                    [kebele],
                                                                    [house_no]) 
                                                              values 
                                                                    (@patient_id,
                                                                    @city,
                                                                    @subcity,
                                                                    @kebele,
                                                                    @house_no)";
                int rows=connection.Execute(sql,address); 
                if (rows > 0)
                {
                    response.IsPassed = true;
                    response.SuccessMessage = "Address Registered Successfully";
                    return response;
                }
                else
                {
                    response.IsPassed= false;
                    response.ErrorMessage = "Address Registeration Failed";
                    return response;    
                }
                            
                            
            }
            catch(Exception ex) 
            {
                response.IsPassed = false;
                response.ErrorMessage = "For Address Excution\n" + ex.Message;
                return response;
            }
        }

        #endregion
        public int getVisitLocation(int id)
        {
            try {
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
            catch { return -1; }
            }
        public int checkVisitExistanceForPatient(int patientId)
        {
            int status = 0;
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string query = "select status_id from general.visit where patient_id=@id";


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", patientId);
                SqlDataReader reader1 = command.ExecuteReader();
                while (reader1.Read())
                {
                    status = int.Parse(reader1["status_id"].ToString().Trim());
                    return status;
                }



                return status;
            }
            catch
            { return status; }
        }
        public Response insertVisitType(Visit visit)//visit query
        {
            Response response = new Response();
            try
            {
              SqlConnection connection = new SqlConnection(connectionString);        
              string sql = @"insert into [general].visit 
                                                     ([patient_id],
                                                     [location_id],
                                                     [start_date],
                                                     [end_date],
                                                     [status_id]) 
                                               values 
                                                     (@patient_id,
                                                     @location_id,
                                                     @start_date,
                                                     @end_date,
                                                     @status_id)";
                var row = connection.Execute(sql, visit);
                if(row>0)
                {
                    response.IsPassed = true;
                    response.SuccessMessage = "Visit Started Successfully";
                    return response;
                }
                else
                {
                    response.IsPassed = false;
                    response.ErrorMessage = "Visit Registeration Failed";
                    return response;
                }


            }
            catch (Exception ex)
            {
                response.IsPassed = false;
                response.ErrorMessage ="For Visit Excution\n"+ ex.Message;
                return response;
            }

        }

        public bool updateVisitType(ComoBoxList type, string personId)
        {
            try
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
            catch { return false; }
        }
        public List<ComoBoxList> LoadListForVisitTypeCoboBox()
        {
            List<ComoBoxList> dataList = new List<ComoBoxList>();

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

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
            catch { return dataList; }
        }

        public Response updateVisitStatus(UpdateVisit updateVisit)
        {
            Response response = new Response(); 
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);               
                string sql = "UPDATE general.visit SET status_id =@status_id WHERE patient_id = @patient_id";
                int rows = connection.Execute(sql, updateVisit);
                if (rows > 0)
                {
                    response.IsPassed = true;
                    response.SuccessMessage = "Visit Closed Successfully";
                    return response;    
                }
                else
                {
                    response.IsPassed = false;
                    response.ErrorMessage = "Vist Closing Failed";
                    return response;
                }


               
            }
            catch(Exception ex) 
            {
                response.IsPassed=false;    
                response.ErrorMessage ="For Closed Visit Excution\n"+ ex.Message;
                return response; ; 
            }
        }
        public Response updateVisit(Visit updateVisit)
        {
            Response response=new Response();   
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);          

                string sql = @"UPDATE general.visit SET location_id=@location_id,
                                                        status_id=@status_id
                                                        WHERE patient_id = @patient_id";

                var row = connection.Execute(sql, updateVisit);
                if(row > 0)
                {
                    response.IsPassed = true;
                    response.SuccessMessage = "Patient Visit Updated Successfully";
                    return response;    
                }
                else
                {
                    response.IsPassed = false;
                    response.ErrorMessage = "Updating visit Failed";
                    return response;
                }
            
            }
            catch(Exception ex)
            { 
                response.IsPassed = false;
                response.ErrorMessage ="For Update Vist Excution \n"+ ex.Message;
                return response;
            }   
        }

        //get all pateint for patient document from database view
        public List<PatientDto> GetPatients() 
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);

                string query = "SELECT *FROM [general].[PatientDocuments]";
                var PatientdataList = connection.Query<PatientDto>(query).ToList();
                return PatientdataList;
            }
            catch { return null; }
        }
     

        // for the appointments
        public List<Appointmentt> loadAppointmentSummary()
        {
            List<Appointmentt> appointments = new List<Appointmentt>();

            try
            {

             
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
                        MiddleName = reader["middile_name"].ToString(),
                        LastName = reader["last_name"].ToString(),
                        ServiceType = reader["service_description"].ToString(),
                        VistLocation = reader["description"].ToString(),
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
            catch { return appointments; }

        }
        ////load Registration Fee Item
        public List<RegistrationItem> getRegistrationfeeItem()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string sql = "select *from [general].RegistrationFeeItem";
                var registrationFeeItem = connection.Query<RegistrationItem>(sql).ToList();
                return registrationFeeItem;
            }
            catch { return null; }  
              
        }
        public List<Defination> getDefinitionDetail()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string sql = "select *from [general].defination";
                var defination = connection.Query<Defination>(sql).ToList();
                return defination;
            }
            catch { return null; }

        }
        public List<Voucher> getVouchers()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string sql = "select *from [general].menu_defination";
                var vouchers = connection.Query<Voucher>(sql).ToList();
                return vouchers;
            }
            catch { return null; }

        }
        public  List<Person> getPersons()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string sql = "select *from [general].person";
                var persons = connection.Query<Person>(sql).ToList();
                return persons;
            }
            catch { return null; }

        }

        public List<Room> getRooms()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string sql = "select *from [room].room";
                var rooms = connection.Query<Room>(sql).ToList();
                return rooms;
            }
            catch { return null; }

        }

        public List<Configurations> getConfigurations()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string sql = "select *from [general].configuration";
                var configurations = connection.Query<Configurations>(sql).ToList();
                return configurations;
            }
            catch { return null; }

        }
        public Response saveOperation(Operation operation)
        {
            Response response = new Response(); 
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = @"INSERT INTO [general].[operation]
                                                               ([operation],
                                                               [type],
                                                               [color],
                                                               [is_final],
                                                               [manual],
                                                               [remark]) 
                                                         VALUES
                                                               (@operation,
                                                               @type,
                                                               @color,
                                                               @is_final,
                                                               @manual,
                                                               @remark) SELECT SCOPE_IDENTITY()";
                    var operationID = conn.Query<int>(sql, operation).FirstOrDefault();
                    if (operationID > 0)
                    {
                        response.IsPassed = true;
                        response.SuccessMessage = "Operation Created Successfully";
                        response.Data = operationID;
                        return response;    
                    }
                    else
                    {
                        response.IsPassed = false;
                        response.ErrorMessage = "Operation Creation Failed";
                        return response;
                    }


                }
            }
            catch(Exception e)
            {
                response.IsPassed = false;
                response.ErrorMessage ="On Operation Excution\n"+ e.Message;
                return response;    

            }

        }
        public Response saveInvoiceOperation(InvoiceOperation invoiceOperation)
        {
            Response response = new Response();
         try { 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO [general].[invoice_operation]
                                         ([operation_id]
                                         ,[invoice_id]
                                         ,[operation_datetime]
                                         ,[device]
                                         ,[UserName])
                                   VALUES
                                         (@operation_id
                                         ,@invoice_id
                                         ,@operation_datetime
                                         ,@device
                                         ,@UserName) SELECT SCOPE_IDENTITY()";
                var invoiceaOperationID = connection.Query<int>(sql, invoiceOperation).FirstOrDefault();
                if (invoiceaOperationID > 0)
                {
                    response.IsPassed = true;
                    response.SuccessMessage = "Invoice Operation Created Successfully";
                    response.Data = invoiceaOperationID;
                    return response;
                }
                else
                {
                    response.IsPassed = false;
                    response.ErrorMessage = "Invoice Operation Creation Failed";
                    return response;
                }

            }
        }
        catch(Exception ex)
            {
                response.IsPassed= false;
                response.ErrorMessage = "For Invoice Operation\n" + ex.Message;
                return response;
            }
      }
        public Response savePeriod(Period period)
        {
            Response response= new Response();  
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string sql = @"INSERT INTO [general].[period]
                                         ([description]
                                         ,[start_date]
                                         ,[end_date]
                                         ,[remark])
                                        
                                   VALUES
                                         (@description
                                         ,@start_date
                                         ,@end_date
                                         ,@remark) SELECT SCOPE_IDENTITY()";
                  var PeriodID = connection.Execute(sql, period);
                    if(PeriodID > 0)
                    {
                        response.IsPassed = true;
                        response.SuccessMessage = "Period Created Successfully";
                        response.Data = PeriodID;
                        return response;
                    }
                    else
                    {
                        response.IsPassed = false;
                        response.ErrorMessage = "Period Creation Failed";
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                response.IsPassed = false;
                response.ErrorMessage="For Period Excution\n"+ex.Message;
                return response;    
            }
        }
        public Response saveInvoice(Invoice invoice)
        {
            Response response= new Response();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                 string sql = @"INSERT INTO [general].[invoice]
                                                              ([code]
                                                              ,[type]
                                                              ,[date]
                                                              ,[consignee]
                                                              ,[period]
                                                              ,[is_final]
                                                              ,[is_void]
                                                              ,[subtotal]
                                                              ,[discount]
                                                              ,[tax]
                                                              ,[grand_total]
                                                              ,[last_operation])
                                                        Values
                                                              (@code
                                                              ,@type
                                                              ,@date
                                                              ,@consignee
                                                              ,@period
                                                              ,@is_final
                                                              ,@is_void
                                                              ,@subtotal
                                                              ,@discount
                                                              ,@tax
                                                              ,@grand_total
                                                              ,@last_operation)";
                    var rows = conn.Execute(sql, invoice);
                    if (rows > 0)
                    {
                        response.IsPassed = true;
                        response.SuccessMessage = "Invoice Created Successfully";
                        return response;
                    }
                    else
                    {
                        response.IsPassed = false;
                        response.ErrorMessage = "Invoice Creation Failled";
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                response.IsPassed = false;
                response.ErrorMessage = "For Invoice Excuution\n" + ex.Message;
                return response;
            }
            

        }
        public Response saveInvoiceLine(InvoiceLine invoiceLine)
        {
            Response response = new Response();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = @"INSERT INTO [general].[invoice_line]
                                                                      ([invoice]
                                                                      ,[itemId]
                                                                      ,[qty]
                                                                      ,[unit_amount]
                                                                      ,[total]
                                                                      ,[taxable_amount]
                                                                      ,[tax_amount])
                                                                VALUES
                                                                      (@invoice
                                                                      ,@itemId
                                                                      ,@qty
                                                                      ,@unit_amount
                                                                      ,@total
                                                                      ,@taxable_amount
                                                                      ,@tax_amount)";
                    var rows = conn.Execute(sql, invoiceLine);
                    if (rows > 0)
                    {
                        response.IsPassed = true;
                        response.SuccessMessage = "InvoiceLine Created Successfully";
                        return response;
                    }
                    else
                    {
                        response.IsPassed = false;
                        response.ErrorMessage = "InvoiceLine Creation Failled";
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                response.IsPassed = false;
                response.ErrorMessage = "For InvoiceLine Excution\n" + ex.Message;
                return response;
            }


        }
        public Response savePatientAssignment(PatientAssignment patientAssignment)
        {
            Response response = new Response();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = @"INSERT INTO [general].[patient_assigment]
                                                                      ([patient_id]
                                                                      ,[assignment_type]
                                                                      ,[assigned_to]
                                                                      ,[Invoice])
                                                                VALUES
                                                                      (@patient_id
                                                                      ,@assignment_type
                                                                      ,@assigned_to
                                                                      ,@Invoice)";
                    var rows = conn.Execute(sql, patientAssignment);
                    if (rows > 0)
                    {
                        response.IsPassed = true;
                        response.SuccessMessage = "Patient Assignment Created Successfully";
                        return response;
                    }
                    else
                    {
                        response.IsPassed = false;
                        response.ErrorMessage = "Patient Assignment Failled";
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                response.IsPassed = false;
                response.ErrorMessage = "For Patient Assignment Excution\n" + ex.Message;
                return response;
            }


        }
        public Response updateInvoice(UpdateInvoice updateInvoice)
        {
            Response response = new Response();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = @"UPDATE [general].[invoice] SET last_operation=@last_operation                                                       
                                                        WHERE consignee = @consignee and code=@code";
                                                                     
                    var rows = conn.Execute(sql, updateInvoice);
                    if (rows > 0)
                    {
                        response.IsPassed = true;
                        response.SuccessMessage = "Invoice Updated Successfully";
                        return response;
                    }
                    else
                    {
                        response.IsPassed = false;
                        response.ErrorMessage = "Invoice Updation Failed";
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                response.IsPassed = false;
                response.ErrorMessage = "For Update Invoice Excution\n" + ex.Message;
                return response;
            }


        }
        public Response updateRegistrationInvoice(Invoice update)
        {
            Response response = new Response();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = @"UPDATE [general].[invoice] SET type=@type                                                       
                                                        WHERE consignee = @consignee";

                    var rows = conn.Execute(sql, update);
                    if (rows > 0)
                    {
                        response.IsPassed = true;
                        response.SuccessMessage = "Invoice Updated Successfully";
                        return response;
                    }
                    else
                    {
                        response.IsPassed = false;
                        response.ErrorMessage = "Invoioce Updation Failled";
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                response.IsPassed = false;
                response.ErrorMessage = "For Update Invoice Excution\n" + ex.Message;
                return response;
            }
        }
    }
}

#endregion 
