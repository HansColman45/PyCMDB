Feature: Login

Scenario: I can login with a valid user and password
	Given I open the home page
	When I logon with a valid user and password
	Then I can logon