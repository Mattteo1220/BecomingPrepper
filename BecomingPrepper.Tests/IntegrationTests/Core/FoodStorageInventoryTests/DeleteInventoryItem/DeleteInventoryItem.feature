Feature: DeleteInventoryItem
	In order for preppers to actualize their goals
	Some of the inventory items will be consumed, lost, expired etc
	So the system will allow the prepper to update their inventory by deleting these items

Background: 
	Given An Inventory
	And That Inventory has been registered

@FoodStorageInventoryRepository
Scenario: Delete Inventory Item
	When A Prepper decides to delete an item from their inventory
	Then that item is removed from their inventory