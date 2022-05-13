Feature: MonitorActions

Scenario: I want to deactivate an existing active monitor
	Given There is an actives monitor existing 
	When I deactivate the monotor with reason Test
	Then The monitor is deactivated

Scenario: I want to activate an inactive monitor
	Given There is an inactive monitor existing
	When I activate the monitor
	Then The monitor is active

