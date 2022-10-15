Feature: CreateSubscriptionType

Scenario: I want to create a new Subscriptiontype
	Given I want to create a Subscriptiontype with the folowing details
		| Field       | Value                         |
		| Category    | Internet Subscription         |
		| Provider    | Orange                        |
		| Type        | Full                          |
		| Description | Completely paid by the person |
	When I save the subscriptiontype
	Then I can find the newly create subscriptiontype back
