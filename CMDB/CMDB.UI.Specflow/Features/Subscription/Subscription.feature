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

Scenario: I want to assign a Identity to my subscripotion
	Given There is an internet subscription existing in the system
	And an identity exist as well
	When I assign the identity to the subscription
	And I fill in the assign form for my subscription
	Then The subscription is assigned to the identity

Scenario: I want to release an Identity from my subscription
	Given There is an internet subscription existing in the system
	And an identity exist as well
	And The internet subsciption is assigend to my subscription
	When I release the identity from my subscription
	Then The Identity is released from the subscription

Scenario: I want to assign a mobile to my subscription
	Given There is a mobile subscription existing in the system
	And There is a mobile existing in the system as well
	And The mobile is asssigned to an idenity
	When I assign the subscription to my mobile
	And I fill in the assign form for my subscription
	Then The mobile is assigned to my subscription

Scenario: I want to release a mobile from my subscription
	Given There is a mobile subscription existing in the system
	And There is a mobile existing in the system as well
	And The mobile is asssigned to an idenity
	And The mobile is assigned to my subscription
	When I release the subcription from my mobile
	Then The mobile is released from the subscription