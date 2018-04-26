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
            string query = "select * from AutoConfig ac inner join AutoType at on ac.CarTypeId = at.id";
            RealizeCacheRepository repositary = new RealizeCacheRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=AutoDB;Integrated Security=True");

            //var car1 = repositary.LoadById(3);

            //var car2 = repositary.LoadById(3);

            //var car3 = repositary.LoadById(4);

            //Console.WriteLine(car3.GetInfo());

            listCar = repositary.Load(query);

            var listCar3 = repositary.Load(query);

            var listCar1 = repositary.LoadFromCacheByLinq(item => item.Id > 3);

            foreach (var car in listCar1)
            {
                Console.WriteLine("{0}____________________", car.GetInfo());
            }

            Console.ReadKey();
        }
    }
}
