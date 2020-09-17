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
Scenario: Get User
		And That user is registered
	When Get is called
	Then the User Entity should be returned

@FoodStorageInventoryRepository @DeleteInventory
Scenario: Delete User
		And That user is registered
		And That user wants to be deleted
	When Delete is called
	Then The user is removed from the Mongo Database

@UpdateInventory @FoodStorageInventoryRepository @NewDbInstantiation
Scenario: Update User
		And That user is registered
		And That user has updated a property
	When Update is called
	Then The user with its updated property should be returned