Feature: GetInventory
	In order for preppers to see what they have in their inventory
	The system will need to fetch it for them
	So that they can better manage it

Background: 
	Given An Inventory
	And That Inventory has been registered

@FoodStorageInventoryRepository @NewDbInstantiation
Scenario: Get Inventory
	When The prepper Requests to view their inventory
	Then it is fetched from the database