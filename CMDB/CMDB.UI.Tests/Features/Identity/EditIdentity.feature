Feature: Edit Identity

Scenario Outline: I want to update an existing Identity
	Given An Identity exisist in the system
	When I want to update <field> with <value>
	Then The identity is updated

	Examples:
		| field     | value                     |
		| FirstName | Testje                    |
		| LastName  | CMDBtje                   |
		| Company   | Veepee                    |
		| UserID    | IND851                    |
		| Email     | testje.cmdbtje@veepee.Com |