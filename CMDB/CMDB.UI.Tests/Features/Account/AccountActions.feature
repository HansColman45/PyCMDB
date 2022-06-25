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
	And an Identity exist as well
	When I assign the identity to my account
	And I fill in the assig form for my account
	Then The identity is assigned to my account

Scenario: 4 I want to release an assigned Identity
	Given There is an account existing
	And There is an Identity assigned
	When I release the Identity
	And I fill in the release form
	Then The identity is released from my account