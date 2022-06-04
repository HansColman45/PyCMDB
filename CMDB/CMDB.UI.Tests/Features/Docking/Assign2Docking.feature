Feature: Assign2Docking

Scenario: I want to assign an existing Identiy to my Laptop
	Given There is an Docking existing
	And an Identy exist as well
	When I assign the Docking to the Identity
	And I fill in the assign form for my Docking
	Then The Identity is assigned to the Docking