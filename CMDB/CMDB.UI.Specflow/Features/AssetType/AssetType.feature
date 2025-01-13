Feature: AssetType
	I want to be able to create, update and delete an AssetType

Scenario Outline: I want to create an assettype
	Given I want to create a new <Category> with <Vendor> and <Type>
	When I create that <Category>
	Then The <Category> is created

	Examples:
		| Category        | Vendor     | Type           |
		| Kensington      | Kensington | Black          |
		| Mobile          | Motorola   | G5             |
		| Laptop          | Dell       | Latitude E5510 |
		| Desktop         | HP         | Pavilion       |
		| Token           | HID        | Flat           |
		| Monitor         | Philips    | LED            |
		| Docking station | HP         | Dockingstation |

Scenario: I want to edit an existing Asset Type
	Given There is an AssetType existing
	When I change the Type and save the changes
	Then The changes are saved

Scenario: I want to ativate an existing active AssetType
	Given There is an Inactive AssetType existing
	When I want to activate the assettype
	Then the assettype has been activeted

Scenario: I want to dacativate an existing active AssetType
	Given There is an active AssetType existing
	When I want to deactivate the assettype with reason Test
	Then the assettype has been deactiveted