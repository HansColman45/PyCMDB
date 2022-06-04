Feature: TokenActions

Scenario: I want to deactivate an existing token
	Given There is an active token existing
	When I want to deactivete the token whith the reason test
	Then the token is deactivated

Scenario: I want to activate an inactive token
	Given There is an inactive token existing
	When I want to activate this token
	Then The token is active

Scenario: I want to assign an existing Identiy to my Token
	Given There is an token existing
	And an Identy exist as well
	When I assign the Token to the Identity
	And I fill in the assign form for my Token
	Then The Identity is assigned to the Token