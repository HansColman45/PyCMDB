Feature: AccountActions

Scenario: 1 I want to deactivate an existing Account
	Given There is an active account existing
	When I deactivate the account with reason Test
	Then the account is inactive

Scenario: 2 I want to activate an existing inactive account
	Given There is an inactive account existing
	When I activate the account
	Then The account is active

Scenario: 3 I want to assign an existing Identity to my account
	Given There is an account existing
	And an Identy exist as well
	When I assign the identity to my account
	And I fill in the assig form for my account
	Then The identity is assigned to my account