Feature: DeleteInventory
	In order for some accounts to be removed
	The Prepper of the inventory to manage their inventory
	So that entire inventory can be redone
Background: 
	Given An Inventory
	And That Inventory has been registered

@FoodStorageInventoryRepository
Scenario: Delete Inventory
	When The prepper decides to start over on their inventory
	Then the entire Inventory is deleted