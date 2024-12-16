Feature: Mobile
	I want to create or edit and update a Mobile

Scenario: I want to create a new mobile
	Given  I want to create a new Mobile with these details
		| Field | Value      |
		| Type  | IPhone X10 |
		| IMEI  | 9          |
	When I save the mobile
	Then I can find the newly created Mobile back

Scenario Outline: I want to update an existing Mobile
	Given There is a mobile existing in the system
	When I change the <Field> to <Value> and I save the changes for my mobile
	Then The changes in mobile are saved

	Examples: 
	| Field | Value    |
	| IMEI  | 8        |
	| Type  | IPhone S |

Scenario: I want to deactivate an existing Mobile
	Given There is a mobile existing in the system
	When I deactivate the mobile with reason Test
	Then The mobile is deactivated

Scenario: I want to activate an inactive Mobile
	Given There is an inactive mobile existing in the system
	When I activate the mobile
	Then The mobile is actice again

Scenario: I want to assign an Idenitity to an existing Mobile
	Given There is an active mobile in the system
	And The Identity to assign to my mobile exists as well
	When I want to assign that Identity to my mobile
	And I fill in thee assign form
	Then The identity is assigned to my mobile

Scenario: I want to release an Idenitity from an existing Mobile
	Given There is an active mobile in the system
	And The Identity to assign to my mobile exists as well
	And The Identity is assigned to my mobile
	When I release the Identity from my mobile
	Then The identity is released from my mobile