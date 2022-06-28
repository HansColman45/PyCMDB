Feature: EditMobile

Scenario Outline: I want to update an existing Mobile
Given There is a mobile existing in the system
When I change the <Field> to <Value> and I save the changes for my mobile
Then The changes in mobile are saved

Examples: 
| Field | Value    |
| IMEI  | 8        |
| Type  | IPhone S |