Feature: AddInventory
	In order to register an account completely
	The prepper will need to create their inventory
	So that they can manage their inventory Items
Background: 
	Given An Inventory

@FoodStorageInventoryRepository @NewDbInstantiation
Scenario: Add Inventory
	When a Prepper creates their inventory
	Then the Inventory is set up in the Database
