using System.Collections.Generic;

namespace BecomingPrepper.Api.Objects
{
    public class FoodStorageInventoryInfo
    {
        public string AccountId { get; set; }
        public List<InventoryInfo> Inventory { get; set; }
    }
}
