Feature: EditAssetType

Scenario: I want to edit an existing Asset Type
	Given There is an AssetType existing
	When I change the Type and save the changes
	Then The changes are saved