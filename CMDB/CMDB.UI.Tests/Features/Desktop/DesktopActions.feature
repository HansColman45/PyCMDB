Feature: DeactivateDesktop

Scenario: I want to deactivate an existing Desktop
	Given There is an active Desktop existing
	When I deactivate the Desktop with reason Test
	Then The desktop is deactivated