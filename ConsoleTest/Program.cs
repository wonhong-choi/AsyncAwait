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

            var firstClient = new FirstClient(engine);
            var secondClient = new SecondClient(engine);

            engine.Start();

            int id = 0;
            string name = "Test";
            var random = new Random();

            var times = 0;
            while (times < 20)
            {
                Console.WriteLine($"\n{DateTime.Now} > parsed msg)");

                var val = random.Next(2142);
                switch (val % 4)
                {
                    case 0:
                        firstClient.OnCreateRequestMsgReceived(id, name);
                        id++;
                        break;

                    case 1:
                        firstClient.OnDeleteRequestMsgReceived(random.Next(id));
                        break;

                    case 2:
                        secondClient.OnUpdateRequestMsgReceived(random.Next(id));
                        break;

                    case 3:
                    default:
                        firstClient.OnNullRequestMsgReceived();
                        break;
                }

                await Task.Delay(1000);
                times++;
            }

            engine.Stop();
        }
    }
}
