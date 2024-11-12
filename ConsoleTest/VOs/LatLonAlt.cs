using ConsoleTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.VOs
{
    public class LatLon
    {
        public double Lat { get; }
        
        public double Lon { get; }

        public LatLon(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }

        public LatLon GetLatLon() => new LatLon(Lat, Lon);
    }

    public class LatLonAlt : LatLon
    {
        public double Alt { get; }

        public LatLonAlt(double lat, double lon, double alt)
            : base(lat, lon)
        {
            Alt = alt;
        }
    }
}
