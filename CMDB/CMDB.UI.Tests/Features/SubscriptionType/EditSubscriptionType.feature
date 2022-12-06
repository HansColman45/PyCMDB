Feature: EditSubscriptionType

Scenario Outline: I want to update an existing subscription type
Given There is a subscription type existing
When I update the <type> and change it to <value> and save the subscriptiontype
Then I can see the changes in the subscription type

Examples: 
	| type        | value           |
	| provider    | proximus        |
	| type        | 25              |
	| description | only 20 procent |