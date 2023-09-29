using DevExpress.XtraPrinting.Native;
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
    public class IdGenerationDataAcess
    {
        string connectionString = ConfigurationManager.ConnectionStrings["HahcConnection"].ConnectionString;

        string maxId = "";
        string center = "";
        int maximumValue = 0;
        int count = 0;
       
        IdGeneration id = new IdGeneration();
        public string idGeneration()
        {
            idGen();
            SqlConnection conn1 = new SqlConnection(connectionString);
            conn1.Open();
            var query = "select MAX(current_value) from general.id_value";
            SqlCommand cmd = new SqlCommand(query, conn1);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                maxId = reader.GetString(0);
            }
            if (maxId == "0")
            {
                maximumValue = 1;
                return "";
            }
            maxId = maxId.Substring(2, 5);
            maximumValue = int.Parse(maxId) + 1;
            var CenterId=maximumValue.ToString().PadLeft(id.Length, '0');
           var test= string.Format($"{id.prefix}" + $"{CenterId}" + $"{id.suffix}", maximumValue);
            return test;
        }
        public void idGen()
        {


            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            var query = "select length,prefix,prefix_separator,suffix_separator,suffix from general.id_defination where id=@id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", 1);
            SqlDataReader reader = cmd.ExecuteReader();



            while (reader.Read())
            {
                id.Length = reader.GetInt32(0);
                id.prefix = reader.GetString(1);
                id.prefix_separator = reader.GetString(2);
                id.suffix_separator = reader.GetString(3);
                id.suffix = reader.GetString(4);
            }
            conn.Close();

            int[] idLen = new int[id.Length];
            id.prefix = id.prefix.Trim() + id.prefix_separator.Trim();

            for (int i = 0; i < id.Length; i++)
            {
                idLen[i] = 0;
                center = idLen[i] + center;
            }


            id.suffix = id.suffix_separator.Trim() + id.suffix.Trim();

        }
        public string idNo()
        {

            try
            {
                if (count == 0)
                {
                    idGen();

                }

                SqlConnection conn1 = new SqlConnection(connectionString);


                int defNo = 1;
             

                var CenterId = maximumValue.ToString().PadLeft(id.Length, '0');

                var PersonId = string.Format($"{id.prefix}" + $"{CenterId}" + $"{id.suffix}", maximumValue);


                conn1.Open();
                string sql = "insert into general.id_value(defination,current_value) values('" + defNo + "','" + PersonId + "')";
                SqlCommand cmd = new SqlCommand(sql, conn1);
                cmd.ExecuteNonQuery();
                conn1.Close();
                maximumValue++;
                 CenterId = maximumValue.ToString().PadLeft(id.Length, '0');

                 PersonId = string.Format($"{id.prefix}" + $"{CenterId}" + $"{id.suffix}", maximumValue);

                return PersonId;





            }
            catch (Exception e)
            {
               return e.Message;

            }
        }
    }
}
