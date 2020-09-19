Feature: ExceptionLogger
	In order to address issues
	The system will need to log additional information when an error is thrown
	So that the issues can be diagnosed, understood and addressed.

Background: 
	Given system information needs to be logged

@ExceptionLogger @Ignore @Manual
Scenario: Log Error
	When LogError is called
	Then the exception is stored in the ExceptionLogs in the Mongo Database

@ExceptionLogger @Ignore @Manual
Scenario: Log Information
	When LogInformation is called
	Then The infromation is stored in the ExceptionLogs in the mongo datbase

@ExceptionLogger @Ignore @Manual
Scenario: Log Warning
	When LogWarning is called
	Then the warning is stored in the ExceptionLogs in the mongo Database