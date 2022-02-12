Feature: CreateDesktop

Scenario: I want to create a new Desktop
	Given I want to create a new Desktop with these details
		| Field        | Value         |
		| AssetTag     | DST           |
		| SerialNumber | 123456789     |
		| Type         | Dell Latitude |
		| RAM          | 5 Gb          |
	When I save the Desktop
	Then I can find the newly created Desktop back