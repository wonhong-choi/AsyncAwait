using ConsoleTest.Enums;
using ConsoleTest.Utils;
using ConsoleTest.VOs;
using System.Runtime.CompilerServices;

namespace ConsoleTest.Models
{
    public class Platform : BasePlatform
    {
        public PlatformStateType State { get; private set; }
        
        public Platform(LatLonAlt initialPosition, IMoveStrategy moveStrategy) : base(initialPosition, moveStrategy)
        {
        }

        public override void Collide(PlatformStateType platformStateType = PlatformStateType.Active)
        {
            State = PlatformStateType.InActive;
        }
    }
}