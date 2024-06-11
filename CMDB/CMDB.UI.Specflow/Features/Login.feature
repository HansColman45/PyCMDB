Feature: Login
	I can logon

	Scenario: I can login with a valid username and password
		Given I open the home page
		When I logon with a valid user name and password
		Then I can logon