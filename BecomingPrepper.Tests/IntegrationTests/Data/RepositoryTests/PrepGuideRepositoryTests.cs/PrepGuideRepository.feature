Feature: PrepGuideRepository
	In order to educate preppers on the benefits of being prepared
	The system must render back additional tips, guides, stats etc
	Through the PrepGuideRepository to aid the prepper in BecomingPrepper

	Background: 
		Given A Prep Guide

@PrepGuideRepository @DisposePrepGuide
Scenario: Add Tip
		And The prepper needs to add a new tip
	When PrepGuide Repository Add is called
	Then A new Tip is added

@PrepGuideRepository @DisposePrepGuide
Scenario: Get Tip
	And that tip exists in the Database
	When PrepGuide Repository Get is called
	Then That tip is returned

@PrepGuideRepository @DisposePrepGuide @NewDbInstantiation
Scenario: Update Tip
	And that tip exists in the Database
	And that the tip Name is updated
	When PrepGuideRepository update is called
	Then the Tip name is updated and returned

@PrepGuideRepository @DisposePrepGuide @NewDbInstantiation
Scenario: Delete Tip
	And that tip exists in the Database
	When PrepGuideRepository Delete is called
	Then The Tip is deleted