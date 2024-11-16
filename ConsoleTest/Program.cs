using ConsoleTest.DAOs;
using ConsoleTest.Services;
using ConsoleTest.VOs;
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
            _ = Run();

            Console.ReadLine();
        }

        private static async Task Run()
        {
            var engine = new Engine();

            var platformDAO = new PlatformDAO(engine);
            var missileDAO = new MissileDAO(engine);

            engine.Start();

            for(int times = 0; times < 1000; times++)
            {
                Console.WriteLine($"\n{times} ticks: {DateTime.Now} > parsed msg)");

                SimulateReceivingMsg(times, platformDAO, missileDAO);

                await Task.Delay(1000);
                times++;
            }

            engine.Stop();
        }

        private static void SimulateReceivingMsg(int times, PlatformDAO platformDAO, MissileDAO missileDAO)
        {
            switch (times)
            {
                case 0:
                    platformDAO.OnPlatformCreateRequestReceived(0, new LatLonAlt(37.0, 127.0, 0.0));
                    break;

                case 2:
                    platformDAO.OnPlatformCreateRequestReceived(0, new LatLonAlt(37.0, 127.0, 0.0));
                    break;

                case 4:
                    platformDAO.OnPlatformCreateRequestReceived(1, new LatLonAlt(38.0, 128.0, 0.0));
                    break;

                case 6:
                    platformDAO.OnPlatformCreateRequestReceived(2, new LatLonAlt(40.0, 129.0, 0.0));
                    break;

                case 8:
                    platformDAO.OnPlatformCreateRequestReceived(3, new LatLonAlt(40.0, 129.0, 0.0));
                    break;

                case 10:
                    platformDAO.OnDeleteRequestMsgReceived(3);
                    break;

                case 12:
                    platformDAO.OnPlatformSyncMsgRequestReceived(1, 2);
                    break;

                case 14:
                    missileDAO.OnMissileShootRequestReceived(0, 1);
                    break;

                default:
                    break;
            }
        }
    }
}
