using System.Collections.ObjectModel;
using Sift;

namespace SiftTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var sift = new Client("beb441da830b7bc3");
            // Construct reserved events with known fields
            var createOrder = new CreateOrder
            {
                user_id = "gary",
                order_id = "oid",
                amount = 1000000,
                currency_code = "USD",
                billing_address = new Address {
                    name = "gary",
                    city = "san francisco"
                },
                app = new App {
                    app_name = "my app",
                    app_version = "1.0"
                },
                items = new ObservableCollection<Item>() { new Item{sku="abc"}, new Item{sku="abc"} }
            };

            // Augment with custom fields
            createOrder.AddField("foo", "bar");
            Console.WriteLine("Sending Order");   
            try 
            {
                EventResponse res = sift.SendAsync(new EventRequest
                {
                    Event = createOrder,
                    ReturnScore = true,
                    AbuseTypes = new List<string>() { "payment_abuse", "account_takeover" }
                }).Result;
                Console.WriteLine("Order status: " + res.ScoreResponse.Status);  
            }
            catch (AggregateException ae)
            {
                // Handle InnerException
            }
        }
    }
}