Feature: EditLapop

Scenario Outline: I want to update an existing Laptop
	Given There is an Laptop existing
	When I update the <field> with <Value> and I save
	Then The Laptop is saved

	Examples:
		| field        | Value     |
		| Serialnumber | 987654321 |
		| RAM          | 4 Gb      |