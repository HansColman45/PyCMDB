Feature: AccountTypeActions

Scenario: I want to deactivate an existing accountType
Given There is an accounttype existing in the system
When I deactivate the accountType with reason Test
Then The accountType is deacticated

Scenario: I want to acticate an inactive accountType
Given  There is an inactive accountType existing in the system
When I activate the accountType
Then The accountType is active
