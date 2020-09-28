Feature: Register
	In order for a prepper to have an account
	They will need to register their data
	in order to inventory their food storage with the application

@UserRepository @DisposeUser @NewDbInstantiation
Scenario: Register User
	Given A User
	And That user has never registered
	When That user registers their data
	Then That user is registered in the Database
