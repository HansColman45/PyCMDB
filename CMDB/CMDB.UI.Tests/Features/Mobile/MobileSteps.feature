Feature: MobileActions

Scenario: I want to deactivate an existing Mobile
Given There is a mobile existing in the system
When I deactivate the mobile with reason Test
Then The mobile is deactivated

Scenario: I want to activate an inactiva Mobile
Given There is an inactive mobile existing in the system
When I activate the mobile
Then The mobile is actice again

Scenario: I want to assign an Identity to an existing Mobile
Given There is an inactive mobile existing in the system
And an Identity exist as well
When I assign the identity to my mobile
And I fill in the assign form for my mobile
Then The identity is assigned to my mobile