Feature: UpdateObjective
	In order for progress tracking to be aligned with the goals of the prepper family
	The family can and will update their objective periodically
	So that their progress can show in regard to their family goals

@UserRepository @DisposeUser @NewDbInstantiation
Scenario: Update Objective
	Given A User
	And That user is registered
	When That user updates their Objective
	Then The updated property is returned from the database