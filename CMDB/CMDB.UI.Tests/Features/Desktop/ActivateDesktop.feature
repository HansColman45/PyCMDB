Feature: ActivateDesktop

Scenario: I want to activate and inactive Desktop
	Given There is an inactive Desktop existing
	When I activate the Desktop
	Then The desktop is active