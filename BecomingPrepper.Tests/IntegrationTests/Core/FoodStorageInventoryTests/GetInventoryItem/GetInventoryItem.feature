Feature: GetInventoryItem
	In order for preppers to analyze and make adjustments to their inventory
	They will need to update their existing inventory items as items are used or added
	So that preppers can be more fully in tune with what is contained within their inventory
Background: 
	Given An Inventory
	And That Inventory has been registered


@FoodStorageInventoryRepository
Scenario: Get Inventory Item
	When Get inventory Item is called
	Then that inventory item is returned