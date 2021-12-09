Feature: CreateAccount

Scenario: I want to create a Account
	Given I want to create a Account with the following details
		| Field       | Values      |
		| UserId      | Test        |
		| Type        | Normal User |
		| Application | CMDB        |
	When I save the account
	Then The account is saved