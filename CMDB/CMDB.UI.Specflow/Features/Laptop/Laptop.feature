Feature: Laptop
	I want to create, update and delelete a Laptop

Scenario: I want to create a new Laptop
	Given I want to create a new Laptop with these details
		| Field        | Value         |
		| AssetTag     | IND           |
		| SerialNumber | 123456789     |
		| Type         | Dell Latitude |
		| RAM          | 5 Gb          |
	When I save the Laptop
	Then I can find the newly created Laptop back

Scenario Outline: I want to update an existing Laptop
	Given There is an Laptop existing
	When I update the <field> with <Value> on my Laptop and I save
	Then The Laptop is saved

	Examples:
		| field        | Value     |
		| Serialnumber | 987654321 |
		| RAM          | 4 Gb      |

Scenario: I want to deactivate an existing Laptop
	Given There is an Laptop existing
	When I deactivate the Laptop with reason Test
	Then The laptop is deactivated

Scenario: I want to activate and inactive Laptop
	Given There is an inactive Laptop existing
	When I activate the Laptop
	Then The laptop is active