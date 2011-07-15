using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BettingDemo.DAL
{
    public class Title
    {
        public int Id { get; set; }
        public string TitleName { get; set; }
        public int Position { get; set; }

        public static List<Title> All ()
        {
            SqlConnection l_sqlConnection = null;
            SqlCommand l_sqlCommand = null;
            SqlDataReader l_sqlDataReader = null;
            List<Title> l_result = new List<Title>();

            string l_strSqlToExecute = "";
            try
            {
                l_sqlConnection =new SqlConnection(ConfigurationManager.ConnectionStrings[Common.DbName].ConnectionString);
                l_sqlConnection.Open();

                l_strSqlToExecute = "select * from titles order by position";
                l_sqlCommand = new SqlCommand(l_strSqlToExecute, l_sqlConnection);

                l_sqlDataReader = l_sqlCommand.ExecuteReader();
                while ( l_sqlDataReader.Read() )
                {
                    Title l_objectToAdd = new Title();
                    l_objectToAdd.Id = (int)l_sqlDataReader["Id"];
                    l_objectToAdd.TitleName = (string) l_sqlDataReader["Title"];
                    l_objectToAdd.Position = (int) l_sqlDataReader["Position"];

                    l_result.Add(l_objectToAdd);

                }
                return l_result;

            }
            catch (Exception ex)
            {
                throw  new Exception("Cannot get titles: ex: " + ex);
            }
            finally
            {
                if ( l_sqlDataReader != null )
                {
                    l_sqlDataReader.Close();
                }
                if ( l_sqlCommand != null )
                {
                    l_sqlCommand.Dispose();
                }
                if ( l_sqlConnection != null)
                {
                    if ( l_sqlConnection.State == ConnectionState.Open)
                    {
                        l_sqlConnection.Close();
                    }
                    l_sqlConnection.Dispose();
                }
            }



        } // All ()
        //----------
    }
}
