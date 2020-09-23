Feature: GetRecommendedQuantities
	In order to accurately depict how well a prepper is performing
	I will need to fetch the recommended Quantities
	in order to process the Progress Tracker

@RecommendedQuantityRepository
Scenario: Get RecommendedAmount
	When GetRecommended Quantities Is called
	Then the Recommended Quantities are returned 