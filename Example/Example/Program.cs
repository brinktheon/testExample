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

            var car1 = repositary.LoadById(3);

            Console.WriteLine(car1.GetInfo());

            //foreach (var car in listCar)
            //{
            //    Console.WriteLine("{0}____________________", car.GetInfo());
            //}

            Console.ReadKey();
        }

    }
}
