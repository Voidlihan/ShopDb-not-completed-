using System;
using System.Collections.Generic;
using System.Text;
using Shop.Domain;
using Shop.DataAcess.Abstract;
using System.Data.Common;
using System.Data.SqlClient;

namespace Shop.DataAccess
{
    public class WholeRepository : IDisposable
    {
        public UserRepository Users { get; set; }
        //в конструкторе открываем подключение
        //реализзуем IDisposable
        //создать переменные для каждого из наших репозиториев, которые пользуются единым подключением
        private readonly DbConnection connection;
        private readonly DbProviderFactory providerFactory;
        public WholeRepository(string providerName, string connectionString)
        {
            providerFactory = DbProviderFactories.GetFactory(providerName);
            connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            Users = new UserRepository(connection);
        }
        public void Dispose()
        {
            connection.Close();
        }
    }
}
