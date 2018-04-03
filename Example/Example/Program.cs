using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var listCar = new List<Car>();
            string sql = "Select * From AutoConfig";
            CarRepository repository = new CarRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=AutoDB;Integrated Security=True");

            listCar.AddRange(repository.Load(sql));

            foreach (var car in listCar)
            {
                Console.WriteLine("{0}____________________", car.GetInfo());
            }
            Console.ReadKey();
        }

    }
}
