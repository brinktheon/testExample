using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {

            var listCar = new List<Car>();
            RealizeCacheRepository repositary = new RealizeCacheRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=AutoDB;Integrated Security=True");

            //var car1 = repositary.LoadById(3);

            //var car2 = repositary.LoadById(3);

            //var car3 = repositary.LoadById(1);

            //Console.WriteLine(car2.GetInfo());

            listCar = repositary.Load("Select * from AutoConfig;");

            var listCar3 = repositary.Load("Select * from AutoConfig;");

            var listCar1 = repositary.LoadFromCacheByLinq(item => item.Id > 3);

            foreach (var car in listCar1)
            {
                Console.WriteLine("{0}____________________", car.GetInfo());
            }

            Console.ReadKey();
        }
    }
}
