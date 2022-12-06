Feature: CreateSubscription

	Scenario: I want to create a subscription
	Given I want to create a Subscription with the following details
	| Field       | Value |
	| Type        | Full  |
	| Phonenumber | 123   |
	When I save the subscription
	Then I can find the newly create subscription back