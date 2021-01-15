using System;
using System.Collections.Generic;
using System.Text;

namespace TempoTrack.Models
{
    public class Coord
    {
        public double Latitude { get; set; }
        public double Longtitude{ get; set; }

        public Coord(double latitude, double longtitude){
            Latitude = latitude;
            Longtitude = longtitude;
        }
    }
}
