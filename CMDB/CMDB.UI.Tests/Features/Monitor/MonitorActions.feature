Feature: MonitorActions

Scenario: I want to deactivate an existing active monitor
	Given There is an actives monitor existing 
	When I deactivate the monotor with reason Test
	Then The monitor is deactivated

Scenario: I want to activate an inactive monitor
	Given There is an inactive monitor existing
	When I activate the monitor
	Then The monitor is active

Scenario: I want to assign an existing Identiy to my Laptop
	Given There is an monitor existing
	And an Identy exist as well
	When I assign the montitor to the Identity
	And I fill in the assign form for my montitor
	Then The Identity is assigned to the montitor