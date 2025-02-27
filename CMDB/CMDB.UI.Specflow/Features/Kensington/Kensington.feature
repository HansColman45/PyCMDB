Feature: Kensington
	I want to be able to create, update, dactivate and activate a Kensington

Scenario: I want to create a Kensington
	Given I have a Kensington with these detials
		| Field        | Value             |
		| SerialNumber | 123456            |
		| AmountOfKeys | 1                 |
		| Type         | Kensington Orange |
	When I save the Kensington
	Then I can find the newly create Kensington back

Scenario Outline: I want to update a exising Kensington
	Given There is an Kensington existing in the system
	When I update the <Field> and change it to <Value> and save the Kensington
	Then I can find the updated Kensington back

Examples:
	| Field        | Value |
	| SerialNumber | 19645 |
	| AmountOfKeys | 2     |
	