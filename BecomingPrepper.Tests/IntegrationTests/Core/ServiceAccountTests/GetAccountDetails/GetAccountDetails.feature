Feature: GetAccountDetails
	In order for preppers to manage their account details
	They will need to fetch the details from the database
	So that they can make updates to them.

Background: 
	Given A User
	And That user is registered

@UserRepository
Scenario: Get Account Details
	When the account details are requested
	Then they are returned