Feature: EditAccount

Scenario Outline: I want to change an existing Account
	Given There is an account existing
	When I change the <field> to <newValue> and I save the changes
	Then The changes in account are saved

	Examples:
		| field       | newValue         |
		| UserId      | Testje           |
		| Type        | Administrator    |
		| Application | Active Directory |