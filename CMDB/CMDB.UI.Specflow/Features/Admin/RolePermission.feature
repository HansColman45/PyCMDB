Feature: RolePermission
	I want to be able to create, edit and delete the Rolepermissions

Scenario: I want to create a RolePermission
	Given I want to create a RolePermission as
		| Field      | Value     |
		| Menu       | Test Menu |
		| Permission | Add       |
		| Level      | 1         |
	When I save the RolePermission
	Then I should be able to find the created RolePermission in the system

Scenario: I want to edit a RolePermission
	Given There is a RolePermission existing in the system
	When I change the Level to 1 and update the RolePermission
	Then I should be able to find the updated RolePermission in the system