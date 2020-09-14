Feature: PrepGuideRepository
	In order to educate preppers on the benefits of being prepared
	The system must render back additional tips, guides, stats etc
	Through the PrepGuideRepository to aid the prepper in BecomingPrepper

	Background: 
		Given A Prep Guide

@PrepGuideRepository @DisposePrepGuide
Scenario: CRUD Tip
		And The Prep Guide Already Exists
		And The prepper needs to add a new tip
	When PrepGuide Repository Update is called
	Then A new Tip is added