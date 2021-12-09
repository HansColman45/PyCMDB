Feature: CreateLaptop

Scenario: I want to create a new Laptop
	Given I want to create a new Laptop with these details
		| Field        | Value         |
		| AssetTag     | IND           |
		| SerialNumber | 123456789     |
		| Type         | Dell Latitude |
		| RAM          | 5Gb           |
	When I save the Laptop
	Then I can find the newly created Laptop back