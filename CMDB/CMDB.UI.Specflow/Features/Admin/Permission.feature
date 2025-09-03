Feature: Permission
	I want to Create, Edit and delete permissions in the system

Scenario: I want to create a new permission
	Given I want to create a permission with the following details
			| Field       | Value               |
			| Right       | Test                |
			| Description | This is a test perm |
	When I create the permission
	Then I can find the newly created permission in the system

Scenario Outline: I want to edit an existing permission
	Given There is a permission existing in the system
	When I change the <Field> to <Value> for my permission and save
	Then I can see the changes done to my permission

Examples: 
	| Field       | Value                   |
	| Right       | Updated Test            |
	| Description | This is an updated perm |

Scenario: I want to delete an existing permission
	Given There is a permission existing in the system
	When I delete the permission
	Then I can no longer find the permission in the system