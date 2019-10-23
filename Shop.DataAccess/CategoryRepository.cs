using Shop.DataAcess.Abstract;
using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;  

namespace Shop.DataAccess
{
    public class CategoryRepository : ICategoryRepository
    {
        /*
         * 1.Открыть подключение
         * 2.Создать запрос
         * 3.Выполнить запрос
         * 4.Закрыть подключение
         */
        private readonly string connectionString;
        private readonly DbProviderFactory providerFactory;
        public CategoryRepository(string connectionString, string providerName)
        {
            this.connectionString = connectionString;
            providerFactory = DbProviderFactories.GetFactory(providerName);
        }

        public void Add(Category category)
        {
            using (DbConnection connection = providerFactory.CreateConnection())
            using (DbCommand command = connection.CreateCommand())
            {
                string query = $"insert into Categories (id, creationDate, name, ImagePath) values(@ID, " +
                $"@CreationDate, " +
                $"@DeletedDate, " +
                $"@Name, " +
                $"@ImagePath);";
                command.CommandText = query;

                DbParameter parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.Guid;
                parameter.ParameterName = "@ID";
                parameter.Value = category.ID;
                command.Parameters.Add(parameter);

                parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.Guid;
                parameter.ParameterName = "@CreationDate";
                parameter.Value = category.CreationDate;
                command.Parameters.Add(parameter);

                parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.Guid;
                parameter.ParameterName = "@DeletedDate";
                parameter.IsNullable = true;
                parameter.Value = category.DeletedDate;
                command.Parameters.Add(parameter);

                parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.Guid;
                parameter.ParameterName = "@Name";
                parameter.Value = category.Name;
                command.Parameters.Add(parameter);

                parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.Guid;
                parameter.ParameterName = "@ImagePath";
                parameter.Value = category.ImagePath;
                command.Parameters.Add(parameter);

                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
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
            using (DbConnection connection = providerFactory.CreateConnection())
            using (DbCommand command = connection.CreateCommand())            
            {
                string query = "select * from Categories;";
                command.CommandText = query;
                connection.Open();
                DbDataReader sqlDataReader = command.ExecuteReader();
                List<Category> categories = new List<Category>();
                while(sqlDataReader.Read())
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

        public void Update(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
