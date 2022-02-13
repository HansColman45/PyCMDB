Feature: DeactivateLaptop

Scenario: 1 I want to deactivate an existing Laptop
	Given There is an active Laptop existing
	When I deactivate the Laptop with reason Test
	Then The laptop is deactivated

Scenario: 2 I want to activate and inactive Laptop
	Given There is an inactive Laptop existing
	When I activate the Laptop
	Then The laptop is active