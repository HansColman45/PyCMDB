Feature: ActivateAssetType

Scenario: I want to ativate an existing active AssetType
	Given There is an Inactive AssetType existing
	When I want to activate the assettype
	Then the assettype has been activeted