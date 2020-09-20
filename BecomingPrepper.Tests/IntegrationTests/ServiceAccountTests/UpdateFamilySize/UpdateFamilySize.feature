Feature: UpdateFamilySize
	In order for families to stay on track of their objective
	Families will need to manage their portfolio
	So that they can prepare in the best way possible for emergencies

@UserRepository @DisposeUser
Scenario: Update Family Size
	Given A User
	And That user is registered
	When That user updates their Family Size
	Then the new family size is returned from the database