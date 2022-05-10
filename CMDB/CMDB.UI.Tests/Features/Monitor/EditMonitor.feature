Feature: UpdateMonitor

Scenario Outline: I want to update an existing monitor
	Given There is an monitor existing
	When I update the <Field> with <Value> on my monitor and I save
	Then Then the monitor is saved

	Examples: 
	| Field        | Value      |
	| SerialNumber | 456123     |
	| Type         | HP Generic |
