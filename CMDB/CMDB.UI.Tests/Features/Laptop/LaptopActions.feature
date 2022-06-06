Feature: LaptopActions

Scenario: I want to activate and inactive Laptop
	Given There is an inactive Laptop existing
	When I activate the Laptop
	Then The laptop is active

Scenario: I want to deactivate an existing Laptop
	Given There is an active Laptop existing
	When I deactivate the Laptop with reason Test
	Then The laptop is deactivated

Scenario: I want to assign an existing Identiy to my Laptop
	Given There is an Laptop existing
	And an Identity exist as well
	When I assign the Laptop to the Identity
	And I fill in the assign form for my Laptop
	Then The Identity is assigned to the Laptop

Scenario: I want to release an assigned identity from my laptop
	Given There is an Laptop existing
	And an Identity exist as well
	And that Identity is assigned to my laptop
	When I release that identity
	And I fill in the release form for my laptop
	Then The identity is released from my laptop