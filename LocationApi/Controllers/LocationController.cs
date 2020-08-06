using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LocationApi.Manager;
using Microsoft.AspNetCore.Authorization;

namespace LocationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {

        private readonly ILogger<LocationController> _logger;
        private IPetLocationManager _petLocationManager;

        public LocationController(ILogger<LocationController> logger, IPetLocationManager petLocationManager)
        {
            _logger = logger;
            _petLocationManager = petLocationManager;
        }

        [HttpPost]
        [Route("ReceivePetLocation")]
        public void ReceivePetLocation(PetLocationDomainModel petLocation)
        {
            _petLocationManager.ReceivePetLocation(petLocation);
        }

        [HttpGet]
        [Route("GetPetLocationReport")]
        public List<PetLocationReport> GetPetLocationReport(string collarBarCode, bool isPremiumUser)
        {
            return _petLocationManager.GetPetLocationReport(collarBarCode, isPremiumUser).ToList();
        }

        [HttpPost]
        [Route("GetNearbyPetOwners")]
        //[Authorize(Roles="PremiumUser")] - Implement in future - Authorize only premium users based on user role 
        public List<PetOwner> GetNearbyPetOwners(PetLocationDomainModel petLocation)
        {
            return _petLocationManager.GetNearbyPetOwners(petLocation).ToList();
        }

    }
}
