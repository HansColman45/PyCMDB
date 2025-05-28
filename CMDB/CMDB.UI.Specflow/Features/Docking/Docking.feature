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

Scenario: I want to activate an existing inactive docking station
	Given There is an inactve Docking existing
	When I activate the docking station
	Then The docking station is activated

Scenario: I want to deactivate a existing active Docking station
	Given There is an Docking existing
	When I deactivate the Docking with reason Test
	Then The Docking is deactivated

Scenario: I want to assign an Identity to my docking station
	Given There is an active Docking existing
	And The Identity exist as well
	When I assign the Docking to the Identity
	And I fill in the assign form for my Docking
	Then The Identity is assigned to the Docking

Scenario: I want to release an assigned identity from my Docking
	Given There is an active Docking existing
	And The Identity exist as well
	And that Identity is assigned to my Docking
	When I release the Identity from the Docking and I have filled in the release form
	Then The Identity is released from the Docking

Scenario: I want to assign an existing Key to my Docking
	Given There is an active Docking existing
	And The Identity to assign to my docking is existing
	And that Identity is assigned to my Docking
	And A Key to assign to my Docking is existing in the system
	When I assign the Key to the Docking
	And I fill in the assign form for my Docking
	Then The Key is assigned to the Docking

Scenario: I want to relase an assigned Key from my Docking
	Given There is an active Docking existing
	And The Identity to assign to my docking is existing
	And that Identity is assigned to my Docking
	And A Key to assign to my Docking is existing in the system
	And that Key is assigned to my Docking
	When I release the Key from my Docking and I fill in the release form
	Then The Key is released from my Docking