using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace LocationApi.DAL
{
    public interface IPetLocationRepository
    {
        IConfiguration Configuration { get; }

        void AddPetLocation(PetLocation petLocation);

        IList<PetLocation> GetPetLocation(string collarBarCode);

    }
}