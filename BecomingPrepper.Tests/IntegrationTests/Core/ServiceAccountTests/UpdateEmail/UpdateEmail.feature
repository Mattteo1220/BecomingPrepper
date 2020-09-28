Feature: UpdateEmail
	In order for users to update their email
	They can update it within their account
	So that they can receive emails

@UserRepository @DisposeUser @NewDbInstantiation
Scenario: Update Email 
	Given A User
	And That user is registered
	When That user updates their email
	Then it is saved to the database