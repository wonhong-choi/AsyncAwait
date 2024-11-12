using ConsoleTest.DTOs;
using ConsoleTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.DAOs
{
    internal class MissileDAO
    {
        private readonly IEngine _engine;

        public MissileDAO(IEngine engine)
        {
            _engine = engine;
            _engine.MissileInfoUpdated += OnMissileInfoUpdated;
        }

        public async void OnMissileShootRequestReceived(int srcId, int targetId)
        {
            var response = await _engine.Receive(new MissileShootRequestCmd()
            {
                MissileId = srcId * 1000 + targetId,
                SrcId = srcId,
                TargetId = targetId,
            });

            response.Accept(this);
        }

        public void Visit(MissileShootResponseCmd missileShootResponseCmd)
        {
            Console.WriteLine($"Shoot > from {missileShootResponseCmd.SrcId} to {missileShootResponseCmd.TargetId} / {missileShootResponseCmd.WhenShot}");
        }

        private void OnMissileInfoUpdated(MissileInfo info)
        {
            foreach (var item in info.Items)
            {
                Console.WriteLine($"[Msl {item.Id}] : {item.LatLonAlt.Lat:N2}, " +
                    $"{item.LatLonAlt.Lon:N2}, " +
                    $"{item.LatLonAlt.Alt:N2}, " +
                    $"{item.Heading:N2}, " +
                    $"{item.Speed:N2}, " +
                    $"{item.MissileState}");
            }
        }
    }
}
