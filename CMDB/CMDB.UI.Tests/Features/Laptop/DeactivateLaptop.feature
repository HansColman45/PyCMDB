Feature: DeactivateLaptop

Scenario: I want to deactivate an existing Laptop
	Given There is an active Laptop existing
	When I deactivate the Laptop with reason Test
	Then The laptop is deactivated