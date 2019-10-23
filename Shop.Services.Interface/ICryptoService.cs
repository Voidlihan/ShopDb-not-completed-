using System;
using System.Collections.Generic;
using System.Text;
namespace Shop.Services.Interface
{
    interface ICryptoService
    {
        string EncryptPassword(string password);
        bool VerifyPassword(string password);
    }
}
