Feature: DeactivateDesktop

Scenario: 1 I want to deactivate an existing Desktop
	Given There is an active Desktop existing
	When I deactivate the Desktop with reason Test
	Then The desktop is deactivated

Scenario: 2 I want to activate and inactive Desktop
	Given There is an inactive Desktop existing
	When I activate the Desktop
	Then The desktop is active