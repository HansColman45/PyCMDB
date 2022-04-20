Feature: DeactivateDocking

Scenario: I want to deactivate a existing active Docking station
	Given There is an active Docking existing
	When I deactivate the Docking with reason Test
	Then The Docking is deactivated