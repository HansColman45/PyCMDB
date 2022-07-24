Feature: UpdateAccountType

Scenario Outline: I want to update an existing AccountType
	Given There is an accounttype existing in the system
	When I update the <Field> and change it to <Value> and I save the accounttype
	Then The account type has been saved
	And the Change is done

	Examples: 
	| Field       | Value      |
	| Type        | root       |
	| Description | full admin |
