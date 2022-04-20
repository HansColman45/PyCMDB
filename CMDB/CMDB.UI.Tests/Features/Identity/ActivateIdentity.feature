Feature: ActivateIdentity
	
Scenario: I want to activate an inactive Identity
	Given An inactive Identity exisist in the system
	When I want to activate this identity
	Then The Identity is active