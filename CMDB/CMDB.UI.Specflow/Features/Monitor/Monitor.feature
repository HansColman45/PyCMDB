Feature: Monitor
	I want to create, update and delete a Monitor

Scenario: I want to create a new monitor
	Given I want to create a monitor with the folowing details
		| Field        | Value     |
		| AssetTag     | SRC       |
		| SerialNumber | 123456789 |
		| Type         | HP 17inch |
	When I save the monitor
	Then The monitor can be found

Scenario Outline: I want to update an existing monitor
	Given There is an monitor existing
	When I update the <Field> with <Value> on my monitor and I save
	Then The monitor is saved

	Examples: 
	| Field        | Value      |
	| SerialNumber | 456123     |
	| Type         | HP Generic |

Scenario: I want to deactivate an existing active monitor
	Given There is an monitor existing 
	When I deactivate the monotor with reason Test
	Then The monitor is deactivated

Scenario: I want to activate an inactive monitor
	Given There is an inactive monitor existing
	When I activate the monitor
	Then The monitor is active

Scenario: I want to assign an existing Identiy to my monitor
	Given There is an active monitor existing
	And The Identity to assign to my monitor is existing
	When I assign the monitor to the Identity
	And I fill in the assign form for my monitor
	Then The Identity is assigned to the monitor

Scenario: I want to release an assigned identity from my monitor
	Given There is an active monitor existing
	And The Identity to assign to my monitor is existing
	And that Identity is assigned to my monitor
	When I release that identity from my monitor and I fill in the release form
	Then The identity is released from my monitor

Scenario: I want to assign an existing Key to my monitor
	Given There is an active monitor existing
	And The Identity to assign to my monitor is existing
	And that Identity is assigned to my monitor
	And A Key to assign to my monitor is existing in the system
	When I assign the Key to the monitor
	And I fill in the assign form for my monitor
	Then The Key is assigned to the monitor

Scenario: I want to relase an assigned Key from my monitor
	Given There is an active monitor existing
	And The Identity to assign to my monitor is existing
	And that Identity is assigned to my monitor
	And A Key to assign to my monitor is existing in the system
	And that Key is assigned to my monitor
	When I release the Key from my monitor and I fill in the release form
	Then The Key is released from my monitor