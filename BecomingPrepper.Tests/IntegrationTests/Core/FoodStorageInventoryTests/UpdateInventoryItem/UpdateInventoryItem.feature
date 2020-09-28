Feature: UpdateInventoryItem
	In order for preppers to manage their inventories
	Preppers will be updating their inventory items
	So that they can be better stocked and prepared for emergency situations

Background: 
	Given An Inventory
	And That Inventory has been registered

@FoodStorageInventoryRepository @NewDbInstantiation
Scenario: Update Inventory Item
	When The prepper updates a field within the inventory Item
	Then that field is updated in the database