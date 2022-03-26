Feature: CreateDocking

Scenario: I want to create a Dockingstaion
Given I want to create a new Dockingstation with these details
| Field        | Value      |
| AssetTag     | DOC        |
| SerialNumber | 987654     |
| Type         | HP Generic |
When I save the Dockingstion
Then I can find the newly created Docking station