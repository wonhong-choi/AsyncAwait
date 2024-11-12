using ConsoleTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Models
{
    public class SyncWithOriginStrategy : IMoveStrategy
    {
        private readonly BasePlatform _originator;

        public SyncWithOriginStrategy(BasePlatform originator)
        {
            _originator = originator;
        }

        public void MoveImpl(BasePlatform syncher)
        {
            syncher.SetPosition(_originator.LatLonAlt);
        }
    }
}
