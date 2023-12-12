using Dapper;
using DevExpress.XtraPrinting.Native;
using Patient_Managment_System.DTO;
using Patient_Managment_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Managment_System.Data_Access_Layer
{
    public class IDGenerationDbContext
    {
        string connectionString = ConfigurationManager.ConnectionStrings["HahcConnection"].ConnectionString;     
        public int getMaxIdValue(int defination)
        {
            
            SqlConnection connection = new SqlConnection(connectionString);       
            var sql = "select MAX(current_value) from general.id_value where defination=@id";
            var lastId = connection.QueryFirstOrDefault<string>(sql, new {id=defination});
            var centerValue = lastId.Substring(4, 5);
            var maxValue=int.Parse(centerValue);
            return maxValue;
        }
        public int getMaxVouchurID()
        {

            SqlConnection connection = new SqlConnection(connectionString);
            var sql = "select MAX(invoice_id) from general.invoice_operation where (invoice_id like'%REG%') ";
            var lastId = connection.QueryFirstOrDefault<string>(sql);
            var centerValue = lastId.Substring(4, 5);
            var maxValue = int.Parse(centerValue);
            return maxValue;
        }
        public int getMaxDepositID(int defination)
        {

            SqlConnection connection = new SqlConnection(connectionString);
            var sql = "select MAX(current_value) from general.id_value where defination=@id";
            var lastId = connection.QueryFirstOrDefault<string>(sql, new { id = defination });
            var centerValue =lastId is null?"0": lastId.Substring(4, 5);
            var maxValue = int.Parse(centerValue);
            return maxValue;
        }

        public Response saveID(ID ID)
        {
            Response response = new Response(); 
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);          

                string sql = @"insert into general.id_value
                                                           ([defination],
                                                           [current_value]) 
                                                     values
                                                           (@defination,
                                                           @current_value)";
                int row = connection.Execute(sql, ID);
                if(row > 0) 
                {
                    response.IsPassed = true;   
                    return response;    
                }
                else
                {
                    response.IsPassed=false;
                    return response;
                }





            }
            catch (Exception e)
            {
                response.IsPassed = false;
                response.ErrorMessage ="For Inser Id Value\n"+ e.Message;  
                return response;

            }
        }
        //gets all entries of Id_definition Table
        public List<IdDefinitionDetail>getIDDefinitionDetail()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string sql = "select * from [general].id_defination";
          return  connection.Query<IdDefinitionDetail>(sql).ToList();

        }
        //gets menu_definition datas based on parent name
        public List<MenuDefinition> getMenuDefinitions(string parentName)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string sql = "select *from [general].menu_defination where parent=@parent";
            return connection.Query<MenuDefinition>(sql, new {parent=parentName}).ToList();
        }

    }
}
