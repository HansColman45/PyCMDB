Feature: MobileActions

Scenario: I want to deactivate an existing Mobile
Given There is a mobile existing in the system
When I deactivate the mobile with reason Test
Then The mobile is deactivated

Scenario: I want to activate an inactiva Mobile
Given There is an inactive mobile existing in the system
When I activate the mobile
Then The mobile is actice again