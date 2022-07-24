Feature: CreateAccountType

Scenario: I want to create an accounttype
	Given I want to create an accounttype as follows:
	| Field       | Value                       |
	| Type        | Supper user                 |
	| Description | user that has all the power |
	When I save the accounttype
	Then The new accounttype can be find in the system
