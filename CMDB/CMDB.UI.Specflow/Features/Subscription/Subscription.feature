Feature: Subscription
	I want to create, edit and delete a subscription

Scenario: I want to create a subscription
	Given I want to create a Subscription with the following details
		| Field       | Value |
		| Type        | Full  |
		| Phonenumber | 123   |
	When I save the subscription
	Then I can find the newly create subscription back

Scenario: I want to update an existing subscription
	Given There is an subscription existing in the system
	When I update the phonenumber and save the changes
	Then I can see the changes in the subscription

Scenario: I want to deactivate an exisiting subscription
	Given There is an subscription existing in the system
	When I deactivate the subscription with test
	Then The subscription is deactivated

Scenario: I want to activate an inactive subscription
	Given There is an inactive subscription in the system
	When I activate the subscription
	Then The subscription is active