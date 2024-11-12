using ConsoleTest.Enums;
using ConsoleTest.VOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleTest.DTOs.PlatformInfo;

namespace ConsoleTest.DTOs
{
    public class PlatformInfo
    {
        public class PlatformInfoItem
        {
            public int Id { get; set; }
            public LatLonAlt LatLonAlt { get; set; }
            public double Heading { get; set; }
            public double Speed { get; set; }
            public PlatformStateType PlatformState { get; set; }
        }

        public List<PlatformInfoItem> Items { get; set;} = new List<PlatformInfoItem>();

    }

    public class MissileInfo
    {
        public class MissileInfoItem
        {
            public int Id { get; set; }
            public LatLonAlt LatLonAlt { get; set; }
            public double Heading { get; set; }
            public double Speed { get; set; }
            public MissileStateType MissileState { get; set; }
        }

        public List<MissileInfoItem> Items { get; set; } = new List<MissileInfoItem>();

    }
}
