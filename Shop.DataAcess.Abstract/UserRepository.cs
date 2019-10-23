using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Shop.Domain;
using System.Data.SqlClient;

namespace Shop.DataAcess.Abstract
{
    public class UserRepository
    {
        private DbConnection connection;

        public UserRepository(DbConnection connection)
        {
            this.connection = connection;
        }
        public void Add(User user)
        {
          
        }
        public void Delete(Guid id)
        {

        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
