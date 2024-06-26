Feature: AccountType
	I want to be able te search, create and update an AccountType

Scenario: I want to create an accounttype
	Given I want to create an accounttype as follows
	| Field       | Value                       |
	| Type        | Supper user                 |
	| Description | user that has all the power |
	When I save the accounttype
	Then The new accounttype can be find in the system

Scenario Outline: I want to update an existing AccountType
	Given There is an accounttype existing in the system
	When I update the <Field> and change it to <Value> and I save the accounttype
	Then The account type has been saved
	And the Change is done

	Examples: 
	| Field       | Value      |
	| Type        | root       |
	| Description | full admin |

Scenario: I want to deactivate an existing accountType
	Given There is an accounttype existing in the system
	When I deactivate the accountType with reason Test
	Then The accountType is deacticated

Scenario: I want to acticate an inactive accountType
	Given There is an inactive accountType existing in the system
	When I activate the accountType
	Then The accountType is active