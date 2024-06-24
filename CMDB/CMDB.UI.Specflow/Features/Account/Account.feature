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