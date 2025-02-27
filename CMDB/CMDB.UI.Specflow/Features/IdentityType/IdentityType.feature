Feature: IdentityType
	I want to be able to create, update and delete Identitytypes

Scenario: I want to create a new Identity Type
	Given I want to create an Identity type with these details
	| Field       | Value                                     |
	| Type        | Stagair                                   |
	| Description | person that is working for a short period |
	When I save the Identity type
	Then I can find the newly create Identity type back

Scenario Outline: I want to edit an existing Identity type
	Given There is an Identity type existing
	When I change the <Field> to <Value> and I save the Identity type
	Then The Identity type is changed and the new values are visable

	Examples: 
	| Field       | Value                      |
	| Type        | Alien                      |
	| Description | Person not from this wolrd |

Scenario: I want to deactivate an Identitytype
	Given There is an Identity type existing
	When I want to deactivate the identity type with reason Test
	Then The Identity type is deactivated

Scenario: I want to activate an inactive identitity type
	Given There is an inactive Identitytype existing
	When I want to activate the Idenity type
	Then The Identity type is active