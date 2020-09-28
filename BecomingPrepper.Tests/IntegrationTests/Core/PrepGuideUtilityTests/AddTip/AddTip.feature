Feature: AddTip
	In order to increase our emergency preparedness
	More tips will need to be added
	So that we can continue to learn and progress for emergencies
	
Background: 
	Given A Prep Guide

@PrepGuideRepository @DisposePrepGuide @NewDbInstantiation
Scenario: Add Tip
	And The Prep Guide Already Exists
	When A prepper adds a tip
	Then it is saved in the collection of tips