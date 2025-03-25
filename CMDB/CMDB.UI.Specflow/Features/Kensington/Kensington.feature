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
	Given There is a Kensington existing in the system
	When I update the <Field> and change it to <Value> and save the Kensington
	Then I can find the updated Kensington back

Examples:
	| Field        | Value |
	| SerialNumber | 19645 |
	| AmountOfKeys | 2     |

Scenario: I want to deactivate a Kensington
	Given There is a Kensington existing in the system
	When I deactivate the Kensington with the reason Test
	Then The Kensington is deactivated
	
Scenario: I want to activate a deactivated Kensington
	Given There is an inactive Kensington existing in the system
	When I activate the Kensington
	Then The Kensington is activated

Scenario Outline: I want to link an existing device to my kenington
	Given There is an active Kensington existing in the system
	And a active <Device> existing in the system
	And That <Device> is assiged to an Identity
	When I link the Kensington to that <Device>
	And I fill in the assign form for that device
	Then The Kensington is linked to the device

Examples: 
	| Device  |
	| Laptop  |
	| Desktop |
	| Docking |

Scenario Outline: I want to release a device from my Kensington
	Given There is an active Kensington existing in the system
	And a active <Device> existing in the system
	And That <Device> is assiged to an Identity
	And That <Device> is linked to my key
	When I release the <Device> from the Kensington
	Then The Kensington is released from the device

	Examples: 
	| Device  |
	| Laptop  |
	| Desktop |
	| Docking |