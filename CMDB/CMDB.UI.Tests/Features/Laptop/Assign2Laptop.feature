Feature: Assign2Laptop

Scenario: I want to assign an existing Identiy to my Laptop
	Given There is an Laptop existing
	And an Identy exist as well
	When I assign the Laptop to the Identity
	And I fill in the assign form for my Laptop
	Then The Identity is assigned to the Laptop