Feature: LoadLargeInventory
	In order for a prepper with a large inventory to load
	When the inventory is requested
	Then the Large inventory is fetched in less than a few seconds



@FoodStorageInventoryRepository
Scenario: Load Large Inventory
	Given a Large Inventory
	When that inventory is requested
	Then the inventory is returned in less than 60 Seconds