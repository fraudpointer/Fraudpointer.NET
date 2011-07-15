using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BettingDemo.DAL
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public string TypeOfTransaction { get; set; } // "deposit", "play", "withdraw"
        public int BankDeposit { get; set; } // 0 means CC, 1 means Bank, -1 means nothing

        public static Transaction Create (Transaction i_transaction)
        {
            SqlConnection l_sqlConnection = null;
            SqlCommand l_sqlCommand = null;
            string l_strSqlForInsert = "insert into transactions (AccountId, Amount, DateOfTransaction, TypeOfTransaction, BankDeposit) " +
                                       " values ( @AccountId, @Amount, @DateOfTransaction, @TypeOfTransaction, @BankDeposit)";
            SqlDataReader l_sqlDataReader = null;
            try
            {
                l_sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[Common.DbName].ConnectionString);
                l_sqlConnection.Open();

                l_sqlCommand = new SqlCommand(l_strSqlForInsert, l_sqlConnection);

                SqlParameter l_paramAccountId = new SqlParameter("AccountId", SqlDbType.Int);
                l_paramAccountId.Value = i_transaction.AccountId;
                l_sqlCommand.Parameters.Add(l_paramAccountId);

                SqlParameter l_paramAmount = new SqlParameter("Amount", SqlDbType.Decimal);
                l_paramAmount.Value = i_transaction.Amount;
                l_sqlCommand.Parameters.Add(l_paramAmount);

                SqlParameter l_paramDateOfTransaction = new SqlParameter("DateOfTransaction", SqlDbType.DateTime);
                l_paramDateOfTransaction.Value = i_transaction.DateOfTransaction;
                l_sqlCommand.Parameters.Add(l_paramDateOfTransaction);

                SqlParameter l_paramTypeOfTransaction = new SqlParameter("TypeOfTransaction", SqlDbType.VarChar);
                l_paramTypeOfTransaction.Value = i_transaction.TypeOfTransaction;
                l_sqlCommand.Parameters.Add(l_paramTypeOfTransaction);

                SqlParameter l_paramBankDeposit = new SqlParameter("BankDeposit", SqlDbType.Int);
                l_paramBankDeposit.Value = i_transaction.BankDeposit;
                l_sqlCommand.Parameters.Add(l_paramBankDeposit);

                l_sqlCommand.ExecuteNonQuery();

                l_sqlCommand.Dispose();

                // let us get the last id inserted
                string l_strScopeIdentity = "select max(id) as new_id from transactions where AccountId = " +
                                            i_transaction.AccountId;
                
                l_sqlCommand = new SqlCommand(l_strScopeIdentity, l_sqlConnection);

                int l_iNewId = 0;

                l_sqlDataReader = l_sqlCommand.ExecuteReader();
                if ( l_sqlDataReader.Read() )
                {
                    l_iNewId = (int) l_sqlDataReader["new_id"];
                }
                
                i_transaction.Id = l_iNewId;
                return i_transaction;

            }
            catch (Exception ex)
            {
                throw new Exception("Cannot create account, ex: " + ex);
            }
            finally
            {
                if ( l_sqlDataReader != null )
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
    
        } // Create ()
        //------------

        public static int NumberOfDeposits (int i_iAccountId, DateTime i_dtLaterThan)
        {            

            SqlConnection l_sqlConnection = null;
            SqlCommand l_sqlCommand = null;
            SqlDataReader l_sqlDataReader = null;
            int l_result = 0;
            string l_strSqlToExecute =
                "select count(*) as number_of_deposits from transactions where TypeOfTransaction = 'deposit' and AccountId = @param_account_id";
            if ( i_dtLaterThan != DateTime.MinValue )
            {
                l_strSqlToExecute = l_strSqlToExecute + " and DateOfTransaction > @param_date_of_transaction";
            }
            try
            {
                l_sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[Common.DbName].ConnectionString);
                l_sqlConnection.Open();

                l_sqlCommand = new SqlCommand(l_strSqlToExecute, l_sqlConnection);

                SqlParameter l_param = new SqlParameter("param_account_id", SqlDbType.Int);
                l_param.Value = i_iAccountId;
                l_sqlCommand.Parameters.Add(l_param);
                
                if (i_dtLaterThan != DateTime.MinValue)
                {
                    SqlParameter l_paramDateOfTransaction = new SqlParameter("param_date_of_transaction", SqlDbType.DateTime);
                    l_paramDateOfTransaction.Value = i_dtLaterThan;
                    l_sqlCommand.Parameters.Add(l_paramDateOfTransaction);
                }

                l_sqlDataReader = l_sqlCommand.ExecuteReader();

                if (l_sqlDataReader.Read())
                {
                    if (l_sqlDataReader["number_of_deposits"] != DBNull.Value)
                    {
                        l_result = (int)l_sqlDataReader["number_of_deposits"];
                    }
                    else
                    {
                        l_result = 0;
                    }
                    return l_result;
                }
                return l_result;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot find Number of Deposits: ex: " + ex);
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

        } // NumberOfDeposits
        //--------------------

        public static int NumberOfBankDeposits(int i_iAccountId, DateTime i_dtLaterThan)
        {

            SqlConnection l_sqlConnection = null;
            SqlCommand l_sqlCommand = null;
            SqlDataReader l_sqlDataReader = null;
            int l_result = 0;
            string l_strSqlToExecute =
                "select count(*) as number_of_deposits from transactions where TypeOfTransaction = 'deposit' and AccountId = @param_account_id and BankDeposit = 1";
            if (i_dtLaterThan != DateTime.MinValue)
            {
                l_strSqlToExecute = l_strSqlToExecute + " and DateOfTransaction > @param_data_of_transaction";
            }
            try
            {
                l_sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[Common.DbName].ConnectionString);
                l_sqlConnection.Open();

                l_sqlCommand = new SqlCommand(l_strSqlToExecute, l_sqlConnection);

                SqlParameter l_param = new SqlParameter("param_account_id", SqlDbType.Int);
                l_param.Value = i_iAccountId;
                l_sqlCommand.Parameters.Add(l_param);

                if (i_dtLaterThan != DateTime.MinValue)
                {
                    SqlParameter l_paramDateOfTransaction = new SqlParameter("param_date_of_transaction", SqlDbType.DateTime);
                    l_paramDateOfTransaction.Value = i_dtLaterThan;
                    l_sqlCommand.Parameters.Add(l_paramDateOfTransaction);
                }

                l_sqlDataReader = l_sqlCommand.ExecuteReader();

                if (l_sqlDataReader.Read())
                {
                    if ( l_sqlDataReader["number_of_deposits"] != DBNull.Value )
                    {
                        l_result = (int)l_sqlDataReader["number_of_deposits"];    
                    }
                    else
                    {
                        l_result = 0;
                    }
                    
                    return l_result;
                }
                return l_result;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot find Number of Deposits: ex: " + ex);
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

        } // NumberOfBankDeposits
        //------------------------


        public static decimal SumDepositsLaterThan (int i_iAccountId, DateTime i_dtLaterThan)
        {
            SqlConnection l_sqlConnection = null;
            SqlCommand l_sqlCommand = null;
            SqlDataReader l_sqlDataReader = null;
            decimal l_result = 0;
            string l_strSqlToExecute =
                "select sum(Amount) as sum_of_deposits from transactions where TypeOfTransaction = 'deposit' and AccountId = @param_account_id and DateOfTransaction > @param_date_of_transaction";
            try
            {
                l_sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[Common.DbName].ConnectionString);
                l_sqlConnection.Open();

                l_sqlCommand = new SqlCommand(l_strSqlToExecute, l_sqlConnection);

                SqlParameter l_param = new SqlParameter("param_account_id", SqlDbType.Int);
                l_param.Value = i_iAccountId;
                l_sqlCommand.Parameters.Add(l_param);

                SqlParameter l_paramDateOfTransaction = new SqlParameter("param_date_of_transaction", SqlDbType.DateTime);
                l_paramDateOfTransaction.Value = i_dtLaterThan;
                l_sqlCommand.Parameters.Add(l_paramDateOfTransaction);

                l_sqlDataReader = l_sqlCommand.ExecuteReader();

                if (l_sqlDataReader.Read())
                {
                    if ( l_sqlDataReader["sum_of_deposits"] != DBNull.Value )
                    {
                        l_result = (decimal)l_sqlDataReader["sum_of_deposits"];    
                    }
                    else
                    {
                        l_result = 0;
                    }
                    
                    return l_result;
                }
                return l_result;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot find Deposits: ex: " + ex);
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
        } // SumDepositsLaterThan
        //--------------------------

    } // class Transaction
    //---------------------

} // BettingDemo.DAL
// --------------------