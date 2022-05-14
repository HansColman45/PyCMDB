Feature: UpdateToken

Scenario Outline: I want to update an existing token
	Given There is an token existing
	When I update the <Field> with <Value> on my token and I save
	Then Then the token is saved with the new value

	Examples: 
	| Field        | Value       |
	| SerialNumber | 456123      |
	| Type         | HID Generic |
