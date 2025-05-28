Feature: Admin
	I want to be able to create, update, deactivate and activate a new Admin

Scenario: I want to create a new admin
	Given There is a account existing in the system
	When I create the new admin for that account with level 8
	Then The newly created amin exists in the system

Scenario: I want to update an existing admin
	Given There is a admin existing in the system
	When I change the admin's level to 1 and update it
	Then The updated admin exists in the system

Scenario: I want to deactivate an existing admin
	Given There is a admin existing in the system
	When I deactivate the admin with the reason Test
	Then The admin is deactivated

Scenario: I want to activate an inactive admin
	Given There is a deactivated admin existing in the system
	When I activate the admin
	Then The admin is activated