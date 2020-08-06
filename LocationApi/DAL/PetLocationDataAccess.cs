using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LocationApi.DAL
{
    public class PetLocationDataAccess : IPetLocationRepository
    {

        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment environment;

        private string jsonPetLocationFilePath;

        public PetLocationDataAccess(IConfiguration config, IWebHostEnvironment env)
        {
            Configuration = config;
            environment = env;

            jsonPetLocationFilePath = Configuration["Paths:jsonPetLocationFilePath"];
            jsonPetLocationFilePath = environment.WebRootPath + jsonPetLocationFilePath;
        }

        public void AddPetLocation(PetLocation petLocation)
        {
            List<PetLocation> petLocationList = GetPetLocationListJson();

            petLocationList.Add(petLocation);
            var jsonPetOwner = JsonConvert.SerializeObject(petLocationList);
            File.WriteAllText(jsonPetLocationFilePath, jsonPetOwner);
        }

        public IList<PetLocation> GetPetLocation(string collarBarCode)
        {
            List<PetLocation> petLocationList = GetPetLocationListJson();

            return petLocationList.Where(p => p.CollarBarCode == collarBarCode).ToList();
        }

        private List<PetLocation> GetPetLocationListJson()
        {
            string buffer = File.ReadAllText(jsonPetLocationFilePath);
            List<PetLocation> petLocationList = JsonConvert.DeserializeObject<List<PetLocation>>(buffer);
            return petLocationList;
        }

    }
}
