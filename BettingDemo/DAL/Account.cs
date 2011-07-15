using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BettingDemo.DAL
{
    public class Account
    {
        public int Id { get; set; }
        public int TitleId { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public int CountryId { get; set; }
        public string AddressStreet { get; set; }
        public string AddressNumber { get; set; }
        public string AddressCity { get; set; }
        public string AddressPostCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public string IpAddressOnRegistration { get; set; }

        public static Account FindByFirstnameAndSurname(string i_strFirstname, string i_strSurname)
        {
            SqlConnection l_sqlConnection = null;
            SqlCommand l_sqlCommand = null;
            SqlDataReader l_sqlDataReader = null;
            Account l_result = null;
            string l_strSqlToExecute =
                "select top 1 * from accounts where Firstname = @param_first_name and Surname = @param_surname";
            try
            {
                l_sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[Common.DbName].ConnectionString);
                l_sqlConnection.Open();

                l_sqlCommand = new SqlCommand(l_strSqlToExecute, l_sqlConnection);
                SqlParameter l_param = new SqlParameter("param_first_name", SqlDbType.VarChar);
                l_param.Value = i_strFirstname;
                l_sqlCommand.Parameters.Add(l_param);

                SqlParameter l_param2 = new SqlParameter("param_surname", SqlDbType.VarChar);
                l_param2.Value = i_strSurname;
                l_sqlCommand.Parameters.Add(l_param2);

                l_sqlDataReader = l_sqlCommand.ExecuteReader();

                while ( l_sqlDataReader.Read() )
                {
                    l_result = BuildFromSqlDataReader(l_sqlDataReader);

                    return l_result;
                }
                return l_result;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot find Account: ex: " + ex);    
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
        } // FindByFirstnameAndSurname ()
        //---------------------------------

        public static Account FindByEmail(string i_strEmail)
        {
            SqlConnection l_sqlConnection = null;
            SqlCommand l_sqlCommand = null;
            SqlDataReader l_sqlDataReader = null;
            Account l_result = null;
            string l_strSqlToExecute =
                "select top 1 * from accounts where Email = @param_email";
            try
            {
                l_sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[Common.DbName].ConnectionString);
                l_sqlConnection.Open();

                l_sqlCommand = new SqlCommand(l_strSqlToExecute, l_sqlConnection);

                SqlParameter l_param = new SqlParameter("param_email", SqlDbType.VarChar);
                l_param.Value = i_strEmail;
                l_sqlCommand.Parameters.Add(l_param);

                l_sqlDataReader = l_sqlCommand.ExecuteReader();

                while (l_sqlDataReader.Read())
                {
                    l_result = BuildFromSqlDataReader(l_sqlDataReader);
                    return l_result;
                }
                return l_result;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot find Account: ex: " + ex);
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
        } // FindByEmail ()
        //---------------------------------

        public static Account FindByUsername(string i_strUsername)
        {
            SqlConnection l_sqlConnection = null;
            SqlCommand l_sqlCommand = null;
            SqlDataReader l_sqlDataReader = null;
            Account l_result = null;
            string l_strSqlToExecute =
                "select top 1 * from accounts where Username = @param_username";
            try
            {
                l_sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[Common.DbName].ConnectionString);
                l_sqlConnection.Open();

                l_sqlCommand = new SqlCommand(l_strSqlToExecute, l_sqlConnection);                
                
                SqlParameter l_param = new SqlParameter("param_username", SqlDbType.VarChar);
                l_param.Value = i_strUsername;
                l_sqlCommand.Parameters.Add(l_param);

                l_sqlDataReader = l_sqlCommand.ExecuteReader();

                while (l_sqlDataReader.Read())
                {
                    l_result = BuildFromSqlDataReader(l_sqlDataReader);
                    return l_result;
                }
                return l_result;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot find Account: ex: " + ex);
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
        } // FindByUsername ()
        //---------------------------------

        public static Account Create (Account i_accountToCreate)
        {
            SqlConnection l_sqlConnection = null;
            SqlCommand l_sqlCommand = null;
            string l_strSqlForInsert = "insert into accounts (TitleId, Firstname, Surname, CountryId, AddressStreet, " +
                                       " AddressNumber, AddressCity, AddressPostCode, DateOfBirth, Email, Telephone, Mobile, Username, Password, DateOfRegistration, IpAddressOnRegistration ) " +
                                       " values ( @TitleId, @Firstname, @Surname, @CountryId, @AddressStreet, @AddressNumber, " +
                                       " @AddressCity, @AddressPostCode, @DateOfBirth, @Email, @Telephone, @Mobile, @Username, @Password, @DateOfRegistration, @IpAddressOnRegistration )";
            try
            {
                l_sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[Common.DbName].ConnectionString);
                l_sqlConnection.Open();

                l_sqlCommand = new SqlCommand(l_strSqlForInsert, l_sqlConnection);

                SqlParameter l_paramTitleId = new SqlParameter("TitleId", SqlDbType.Int);
                l_paramTitleId.Value = i_accountToCreate.TitleId;
                l_sqlCommand.Parameters.Add(l_paramTitleId);

                SqlParameter l_paramFirstName = new SqlParameter("Firstname", SqlDbType.NVarChar);
                l_paramFirstName.Value = i_accountToCreate.Firstname;
                l_sqlCommand.Parameters.Add(l_paramFirstName);

                SqlParameter l_paramSurname = new SqlParameter("Surname", SqlDbType.NVarChar);
                l_paramSurname.Value = i_accountToCreate.Surname;
                l_sqlCommand.Parameters.Add(l_paramSurname);

                SqlParameter l_paramCountryId = new SqlParameter("CountryId", SqlDbType.Int);
                l_paramCountryId.Value = i_accountToCreate.CountryId;
                l_sqlCommand.Parameters.Add(l_paramCountryId);

                SqlParameter l_paramAddressStreet = new SqlParameter("AddressStreet", SqlDbType.NVarChar);
                l_paramAddressStreet.Value = i_accountToCreate.AddressStreet;
                l_sqlCommand.Parameters.Add(l_paramAddressStreet);

                SqlParameter l_paramAddressNumber = new SqlParameter("AddressNumber", SqlDbType.NVarChar);
                if ( String.IsNullOrEmpty(i_accountToCreate.AddressNumber))
                {
                    l_paramAddressNumber.Value = DBNull.Value;    
                }                
                else
                {
                    l_paramAddressNumber.Value = i_accountToCreate.AddressNumber;
                }
                l_sqlCommand.Parameters.Add(l_paramAddressNumber);

                SqlParameter l_paramAddressCity = new SqlParameter("AddressCity", SqlDbType.NVarChar);
                l_paramAddressCity.Value = i_accountToCreate.AddressCity;
                l_sqlCommand.Parameters.Add(l_paramAddressCity);

                SqlParameter l_paramAddressPostCode = new SqlParameter("AddressPostCode", SqlDbType.NVarChar);
                l_paramAddressPostCode.Value = i_accountToCreate.AddressPostCode;
                l_sqlCommand.Parameters.Add(l_paramAddressPostCode);

                SqlParameter l_paramDateOfBirth = new SqlParameter("DateOfBirth", SqlDbType.DateTime);
                l_paramDateOfBirth.Value = i_accountToCreate.DateOfBirth;
                l_sqlCommand.Parameters.Add(l_paramDateOfBirth);                               

                SqlParameter l_paramEmail = new SqlParameter("Email", SqlDbType.NVarChar);
                l_paramEmail.Value = i_accountToCreate.Email;
                l_sqlCommand.Parameters.Add(l_paramEmail);

                SqlParameter l_paramTelephone = new SqlParameter("Telephone", SqlDbType.NVarChar);
                if (String.IsNullOrEmpty(i_accountToCreate.Telephone))
                {
                    l_paramTelephone.Value = DBNull.Value;
                }
                else
                {
                    l_paramTelephone.Value = i_accountToCreate.Telephone;
                }
                l_sqlCommand.Parameters.Add(l_paramTelephone);

                SqlParameter l_paramMobile = new SqlParameter("Mobile", SqlDbType.NVarChar);
                if (String.IsNullOrEmpty(i_accountToCreate.Mobile))
                {
                    l_paramMobile.Value = DBNull.Value;
                }
                else
                {
                    l_paramMobile.Value = i_accountToCreate.Mobile;
                }
                l_sqlCommand.Parameters.Add(l_paramMobile);

                SqlParameter l_paramUsername = new SqlParameter("Username", SqlDbType.NVarChar);
                l_paramUsername.Value = i_accountToCreate.Username;
                l_sqlCommand.Parameters.Add(l_paramUsername);

                SqlParameter l_paramPassword = new SqlParameter("Password", SqlDbType.NVarChar);
                l_paramPassword.Value = i_accountToCreate.Password;
                l_sqlCommand.Parameters.Add(l_paramPassword);

                SqlParameter l_paramDateOfRegistration = new SqlParameter("DateOfRegistration", SqlDbType.DateTime);
                l_paramDateOfRegistration.Value = i_accountToCreate.DateOfRegistration;
                l_sqlCommand.Parameters.Add(l_paramDateOfRegistration);

                SqlParameter l_paramIpAddressOnRegistration = new SqlParameter("IpAddressOnRegistration", SqlDbType.NVarChar);
                if (String.IsNullOrEmpty(i_accountToCreate.Mobile))
                {
                    l_paramIpAddressOnRegistration.Value = DBNull.Value;
                }
                else
                {
                    l_paramIpAddressOnRegistration.Value = i_accountToCreate.IpAddressOnRegistration;
                }
                l_sqlCommand.Parameters.Add(l_paramIpAddressOnRegistration);                

                l_sqlCommand.ExecuteNonQuery();
                             
                // check that the account has been created
                Account l_accountFound = FindByEmail(i_accountToCreate.Email);
                return l_accountFound;

            }
            catch (Exception ex)
            {
                throw new Exception("Cannot create account, ex: " + ex);
            }
            finally
            {
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
       
        } // Create ()
        //------------

        private static Account BuildFromSqlDataReader(SqlDataReader i_sqlDataReader)
        {
            Account l_result;

            l_result = new Account();
            l_result.AddressCity = (string)i_sqlDataReader["AddressCity"];
            if ( i_sqlDataReader["AddressNumber"] != DBNull.Value )
            {
                l_result.AddressNumber = (string)i_sqlDataReader["AddressNumber"];    
            }            
            l_result.AddressPostCode = (string)i_sqlDataReader["AddressPostCode"];
            l_result.AddressStreet = (string)i_sqlDataReader["AddressStreet"];
            l_result.CountryId = (int)i_sqlDataReader["CountryId"];
            l_result.DateOfBirth = (DateTime)i_sqlDataReader["DateOfBirth"];
            l_result.DateOfRegistration = (DateTime)i_sqlDataReader["DateOfRegistration"];
            l_result.Email = (string)i_sqlDataReader["Email"];
            l_result.Firstname = (string)i_sqlDataReader["Firstname"];
            l_result.Id = (int)i_sqlDataReader["Id"];
            if ( i_sqlDataReader["Mobile"] != DBNull.Value )
            {
                l_result.Mobile = (string)i_sqlDataReader["Mobile"];    
            }            
            l_result.Password = (string)i_sqlDataReader["Password"];
            l_result.Surname = (string)i_sqlDataReader["Surname"];
            if ( i_sqlDataReader["Telephone"] != DBNull.Value )
            {
                l_result.Telephone = (string)i_sqlDataReader["Telephone"];    
            }            
            l_result.TitleId = (int)i_sqlDataReader["TitleId"];
            l_result.Username = (string)i_sqlDataReader["Username"];
            if ( i_sqlDataReader["IpAddressOnRegistration"] != DBNull.Value )
            {
                l_result.IpAddressOnRegistration = (string)i_sqlDataReader["IpAddressOnRegistration"];    
            }            
            return l_result;
               
        }
    } // Account
    //-----------

} // BettingDemo.DAL
//-------------------