using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BettingDemo.DAL
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Iso2 { get; set; }

        public static Country Find (int i_iCountryId)
        {
            SqlConnection l_sqlConnection = null;
            SqlCommand l_sqlCommand = null;
            SqlDataReader l_sqlDataReader = null;
            Country l_result = null;

            string l_strSqlToExecute = "";
            try
            {
                l_sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[Common.DbName].ConnectionString);
                l_sqlConnection.Open();

                l_strSqlToExecute = "select * from countries where Id = @param_country_id";
                l_sqlCommand = new SqlCommand(l_strSqlToExecute, l_sqlConnection);

                SqlParameter l_paramCountryId = new SqlParameter("param_country_id", SqlDbType.Int);
                l_paramCountryId.Value = i_iCountryId;
                l_sqlCommand.Parameters.Add(l_paramCountryId);
                
                l_sqlDataReader = l_sqlCommand.ExecuteReader();
                if (l_sqlDataReader.Read())
                {
                    l_result = new Country();
                    l_result.Id = (int)l_sqlDataReader["Id"];
                    l_result.Name = (string)l_sqlDataReader["Name"];
                    l_result.Iso2 = (string)l_sqlDataReader["Iso2"];

                    return l_result;

                }
                return l_result;

            }
            catch (Exception ex)
            {
                throw new Exception("Cannot get country: ex: " + ex);
            }
            finally
            {
                if (l_sqlDataReader != null)
                {
                    l_sqlDataReader.Close();
                }
                if (l_sqlCommand != null)
                {
                    l_sqlCommand.Dispose();
                }
                if (l_sqlConnection != null)
                {
                    if (l_sqlConnection.State == ConnectionState.Open)
                    {
                        l_sqlConnection.Close();
                    }
                    l_sqlConnection.Dispose();
                }
            }            
        } // Find ()
        //-----------

        public static List<Country> All ()
        {
            SqlConnection l_sqlConnection = null;
            SqlCommand l_sqlCommand = null;
            SqlDataReader l_sqlDataReader = null;
            List<Country> l_result = new List<Country>();

            string l_strSqlToExecute = "";
            try
            {
                l_sqlConnection =new SqlConnection(ConfigurationManager.ConnectionStrings[Common.DbName].ConnectionString);
                l_sqlConnection.Open();

                l_strSqlToExecute = "select * from countries order by name";
                l_sqlCommand = new SqlCommand(l_strSqlToExecute, l_sqlConnection);

                l_sqlDataReader = l_sqlCommand.ExecuteReader();
                while ( l_sqlDataReader.Read() )
                {
                    Country l_objectToAdd = new Country();
                    l_objectToAdd.Id = (int)l_sqlDataReader["Id"];
                    l_objectToAdd.Name = (string) l_sqlDataReader["Name"];
                    l_objectToAdd.Iso2 = (string) l_sqlDataReader["Iso2"];

                    l_result.Add(l_objectToAdd);

                }
                return l_result;

            }
            catch (Exception ex)
            {
                throw  new Exception("Cannot get countries: ex: " + ex);
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
