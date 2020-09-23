Feature: RecommendedQuantityRepository
	In order to perform Crud operations on ProgressTracking
	The system will need to interact with the mongo Database collection
	to ensure that the recommendedQuantities are coming back

	Background: 
		Given a Recommended Quantity amount

@RecommendedQuantityRepository
Scenario: Get Recommended Quantity
	Given That recommended Quantity Amount exists in the Mongo Database
	When RecommendedQuantity Get is called
	Then That recommended Quantity is returned

@RecommendedQuantityRepository @DisposeRecommendedQuantityEntity
Scenario: Update Recommended Quantity
	Given That recommended Quantity Amount exists in the Mongo Database
	And The recommended Quantity is updated
	When RecommenedQuantityRepository Update Is Called
	Then The updated RecommendedQuantity is saved and returned from the Mongo Database