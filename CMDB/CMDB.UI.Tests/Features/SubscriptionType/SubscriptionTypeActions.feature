Feature: SubscriptionTypeActions

Scenario: I want to deactivate an existing subscriptiontype
Given There is a subscription type existing
When I deactivate the subscriptiontype with test
Then the subscriptiontype is deactivated

Scenario: I want to activate an inactive subscriptiontype
Given There is an inactive subscriptiontype existing
When  I activate the subscriptiontype
Then the subscriptiontype is activated