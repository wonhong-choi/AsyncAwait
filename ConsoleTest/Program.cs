using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Run();

            Console.ReadLine();
        }

        private static async Task Run()
        {
            var engine = new Engine();
            var client = new Client(engine);

            engine.Start();

            List<int> idList = new List<int>();
            int id = 0;
            string name = "Test";
            var random = new Random();

            var times = 0;
            while (times < 20)
            {
                Console.WriteLine($"\n{DateTime.Now} > parsed msg)");

                var val = random.Next(2142);
                switch (val % 3)
                {
                    case 0:
                        client.OnCreateRequestMsgReceived(id, name);
                        idList.Add(id);
                        id++;
                        break;

                    case 1 when idList.Count > 0:
                        client.OnDeleteRequestMsgReceived(idList[0]);
                        break;

                    case 2:
                    default:
                        client.OnNullRequestMsgReceived();
                        break;
                }

                await Task.Delay(1000);
                times++;
            }

            engine.Stop();
        }
    }
}
