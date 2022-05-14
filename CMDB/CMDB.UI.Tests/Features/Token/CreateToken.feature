Feature: CreateToken

Scenario: I want to create a new token
	Given I want to create a new Token with these details
	| Field        | Value      |
	| AssetTag     | TKN        |
	| SerialNumber | 987654     |
	| Type         | HP Generic |
	When I save the Token
	Then I can find the newly created Token
