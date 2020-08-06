using System.Collections.Generic;

namespace LocationApi.Manager
{
    public interface IPetLocationManager
    {
        void ReceivePetLocation(PetLocationDomainModel petLocationDomainModel);

        IList<PetLocationReport> GetPetLocationReport(string collarBarCode, bool isPremiumUser);

        IList<PetOwner> GetNearbyPetOwners(PetLocationDomainModel petLocationDomainModel);
    }
}