Feature: DockingActions

Scenario: 1 I want to deactivate a existing active Docking station
Given There is an active Docking existing
When I deactivate the Docking with reason Test
Then The Docking is deactivated

Scenario: 2 I want to activate an existing inactive docking station
Given There is an inactve Docking existing
When I activate the docking station
Then The docking station is activated