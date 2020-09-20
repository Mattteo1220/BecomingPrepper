Feature: UpdatePassword
	In order for preppers to feel secure about their account
	They have the option of changing their passwords
	So that their accounts can be secure

@UserRepository @DisposeUser
Scenario: Update Password
	Given A User
	And That user is registered
	When that user changes their password
	Then That new password is saved in the database