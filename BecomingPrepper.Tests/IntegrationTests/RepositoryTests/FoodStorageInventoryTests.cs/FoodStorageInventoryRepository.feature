Feature: FoodStorageInventoryRepository
	In order for a prepper to prepare
	The system will need to gather and store the inventory in the database
	To Prepare for longterm emergency situations


Background: 
	Given An Inventory

@AddInventory @FoodStorageInventoryRepository
Scenario: Add Inventory
		And That Inventory has never been registered
	When FoodStorageInventory Add is Called
	Then The Inventory is added to the Mongo Database

@GetInventory @FoodStorageInventoryRepository
Scenario: Get Inventory
		And That Inventory has been registered
	When FoodStorageInventory Get is called
	Then the Inventory should be returned

@FoodStorageInventoryRepository @DeleteInventory
Scenario: Delete inventory
		And That Inventory has been registered
		And That Inventory needs to be deleted
	When FoodStorageInventoryRepository Delete is called
	Then The Inventory is removed from the Mongo Database

@UpdateInventory @FoodStorageInventoryRepository @NewDbInstantiation @Ignore
Scenario: Update Inventory
		And That Inventory has been registered
		And That Inventory has an updated property
	When FoodStorageInventoryRepository Update is called
	Then The Inventory with its updated property should be returned