Feature: Desktop
	I want to create, edit and Delete a Desktop

Scenario: I want to create a new Desktop
	Given I want to create a new Desktop with these details
		| Field        | Value         |
		| AssetTag     | DST           |
		| SerialNumber | 123456789     |
		| Type         | Dell Latitude |
		| RAM          | 5 Gb          |
	When I save the Desktop
	Then I can find the newly created Desktop back

Scenario Outline: I want to update an existing Desktop
	Given There is an Desktop existing
	When I update the <field> with <Value> on my Desktop and I save
	Then The Desktop is saved

	Examples:
		| field        | Value     |
		| Serialnumber | 987654321 |
		| RAM          | 4 Gb      |

Scenario: I want to activate and inactive Desktop
	Given There is an inactive Desktop existing
	When I activate the Desktop
	Then The desktop is active

Scenario: I want to deactivate an existing Desktop
	Given There is an active Desktop existing
	When I deactivate the Desktop with reason Test
	Then The desktop is deactivated