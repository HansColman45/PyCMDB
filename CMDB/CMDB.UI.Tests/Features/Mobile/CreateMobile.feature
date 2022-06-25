Feature: CreateMobile

Scenario: I want to create a new mobile
	Given  I want to create a new Mobile with these details
		| Field | Value      |
		| Type  | IPhone X10 |
		| IMEI  | 987654321  |
	When I save the mobile
	Then I can find the newly created Mobile back

