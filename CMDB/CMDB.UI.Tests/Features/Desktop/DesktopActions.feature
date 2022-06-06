Feature: DeactivateDesktop

Scenario: I want to activate and inactive Desktop
	Given There is an inactive Desktop existing
	When I activate the Desktop
	Then The desktop is active

Scenario: I want to deactivate an existing Desktop
	Given There is an active Desktop existing
	When I deactivate the Desktop with reason Test
	Then The desktop is deactivated

Scenario: I want to assign an existing Identiy to my Desktop
	Given There is an Desktop existing
	And an Identity exist as well
	When I assign the Desktop to the Identity
	And I fill in the assign form for my Desktop
	Then The Identity is assigned to the Desktop

Scenario: I want to release an assigned identity from my Desktop
	Given There is an Desktop existing
	And an Identity exist as well
	And that Identity is assigned to my Desktop
	When I release that identity from my Desktop
	And I fill in the release form for my Desktop
	Then The identity is released from my Desktop