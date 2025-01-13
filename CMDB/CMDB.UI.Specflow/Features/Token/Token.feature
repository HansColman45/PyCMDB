Feature: Token
	I want to create, update and delete a Token

Scenario: I want to create a new Token
	Given I want to create a new Token with these details
		| Field        | Value     |
		| AssetTag     | IND       |
		| SerialNumber | 123456789 |
		| Type         | HD Small  |
	When I save the Token
	Then I can find the newly created Token back

Scenario Outline: I want to update an existing token
	Given There is an active Token existing
	When I update the <field> and change it to <value> and save my Token
	Then I can see the changes

	Examples: 
	| field        | value     |
	| SerialNumber | 987654321 |
	| Type         | HD Big    |

Scenario: I want to delete an existing token
	Given There is an active Token existing
	When I delete the token with reason Test
	Then The token is deleted

Scenario: I want to activate an inactive token
	Given There is an inactive token existing
	When I activate that token
	Then The token is active

Scenario: I want to assign an identity to a token
	Given There is an token existing in the system
	And The Identity to assign to the token exists as well
	When I assign the Identity to the token
	And I fill in the assignform for the token
	Then The Identity is assigned to my token

Scenario: I want to release an identity from a token
	Given There is an token existing in the system
	And The Identity to assign to the token exists as well
	And The Identity is assigned to the token
	When I release the Identity
	Then The Identity is released from the token