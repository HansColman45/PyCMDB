Feature: IdentityTypeActions

Scenario: 1 I want to deactivate an Identitytype
Given There is an Identity type existing
When I want to deactivate the identity type with reason Test
Then The Identity type is deactivated

Scenario: 2 I want to activate an inactive identitity type
Given There is an inactive Identitytype existing
When I want to activate the Idenity type
Then The Identity type is active