using  System;
using System.Collections.Generic;
using System.Text;

namespace todo
{
    /*
     * background location messages
     */
    public class StartLocationServiceMessage
    {
    }
    public class StopLocationServiceMessage
    {
    }
    public class LocationMessage
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public class LocationErrorMessage
    {
    }
}
