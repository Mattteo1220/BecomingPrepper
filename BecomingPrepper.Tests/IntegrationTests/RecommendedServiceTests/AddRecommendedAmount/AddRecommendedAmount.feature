Feature: AddRecommendedAmount
	In order to ensure range of aiding preppers
	The system will from time to time be updated with new relevant recommendedAmoutns
	So that preppers can have adjusted goals to their liking
Background: 
	Given a Recommended Quantity amount
	And That recommended Quantity Amount exists in the Mongo Database

@Ignore
Scenario: Add Recommended Amount
	When a new Recommended Amount is added
	Then that new amount is saved to the Current collection of recommendedAmounts