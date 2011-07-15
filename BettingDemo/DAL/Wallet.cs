using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BettingDemo.DAL
{
    public class Wallet
    {
        public decimal Amount { get; set; }        

        public static Wallet FindByAccountId(int i_iAccountId)
        {
            SqlConnection l_sqlConnection = null;
            SqlCommand l_sqlCommand = null;
            SqlDataReader l_sqlDataReader = null;
            Wallet l_result = null;
            string l_strSqlToExecute =
                "select sum(amount) as sum_amount from transactions where AccountId = @param_account_id";    
            try
            {
                l_sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[Common.DbName].ConnectionString);
                l_sqlConnection.Open();

                l_sqlCommand = new SqlCommand(l_strSqlToExecute, l_sqlConnection);
                SqlParameter l_param = new SqlParameter("param_account_id", SqlDbType.Int);
                l_param.Value = i_iAccountId;
                l_sqlCommand.Parameters.Add(l_param);

                l_sqlDataReader = l_sqlCommand.ExecuteReader();
                if ( l_sqlDataReader.Read() )
                {
                    l_result = new Wallet();
                    if ( l_sqlDataReader["sum_amount"] == DBNull.Value)
                    {
                        l_result.Amount = 0;
                    }
                    else
                    {
                        l_result.Amount = (decimal)l_sqlDataReader["sum_amount"];    
                    }                    
                }
                return l_result;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot find wallet: ex: " + ex);
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
        } // FindByAccountId ()
        //---------------------

    } // class Wallet ()
    //------------------

} // BettingDemo.DAL
//-------------------
