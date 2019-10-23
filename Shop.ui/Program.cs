using Shop.Domain;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

/*
 *  1.Регистрация и вход (смс/email)
 *  2.История покупок
 *  3.Категории и товары (картинка в файловой системе)
 *  4.Покупка(корзина), оплата и доставка (PayPal, Qiwi, ETC)
 *  5.Комментарии и рейтинги
 *  6.Поиск (пагинация)
 *  
 *  Кто сделает 3 версии(Подключенный, автономный и EF) получит автомат на экзамене
 */

namespace Shop.ui
{
    class Program
    {
        private static string connectionString = "DebugConnectionString";
        static void Main(string[] args)
        {            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot configurationRoot = builder.Build();
            string providerName = configurationRoot.GetSection("AppConfig").
                GetChildren().
                Single().Value;
            
            Category category = new Category
            {
                Name = "Бытовая техника",
                ImagePath = "C:/data"
            };
            string connectionString = configurationRoot.GetConnectionString("DebugConnectionString");
            using (var context = new ShopContext(connectionString))
            {
                var item = new Item
                {
                    Name = "Chiller",
                    ImagePath = "C://folder/img.png",
                    Description = "good",
                    Price = 12_321
                };
                context.Add(category);
                context.Add(item);
                context.SaveChanges();
            }
        }
        private static void Pagination()
        {
            int pagezise = 6;
            string query = "SELECT TOP " + pagesize +
            " * FROM Items ORDER BY Id";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Items");
        } 
        private static void Pay()
        {

        }
        static void Menu()
        {

            int answer = 0;
            Console.WriteLine("1.Регистрация");
            Console.WriteLine("2.Вход");
            Console.WriteLine("0.Выход");
            answer = Console.ReadLine();
            while (answer != 0)
            {
                switch(answer)
                {
                    case 1:
                        Reg();
                        break;
                    case 2:
                        Auth();
                        break;
                }
            }
        }
        private static void Reg()
        {            
            Console.WriteLine("Придумйте пароль:");
            const string accountSid = "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            const string authToken = "your_auth_token";
            string password = Console.ReadLine();
            Console.WriteLine("Введите свой email:");
            string email = Console.ReadLine();
            Console.WriteLine("Введите свой адрес:");
            string address = Console.ReadLine();
            Console.WriteLine("Введите свой номер телефона:");
            string phone = Console.ReadLine();
            Console.WriteLine("Подтвердите пароль:");
            string vpassword;
            Console.WriteLine("Код подтверждения:");

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "This is the ship that made the Kessel Run in fourteen parsecs?",
                from: new Twilio.Types.PhoneNumber("+15017122661"),
                to: new Twilio.Types.PhoneNumber("+77473117378")
            );

            Console.WriteLine(message.Sid);
            User user = new User
            {
                Password = password,
                Email = email,
                Address = address,
                Phonenumber = phone
            };
            using(var context = new ShopContext(connectionString))
            {
                context.Add(user);
                context.SaveChanges();
            }
        }
        private static void Auth()
        {
            Console.WriteLine("Введите email:");
            string email = Console.ReadLine();
            Console.WriteLine("Введите пароль:");
            string password = Console.ReadLine();            
        }
    }
}