using System;
using System.Collections.Generic;

namespace LocationApi
{
    public class PetLocationReport
    {

        public string CollarBarCode { get; set; }

        public GeoLocation GeoLocation { get; set; }

        public DateTime LocationDateTime { get; set; }

    }
}
