Feature: Assign

Scenario: I want to assign an Account to an existing Identity
	Given An Identity exisist in the system
	And an Account exist as well
	When I assign the account to the identity
	And I fill in the assig form
	Then The account is assigned to the idenity

Scenario Outline: I want to assign a device to an existing Identity
	Given An Identity exisist in the system
	And a <Device> exist as well
	When I assign that <Device> to the identity
	And I fill in the assig form
	Then The <Device> is assigned

	Examples:
		| Device  |
		| Laptop  |
		| Desktop |
		| Token   |
		| Monitor |
		| Docking |