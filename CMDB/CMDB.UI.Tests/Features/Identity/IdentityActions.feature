Feature: IdentityActions

Scenario: I want to activate an inactive Identity
	Given An inactive Identity exisist in the system
	When I want to activate this identity
	Then The Identity is active

Scenario: I want to deactivate an existing Identity
	Given An acive Identity exisist in the system
	When I want to deactivete the identity whith the reason Test
	Then The Idenetity is inactive

Scenario: I want to assign an Account to an existing Identity
	Given An Identity exisist in the system
	And an Account exist as well
	When I assign the account to the identity
	And I fill in the assig form
	Then The account is assigned to the idenity

Scenario: I want to release an accunt form an existing Idenity
	Given An Identity exisist in the system
	And an Account exist as well
	And The account is assigned to the Idenitity
	When I release the account from my Identity
	And I fill in the release account form for my Identity
	Then The account is released 

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

Scenario Outline: I want to release a device from an existing Identity
	Given An Identity exisist in the system
	And a <Device> exist as well
	And The <Device> is assigned to the Identity
	When I release the <Device> from the Identity
	And I fill in the release form for my Identity
	Then The <Device> is released from the Identity

	Examples:
		| Device  |
		| Laptop  |
		| Desktop |
		| Token   |
		| Monitor |
		| Docking |