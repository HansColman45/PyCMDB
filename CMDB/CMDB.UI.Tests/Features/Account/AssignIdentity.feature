Feature: AssignIdentity

Scenario: I want to assign an existing Identity to my account
	Given There is an account existing
	And an Identy exist as well
	When I assign the identity to my account
	And I fill in the assig form for my account
	Then The identity is assigned to my account