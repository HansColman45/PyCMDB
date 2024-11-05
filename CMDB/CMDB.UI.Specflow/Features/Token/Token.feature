Feature: Token
	I want to create, update and delelete a Token

@tag1
Scenario: I want to create a new Token
	Given I want to create a new Token with these details
		| Field        | Value     |
		| AssetTag     | IND       |
		| SerialNumber | 123456789 |
		| Type         | HD Small  |
	When I save the Token
	Then I can find the newly created Token back