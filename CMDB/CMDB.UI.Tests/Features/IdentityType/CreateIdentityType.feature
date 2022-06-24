Feature: CreateIdentityType

Scenario: I want to create a new Identity Type
	Given I want to create an Identity type with these details
	| Field       | Value                                     |
	| Type        | Stagair                                   |
	| Description | person that is working for a short period |
	When I save the Identity type
	Then The I can find the newly create Identity type back