Feature: DockingActions

Scenario: I want to activate an existing inactive docking station
	Given There is an inactve Docking existing
	When I activate the docking station
	Then The docking station is activated

Scenario: I want to deactivate a existing active Docking station
	Given There is an active Docking existing
	When I deactivate the Docking with reason Test
	Then The Docking is deactivated

Scenario: I want to assign an existing Identiy to my Docking
	Given There is an Docking existing
	And an Identity exist as well
	When I assign the Docking to the Identity
	And I fill in the assign form for my Docking
	Then The Identity is assigned to the Docking

Scenario: I want to release an assigned identity from my Docking
	Given There is an Docking existing
	And an Identity exist as well
	And that Identity is assigned to my Docking
	When I release that identity from my Docking
	And I fill in the release form for my Docking
	Then The identity is released from my Docking