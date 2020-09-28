Feature: Login
	In order for a user to login to their account
	They will enter their credentials and have them validated
	So that they can use the application to inventory their food storage.

@UserRepository @DisposeUser @NewDbInstantiation
Scenario: Login User
	Given A User
	And That user is registered
	When the user logs in
	Then they are verified
