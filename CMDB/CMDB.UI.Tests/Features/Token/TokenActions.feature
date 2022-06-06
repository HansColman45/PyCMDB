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
	And an Identity exist as well
	When I assign the Token to the Identity
	And I fill in the assign form for my Token
	Then The Identity is assigned to the Token

Scenario: I want to release an assigned identity from my Token
	Given There is an token existing
	And an Identity exist as well
	And that Identity is assigned to my token
	When I release that identity from my token
	And I fill in the release form for my token
	Then The identity is released from my token