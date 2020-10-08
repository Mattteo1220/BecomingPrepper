using System;
using BecomingPrepper.Data.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class InventoryEntity
    {
        internal string _itemId;
        [BsonElement]
        public string ItemId
        {
            get
            {
                if (_itemId != null)
                {
                    return _itemId;
                }
                else
                {
                    var category = (int)Enum.Parse(typeof(Category), CategoryId.ToString());
                    return $"Item.{category}.{ProductId}";
                }
            }
            set
            {
                this._itemId = value;
            }
        }

        [BsonElement]
        public Category CategoryId { get; set; }


        [BsonElement]
        public string CategoryName => CategoryId.ToString();

        [BsonElement]
        public int ProductId
        {
            get;
            set;
        }

        private string _productName;
        [BsonElement]
        public string ProductName
        {
            get
            {
                switch (CategoryId)
                {
                    case Category.Grains:
                        var grain = (Grain)ProductId;
                        return grain.ToString();
                    case Category.CannedOrDriedMeats:
                        var meat = (CannedOrDriedMeat)ProductId;
                        return meat.ToString();
                    case Category.FatsAndOils:
                        var fatAndOil = (FatAndOil)ProductId;
                        return fatAndOil.ToString();
                    case Category.Beans:
                        var bean = (Bean)ProductId;
                        return bean.ToString();
                    case Category.Dairy:
                        var dairy = (Dairy)ProductId;
                        return dairy.ToString();
                    case Category.Sugars:
                        var sugar = (Sugar)ProductId;
                        return sugar.ToString();
                    case Category.CookingEssentials:
                        var essentials = (CookingEssentials)ProductId;
                        return essentials.ToString();
                    case Category.DriedFruitsAndVegetables:
                        var driedFruit = (DriedFruitAndVegetable)ProductId;
                        return driedFruit.ToString();
                    case Category.CannedFruitsAndVegetables:
                        var cannedFruit = (CannedFruitAndVegetable)ProductId;
                        return cannedFruit.ToString();
                    case Category.Water:
                        var water = (Water)ProductId;
                        return water.ToString();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set => _productName = value;
        }
        [BsonElement]
        public string ExpiryDateRange { get; set; }
        [BsonElement]
        public int Quantity { get; set; }
        [BsonElement]
        public double Weight { get; set; }
        [BsonElement]
        public DateTime ModifiedDate { get; set; }
        [BsonElement]
        public string ModifiedBy { get; set; }

    }
}
