Feature: CreateMonitor
	As a user I want to create a monitor
	So it can be used

Scenario: I want to create a new monitor
Given I want to create a monitor with the folowing details
	| Field        | Value     |
	| AssetTag     | SRC       |
	| SerialNumber | 123456789 |
	| Type         | HP 17inch |
When I save the monitor
Then The monitor can be found