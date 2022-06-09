Feature: DeactivatedAssetType

Scenario: I want to dacativate an existing active AssetType
	Given There is an active AssetType existing
	When I want to deactivate the assettype with reason Test
	Then the assettype has been deactiveted