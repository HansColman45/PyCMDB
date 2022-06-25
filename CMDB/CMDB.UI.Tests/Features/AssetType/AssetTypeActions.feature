Feature: AssetTypeActions

Scenario: I want to ativate an existing active AssetType
	Given There is an Inactive AssetType existing
	When I want to activate the assettype
	Then the assettype has been activeted

Scenario: I want to dacativate an existing active AssetType
	Given There is an active AssetType existing
	When I want to deactivate the assettype with reason Test
	Then the assettype has been deactiveted