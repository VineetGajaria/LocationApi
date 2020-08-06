using System;
using System.Collections.Generic;

namespace LocationApi.DAL
{
    public class PetLocation
    {

        public string CollarBarCode { get; set; }

        public GeoLocation GeoLocation { get; set; }

        public DateTime LocationDateTime { get; set; }

    }
}
