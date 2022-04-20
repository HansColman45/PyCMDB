Feature: ActivateDocking

Scenario: I want to activate an existing inactive docking station
	Given There is an inactve Docking existing
	When I activate the docking station
	Then The docking station is activated