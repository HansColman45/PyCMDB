Feature: Identity
	I want to be able to create, edit an Identity
	I also want to be able to assign and release devices to an Identity aswell as assign and release accounts
	I want to be able to assign and release Mobiles to an Identity as wadd as assign and release Subscriptions

Scenario: I want to create a new Identity
	Given I want to create an Identity with these details
		| Field     | Value                  |
		| FirstName | Test                   |
		| LastName  | CMDB                   |
		| Type      | Werknemer              |
		| Company   | Brightest              |
		| UserId    | Test                   |
		| Language  | NL                     |
		| Email     | Test.CMDB@brightest.be |
	When I save
	Then I can find the newly created Identity back

Scenario Outline: I want to update an existing Identity
	Given An Identity exisist in the system
	When I want to update <field> with <value>
	Then The identity is updated

	Examples:
		| field     | value                     |
		| FirstName | Testje                    |
		| LastName  | CMDBtje                   |
		| Company   | Veepee                    |
		| UserID    | IND851                    |
		| Email     | testje.cmdbtje@veepee.Com |

Scenario: I want to activate an inactive Identity
	Given An inactive Identity exisist in the system
	When I want to activate this identity
	Then The Identity is active

Scenario: I want to deactivate an existing Identity
	Given An acive Identity exisist in the system
	When I want to deactivete the identity whith the reason Test
	Then The Idenetity is inactive

Scenario Outline: I want to assign a device to an existing Identity
	Given An active Identity exisist in the system
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

Scenario Outline: I want to relase a device from an exising Identity
	Given An active Identity exisist in the system
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

Scenario: I want to assign an account to an exising Identity
	Given There is an active Identity existing in the system
	And an Account as well
	When I assign the Account to my Identity
	And I fill in the assignform
	Then The Account is assigned to my Identity

Scenario: I want to release an account from an existing Identity
	Given There is an active Identity existing in the system
	And an Account as well
	And The Identity is assigned to the account as well
	When I release the Identity from the account
	Then The Identity is released from the account

Scenario: I want to assign a Mobile to an existing Identity
	Given There is an Identity existing in the system
	And The mobile is exisiting as well
	When I assign the mobile to the Identity
	And I fill in the assignform for my identity
	Then The mobile is assigned to the Identity

Scenario: I want to release a mobile from an exising Idenity
	Given There is an Identity existing in the system
	And The mobile is exisiting as well
	And The mobile is assigned to my identity
	When I relase the mobile from my identity
	Then The mobile is released