Feature: AssetTypes

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