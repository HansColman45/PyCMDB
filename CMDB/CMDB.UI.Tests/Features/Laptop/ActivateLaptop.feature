Feature: ActivateLaptop

Scenario: I want to activate and inactive Laptop
	Given There is an inactive Laptop existing
	When I activate the Laptop
	Then The laptop is active