namespace BecomingPrepper.Api.Objects
{
    public class InventoryInfo
    {
        public string ItemId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string ExpiryDateRange { get; set; }

        public int Quantity { get; set; }

        public double Weight { get; set; }
    }
}
