Feature: UpdateIdentityType

Scenario Outline: I want to edit an existing Identity type
Given There is an Identity type existing
When I change the <Field> to <Value> and I save the Identity type
Then The Identity type is changed and the new values are visable

Examples: 
| Field       | Value                      |
| Type        | Alien                      |
| Description | Person not from this wolrd |