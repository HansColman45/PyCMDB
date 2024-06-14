Feature: Identity
	I want to be able to create, edit an Identity

	Scenario: I want to create a new Identity
	Given I want to create an Identity with these details
		| Field     | Value                  |
		| FirstName | Test                   |
		| LastName  | CMDB                   |
		| Type      | Werknemer              |
		| Company   | Brightest              |
		| UserId    | Test                   |
		| Language  | NL                     |
		| Email     | Test.CMDB@brightest.be |
	When I save
	Then I can find the newly created Identity back