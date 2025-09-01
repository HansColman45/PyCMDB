Feature: Permission
	I want to Create, Edit and delete permissions in the system

Scenario: I want to create a new permission
	Given I want to create a permission with the following details
			| Field       | Value               |
			| Right       | Test                |
			| Description | This is a test perm |
	When I create the permission
	Then I can find the newly created permission in the system