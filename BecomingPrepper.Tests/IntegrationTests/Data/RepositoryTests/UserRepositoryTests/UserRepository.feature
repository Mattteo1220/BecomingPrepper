Feature: UserRepository
	In order for a Prepper to Register and act within BecomingPrepper
	The system will need to act upon a User through the UserRepository
	So that the User Account is altered.

Background: 
	Given A User

@DisposeUser @AddUser @UserRepository
Scenario: Add User
		And That user has never registered
	When UserRepositoryAdd is Called
	Then The user is added to the Mongo Database

@DisposeUser @GetUser @UserRepository
Scenario: Get User
		And That user is registered
	When Get is called
	Then the User Entity should be returned

@UserRepository @DeleteUser
Scenario: Delete User
		And That user is registered
		And That user wants to be deleted
	When Delete is called
	Then The user is removed from the Mongo Database

@DisposeUser @UpdateUser @UserRepository @NewDbInstantiation
Scenario: Update User
		And That user is registered
		And That user has updated a property
	When Update is called
	Then The user with its updated property should be returned
	
	
	
