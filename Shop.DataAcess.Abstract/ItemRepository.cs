using System;
using System.Collections.Generic;
using System.Text;
using Shop.Domain;
using System.Data.SqlClient;
using System.Data.Common;

namespace Shop.DataAcess.Abstract
{
    public class ItemRepository : Item
    { 
        private readonly string connectionString;
        private readonly DbProviderFactory providerFactory;
        public void Add(Item item)
        {
            using (DbConnection connection = providerFactory.CreateConnection())
            using (DbCommand command = connection.CreateCommand())
            {
                string query = $"insert into Item (ID, Name, ImagePath, Price, Description, CategoryID) values(@ID, " +
                $"@Name, " +
                $"@ImagePath, " +
                $"@Price, " +
                $"@Description " +
                $"@CategoryID);";
                command.CommandText = query;

                DbParameter parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.Guid;
                parameter.ParameterName = "@ID";
                parameter.Value = item.ID;
                command.Parameters.Add(parameter);

                parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.Guid;
                parameter.ParameterName = "@Name";
                parameter.IsNullable = true;
                parameter.Value = item.Name;
                command.Parameters.Add(parameter);

                parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.Guid;
                parameter.ParameterName = "@ImagePath";
                parameter.Value = item.ImagePath;
                command.Parameters.Add(parameter);

                parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.Guid;
                parameter.ParameterName = "@Price";
                parameter.Value = item.Price;
                command.Parameters.Add(parameter);

                parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.Guid;
                parameter.ParameterName = "@Description";
                parameter.Value = item.Description;
                command.Parameters.Add(parameter);

                parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.Guid;
                parameter.ParameterName = "@CategoryID";
                parameter.Value = item.CategoryID;
                command.Parameters.Add(parameter);

                connection.Open();
                using (DbTransaction transaction = providerFactory.CreateConnection().BeginTransaction())
                {
                    try
                    {
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                        //и так далее тоже самое с другими командами
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
        private void ExecuteCommandsInTransaction(params DbCommand[] commands)
        {
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
            }
            using (DbTransaction transaction = providerFactory.CreateConnection().BeginTransaction())
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
                catch (SqlException exception)
                {
                    transaction.Rollback();
                }
            }
        }
        public void Delete(Guid categoryID)
        {
            string query = "";            
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
