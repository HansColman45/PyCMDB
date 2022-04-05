Feature: EditDockingStation

Scenario Outline: I want to update an existing docking
	Given There is an Docking existing
	When I update the <Field> with <Value> on my Doking and I save
	Then Then The Docking is saved

	Examples: 
	| Field        | Value      |
	| SerialNumber | 456123     |
	| Type         | HP Generic |