using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Enums
{
    public enum PlatformStateType
    {
        Active,     // 활성 == 정상
        RunDown,    // 멈춤 == 기동불가
        InActive,   // 비활성상태 == 침몰
    }

    public enum MissileStateType
    {
        Flight,     // 비행중
        Hit,        // 명중
        Miss,       // 불명중
    }
}
