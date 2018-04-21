using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var listCar = new List<Car>();
            RealizeCacheRepository repositary = new RealizeCacheRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=AutoDB;Integrated Security=True");

            listCar = repositary.Load("Select * from AutoConfig;");

            foreach (var car in listCar)
            {
                Console.WriteLine($"{car.GetInfo()}______________________");
            }

            Console.ReadKey();
        }
    }
}
