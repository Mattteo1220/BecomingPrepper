Feature: DeleteTip
	In order to have the most accurate tips for prepper
	When a tip is out of date or no longer relevant
	Then the tip can be deleted

Background: 
	Given A Prep Guide

@PrepGuideRepository @DisposePrepGuide
Scenario: Delete Tip
	Given The Prep Guide Already Exists
	When The Tip within the PrepGuide Needs deleting
	Then It is Removed from the PrepGuide