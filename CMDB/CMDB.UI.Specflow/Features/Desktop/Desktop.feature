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
	Given There is an Desktop existing
	When I deactivate the Desktop with reason Test
	Then The desktop is deactivated

Scenario: I want to assign an existing Identiy to my Desktop
	Given There is an active Desktop existing
	And the Identity exist as well
	When I assign the Desktop to the Identity
	And I fill in the assign form for my Desktop
	Then The Identity is assigned to the Desktop

Scenario: I want to release an assigned identity from my Desktop
	Given There is an active Desktop existing
	And the Identity exist as well
	And that Identity is assigned to my Desktop
	When I release that identity from my Desktop and I fill in the release form
	Then The identity is released from my Desktop

Scenario: I want to assign an existing Key to my Desktop
	Given There is an active Desktop existing
	And the Identity exist as well
	And that Identity is assigned to my Desktop
	And A Key to assign to my desktop is existing in the system
	When I assign the Key to the Desktop
	And I fill in the assign form for my Desktop
	Then The Key is assigned to the Desktop

Scenario: I want to relase an assigned Key from my Desktop
	Given There is an active Desktop existing
	And the Identity exist as well
	And that Identity is assigned to my Desktop
	And A Key to assign to my desktop is existing in the system
	And that Key is assigned to my Desktop
	When I release the Key from my Desktop and I fill in the release form
	Then The Key is released from my Desktop