using ConsoleTest.Enums;
using ConsoleTest.Utils;
using ConsoleTest.VOs;
using System.Runtime.CompilerServices;

namespace ConsoleTest.Models
{
    public abstract class BasePlatform
    {
        private IMoveStrategy _moveStrategy;

        public LatLonAlt LatLonAlt { get; protected set; }

        public double Heading { get; protected set; }

        public double Speed { get; protected set; }


        public BasePlatform(LatLonAlt initialPosition, IMoveStrategy moveStrategy)
        {
            LatLonAlt = initialPosition;
            _moveStrategy = moveStrategy;
        }

        public void SetMoveStrategy(IMoveStrategy moveStrategy)
        {
            _moveStrategy = moveStrategy;
        }

        public void Move()
        {
            _moveStrategy.MoveImpl(this);
        }

        public virtual void SetPosition(LatLonAlt newPosition)
        {
            LatLonAlt = newPosition;
        }

        public abstract void Collide(PlatformStateType platformStateType = PlatformStateType.Active);
    }
}