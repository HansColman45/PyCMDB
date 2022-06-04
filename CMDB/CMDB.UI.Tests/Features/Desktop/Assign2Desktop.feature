Feature: Assign2Desktop

Scenario: I want to assign an existing Identiy to my Desktop
	Given There is an Desktop existing
	And an Identy exist as well
	When I assign the Desktop to the Identity
	And I fill in the assign form for my Desktop
	Then The Identity is assigned to the Desktop