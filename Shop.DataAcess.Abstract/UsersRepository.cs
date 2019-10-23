using System;
using System.Collections.Generic;
using Shop.Domain;
using System.Data.SqlClient;
using System.Text;
using System.Data.Common;
using Shop.DataAcess.Abstract;

namespace Shop.DataAcess.Abstract
{
    class UsersRepository
    {
        private readonly string connectionString;

        public UsersRepository()
        {
            this.connectionString = connectionString;
        }

        public void Add(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand sqlCommand = connection.CreateCommand())
            {
                string query = $"insert into Users (ID, Email, Address, Password, VerificationCode) values(@ID, " +
                $"@Email, " +
                $"@Address, " +
                $"@Password, " +
                $"@VerificationCode);";
                sqlCommand.CommandText = query;

                SqlParameter parameter = new SqlParameter();
                parameter.SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                parameter.ParameterName = "@ID";
                parameter.Value = user.ID;
                sqlCommand.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                parameter.ParameterName = "@Email";
                parameter.Value = user.Email;
                sqlCommand.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                parameter.ParameterName = "@Address";
                parameter.IsNullable = true;
                parameter.Value = user.Address;
                sqlCommand.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                parameter.ParameterName = "@Password";
                parameter.Value = user.Password;
                sqlCommand.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                parameter.ParameterName = "@VerificationCode";
                parameter.Value = user.VerificationCode;
                sqlCommand.Parameters.Add(parameter);

                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        sqlCommand.Transaction = transaction;
                        sqlCommand.ExecuteNonQuery();
                        //и так далее тоже самое с другими командами
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                    ExecuteCommandsInTransaction(sqlCommand, sqlCommand);
                }
            }
        }
        private void ExecuteCommandsInTransaction(params SqlCommand[] commands)
        {
            using (SqlConnection connection = new SqlConnection())
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var command in commands)
                    {
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                        //и так далее тоже самое с другими командами
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
        public void Delete(Guid categoryID)
        {
            throw new NotImplementedException();
        }

        public ICollection<Category> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand sqlCommand = connection.CreateCommand())
            {
                string query = "select * from Categories;";
                sqlCommand.CommandText = query;
                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                List<Category> categories = new List<Category>();
                while (sqlDataReader.Read())
                {
                    categories.Add(new Category
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        CreationDate = DateTime.Parse(sqlDataReader["creationDate"].ToString()),
                        DeletedDate = DateTime.Parse(sqlDataReader["deletedDate"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        ImagePath = sqlDataReader["ImagePath"].ToString()
                    });
                }
                return categories;
            }
        }
    }
}
