Feature: Account
	I want to be able to create, update, deactivate and activate Accounts

Scenario: I want to create an Account
	Given I want to create an Account with the following details
		| Field       | Values      |
		| UserId      | Test        |
		| Type        | Normal User |
		| Application | CMDB        |
	When I save the account
	Then The account is saved

Scenario Outline: I want to change an existing Account
	Given There is an account existing
	When I change the <field> to <newValue> and I save the changes
	Then The changes in account are saved

	Examples:
		| field       | newValue         |
		| UserId      | Testje           |
		| Type        | Administrator    |
		| Application | Active Directory |

Scenario: I want to deactivate an existing Account
	Given There is an active account existing
	When I deactivate the account with reason Test
	Then the account is inactive

Scenario: I want to activate an existing inactive account
	Given There is an inactive account existing
	When I activate the account
	Then The account is active