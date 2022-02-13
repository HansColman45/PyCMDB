Feature: EditDesktop

Scenario Outline: I want to update an existing Desktop
	Given There is an Desktop existing
	When I update the <field> with <Value> on my Desktop and I save
	Then The Desktop is saved

	Examples:
		| field        | Value     |
		| Serialnumber | 987654321 |
		| RAM          | 4 Gb      |