using ConsoleTest.VOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Utils
{
    public static class Geometry
    {
        private static double ToRadian(double degree) => degree * Math.PI / 180.0;
        private static double ToDegree(double radian) => radian * 180.0 / Math.PI;

        private const double EARTH_RADIOUS_METER = 6371000;


        public static double DistanceBetween(LatLon from, LatLon to)
        {
            var result = 0.0;

            double deltaLatitude = ToRadian(Math.Abs(from.Lat - to.Lat));
            double deltaLongitude = ToRadian(Math.Abs(from.Lon - to.Lon));

            double sinDeltaLat = Math.Sin(deltaLatitude / 2);
            double sinDeltaLng = Math.Sin(deltaLongitude / 2);
            double squareRoot = Math.Sqrt(sinDeltaLat * sinDeltaLat + Math.Cos(ToRadian(from.Lat) * Math.Cos(ToRadian(to.Lat) * sinDeltaLng * sinDeltaLng)));

            result = 2 * EARTH_RADIOUS_METER * Math.Asin(squareRoot);

            return result;
        }

        public static double DistanceBetween(LatLonAlt from, LatLonAlt to)
        {
            double lat1Rad = ToRadian(from.Lat);
            double lon1Rad = ToRadian(from.Lon);
            double lat2Rad = ToRadian(to.Lat);
            double lon2Rad = ToRadian(to.Lon);

            double dLat = lat2Rad - lat1Rad;
            double dLon = lon2Rad - lon1Rad;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // 두 지점 사이의 평면 거리
            double distance = EARTH_RADIOUS_METER * c;

            // 고도 차이를 포함하여 3D 거리 계산
            double altitudeDiff = to.Alt - from.Alt;
            double distance3D = Math.Sqrt(Math.Pow(distance, 2) + Math.Pow(altitudeDiff, 2)); // 고도 차이를 km로 변환

            return distance3D; // 킬로미터 단위
        }

        public static LatLon NextPosition(LatLon from, double bearing, double distance)
        {
            // 위도, 경도를 라디안으로 변환
            double lat1Rad = ToRadian(from.Lat);
            double lon1Rad = ToRadian(from.Lon);
            double bearingRad = ToRadian(bearing);

            // 거리를 지구 반지름으로 나눔
            double angularDistance = distance / EARTH_RADIOUS_METER;

            // 목적지의 위도 계산
            double lat2Rad = Math.Asin(Math.Sin(lat1Rad) * Math.Cos(angularDistance) + Math.Cos(lat1Rad) * Math.Sin(angularDistance) * Math.Cos(bearingRad));

            // 목적지의 경도 계산
            double lon2Rad = lon1Rad + Math.Atan2(Math.Sin(bearingRad) * Math.Sin(angularDistance) * Math.Cos(lat1Rad), Math.Cos(angularDistance) - Math.Sin(lat1Rad) * Math.Sin(lat2Rad));

            // 라디안을 도 단위로 변환하여 반환
            return new LatLon(ToDegree(lat2Rad), ToDegree(lon2Rad));
        }

        public static double Bearing(LatLon from, LatLon to)
        {
            // 위도와 경도를 라디안으로 변환
            double lat1Rad = ToRadian(from.Lat);
            double lon1Rad = ToRadian(from.Lon);
            double lat2Rad = ToRadian(to.Lat);
            double lon2Rad = ToRadian(to.Lon);

            // Δlon 계산
            double deltaLon = lon2Rad - lon1Rad;

            // 방위각 계산
            double x = Math.Cos(lat2Rad) * Math.Sin(deltaLon);
            double y = Math.Cos(lat1Rad) * Math.Sin(lat2Rad) - Math.Sin(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(deltaLon);

            double bearingRad = Math.Atan2(x, y);

            // 라디안을 도 단위로 변환하고, 0~360 범위로 조정
            double bearingDeg = (ToDegree(bearingRad) + 360) % 360;

            return bearingDeg;
        }


    }
}