Feature: TokenActions

Scenario: I want to deactivate an existing token
Given There is an active token existing
When I want to deactivete the token whith the reason test
Then the token is deactivated

Scenario: I want to activate an inactive token
Given There is an inactive token existing
When I want to activate this token
Then The token is active