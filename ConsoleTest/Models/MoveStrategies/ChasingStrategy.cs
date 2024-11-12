using ConsoleTest.Services;
using ConsoleTest.Utils;
using ConsoleTest.VOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Models
{
    public class ChasingStrategy : IMoveStrategy
    {
        private readonly Platform _target;

        public ChasingStrategy(Platform target)
        {
            _target = target;
        }

        public void MoveImpl(BasePlatform chaser)
        {
            if (CanHitTarget(chaser))
            {
                chaser.SetPosition(_target.LatLonAlt);
                chaser.SetMoveStrategy(new HoldingStrategy());

                chaser.Collide(_target.State);
                _target.Collide();
            }
            else
            {
                var afterLatLon = Chase(chaser);
                chaser.SetPosition(new LatLonAlt(afterLatLon.Lat, afterLatLon.Lon, chaser.LatLonAlt.Alt));
            }
        }

        private bool CanHitTarget(BasePlatform chaser)
        {
            // distance to target
            var distance2Target = Geometry.DistanceBetween(chaser.LatLonAlt.GetLatLon(), _target.LatLonAlt.GetLatLon());
            LatLon afterLatLon = Chase(chaser);

            // distance before and after
            var distance2After = Geometry.DistanceBetween(chaser.LatLonAlt.GetLatLon(), afterLatLon);

            return distance2After >= distance2Target;
        }

        private LatLon Chase(BasePlatform chaser)
        {
            return Geometry.NextPosition(chaser.LatLonAlt.GetLatLon(), Bearing(chaser), chaser.Speed * Engine.INTERVAL_MS);
        }

        private double Bearing(BasePlatform chaser)
        {
            return Geometry.Bearing(chaser.LatLonAlt.GetLatLon(), _target.LatLonAlt.GetLatLon());
        }
    }
}
