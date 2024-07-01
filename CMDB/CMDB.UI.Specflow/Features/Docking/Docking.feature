Feature: Docking
	I want to create, update and delete a Docking station

Scenario: I want to create a Dockingstaion
	Given I want to create a new Dockingstation with these details
	| Field        | Value      |
	| AssetTag     | DOC        |
	| SerialNumber | 987654     |
	| Type         | HP Generic |
	When I save the Dockingstion
	Then I can find the newly created Docking station

Scenario Outline: I want to update an existing docking
	Given There is an Docking existing
	When I update the <Field> with <Value> on my Doking and I save
	Then Then The Docking is saved

	Examples: 
	| Field        | Value      |
	| SerialNumber | 456123     |
	| Type         | HP Generic |