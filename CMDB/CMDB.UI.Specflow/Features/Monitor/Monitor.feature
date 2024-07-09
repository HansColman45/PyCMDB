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