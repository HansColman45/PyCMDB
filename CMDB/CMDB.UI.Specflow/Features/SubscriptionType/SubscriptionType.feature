Feature: SubscriptionType
	I want to be able to create, update and delete a subscription type

Scenario: I want to create a Subscription type
	Given I want to create a Subscriptiontype with the folowing details
		| Field       | Value                         |
		| Category    | Internet Subscription         |
		| Provider    | Orange                        |
		| Type        | Full                          |
		| Description | Completely paid by the person |
	When I save the subscriptiontype
	Then I can find the newly create subscriptiontype back

Scenario Outline: I want to update an existing subscription type
	Given There is a subscription type existing
	When I update the <type> and change it to <value> and save the subscriptiontype
	Then I can see the changes in the subscription type

	Examples: 
		| type        | value           |
		| provider    | proximus        |
		| type        | 25              |
		| description | only 20 procent |

Scenario: I want to deactivate an existing subscriptiontype
	Given There is a subscription type existing
	When I deactivate the subscriptiontype with test
	Then The subscriptiontype is deactivated

Scenario: I want to activate an inactive subscriptiontype
	Given There is an inactive subscriptiontype existing
	When  I activate the subscriptiontype
	Then The subscriptiontype is activated