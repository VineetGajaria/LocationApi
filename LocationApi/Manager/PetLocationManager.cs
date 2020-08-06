using LocationApi.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace LocationApi.Manager
{
    public class PetLocationManager : IPetLocationManager
    {

        private readonly IPetLocationRepository _petLocationRepository;
        private string jsonPetLocationMockDataPath;
        public PetLocationManager(IPetLocationRepository petLocationRepository, IConfiguration config, IWebHostEnvironment env)
        {
            _petLocationRepository = petLocationRepository;

            jsonPetLocationMockDataPath = config["Paths:jsonPetLocationMockDataPath"];
            jsonPetLocationMockDataPath = env.WebRootPath + jsonPetLocationMockDataPath;
        }


        public void ReceivePetLocation(PetLocationDomainModel petLocationDomainModel)
        {

            GeoLocation geoLocation = GetLocationDetails(petLocationDomainModel.CellTowerInfo);

            PetLocation petLocation = new PetLocation()
            {
                CollarBarCode = petLocationDomainModel.CollarBarCode,
                GeoLocation = geoLocation,
                LocationDateTime = DateTime.Now
            };

            _petLocationRepository.AddPetLocation(petLocation);
        }

        /// <summary>
        /// Mock method to get GeoLocation from a geolocation service based on data received from cell tower
        /// Currently using Mock Geo Location file
        /// </summary>
        /// <param name="cellTowerInfo"></param>
        /// <returns></returns>
        private GeoLocation GetLocationDetails(CellTowerInfo cellTowerInfo)
        {
            string buffer = File.ReadAllText(jsonPetLocationMockDataPath);
            List<GeoLocation> geoLocation = JsonConvert.DeserializeObject<List<GeoLocation>>(buffer);
            return geoLocation.Where(g => g.CellTowerId == cellTowerInfo.CellTowerId).FirstOrDefault();
        }

        public IList<PetLocationReport> GetPetLocationReport(string collarBarCode, bool isPremiumUser)
        {
            List<PetLocation> petLocationList = _petLocationRepository.GetPetLocation(collarBarCode).ToList();

            if (isPremiumUser)
                petLocationList = petLocationList.Where(p => p.LocationDateTime >= DateTime.Now.AddDays(-30)).ToList();
            else
                petLocationList = petLocationList.Where(p => p.LocationDateTime >= DateTime.Now.AddHours(-24)).ToList();

            List<PetLocationReport> petLocations = new List<PetLocationReport>();

            //Transform data model to business model
            foreach (PetLocation item in petLocationList)
            {
                PetLocationReport petLocation = new PetLocationReport();
                petLocation.CollarBarCode = item.CollarBarCode;
                petLocation.GeoLocation = item.GeoLocation;
                petLocation.LocationDateTime = item.LocationDateTime;

                petLocations.Add(petLocation);
            }

            return petLocations;
        }

        /// <summary>
        /// Get all owners within 5 miles of a pet 
        /// TODO implement lookup of PetLocations within 5 mile range based on latitude/ longitude and location recorded within last 1 min
        /// TODO Invoke PetOwner API to get details of all the PetOwners using CollarBarCode where PetLocations within 5 miles in last 1 min
        /// Currently returns Mock data
        /// </summary>
        /// <param name="collarBarCode"></param>
        /// <returns></returns>
        public IList<PetOwner> GetNearbyPetOwners(PetLocationDomainModel petLocationDomainModel)
        {
            GeoLocation petGeoLocation = GetLocationDetails(petLocationDomainModel.CellTowerInfo);

            List<PetOwner> nearbyPetOwners = new List<PetOwner>();

            // TODO implement lookup of PetLocations within 5 mile range based on latitude/ longitude and location recorded within last 1 min
            // TODO Invoke PetOwner API to get details of all the PetOwners using CollarBarCode where PetLocations within 5 miles in last 1 min
            // Currently returns Mock data

            PetOwner petOwner = new PetOwner();
            petOwner.FirstName = "TestOwner1";
            petOwner.LastName = "TestOwner1";
            petOwner.AddressInfo = new Address();
            petOwner.AddressInfo.Street = "test";
            petOwner.AddressInfo.City = "Boston";
            petOwner.AddressInfo.State = "MA";

            nearbyPetOwners.Add(petOwner);

            return nearbyPetOwners;
        }

    }
}
