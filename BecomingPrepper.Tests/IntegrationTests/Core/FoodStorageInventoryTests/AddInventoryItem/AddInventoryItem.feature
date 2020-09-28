Feature: AddInventoryItem
	In order for preppers to track their inventory
	They will need to be able to add items to their inventory
	So that they can ensure they are on track for being prepared

Background: 
	Given An Inventory
	And That Inventory has been registered

@FoodStorageInventoryRepository @NewDbInstantiation
Scenario: Add Inventory Item
	And The prepper has a new item to add
	When The Prepper adds the new item to their inventory
	Then it is saved in the database