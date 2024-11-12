using ConsoleTest.Enums;
using ConsoleTest.Utils;
using ConsoleTest.VOs;
using System.Runtime.CompilerServices;

namespace ConsoleTest.Models
{
    public class Missile : BasePlatform
    {
        public MissileStateType State { get; private set; }

        public Missile(LatLonAlt initialPosition, IMoveStrategy moveStrategy) : base(initialPosition, moveStrategy)
        {
        }

        public override void Collide(PlatformStateType targetState)
        {
            if (targetState == PlatformStateType.Active)
            {
                this.State = MissileStateType.Hit;
            }
            else
            {
                this.State = MissileStateType.Miss;
            }
        }
    }
}