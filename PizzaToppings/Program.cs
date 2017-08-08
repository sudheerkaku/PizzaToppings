using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaToppings
{
    public class Program
    {
        public static IEnumerable<KeyValuePair<string, int>> ToppingsWithUsageList;

        public static void Main(string[] args)
        {
            List<Pizza> orderInfo = GetOrderInfo();
            GetToppingsWithUsageCount(orderInfo);
            PrintFrequentlyUsed20Toppings();
        }

        /// <summary>
        /// Print Frequently Used 20 Toppings
        /// </summary>
        public static void PrintFrequentlyUsed20Toppings()
        {
            var Top20Toppings = ToppingsWithUsageList.Take(20);

            foreach (var keyValuePair in Top20Toppings)
            {
                Console.WriteLine(string.Format("Topping: {0} (Ordered {1} times)", keyValuePair.Key, keyValuePair.Value));
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Get Toppings With Usage Count
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns>Enumerable of Key Value Pair</returns>
        public static IEnumerable<KeyValuePair<string, int>> GetToppingsWithUsageCount(List<Pizza> orderInfo)
        {
            var orders = orderInfo;
            var dict = orders.Select(p => string.Join(", ", p.Toppings.OrderBy(v => v)))
                             .GroupBy(t => t)
                             .ToDictionary(d => d.Key, d => d.Count())
                             .OrderByDescending(o => o.Value);

            ToppingsWithUsageList = dict;
            return dict;
        }

        /// <summary>
        /// Get Order Info
        /// </summary>
        /// <returns>List of all pizzas ordered</returns>
        public static List<Pizza> GetOrderInfo()
        {
            try
            {
                var jsonData = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "pizzas.json"));
                return JsonConvert.DeserializeObject<List<Pizza>>(jsonData);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occurred while fetching order info with error msg: {0}", ex.Message);
            }
            return new List<Pizza>();
        }
    }
}
