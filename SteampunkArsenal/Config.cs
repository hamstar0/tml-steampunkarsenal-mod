using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using ModLibsCore.Classes.UI.ModConfig;


namespace SteampunkArsenal {
	class MyFloatInputElement : FloatInputElement { }




	public partial class SteampunkArsenalConfig : ModConfig {
		public static SteampunkArsenalConfig Instance => ModContent.GetInstance<SteampunkArsenalConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		public bool DebugModeInfo { get; set; } = false;



		////////////////

		[Range( -4096f, 4096f )]
		[DefaultValue( -96f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float PressureGaugeInitialHUDPositionX { get; set; } = -96f;

		[Range( -2048, 2048 )]
		[DefaultValue( -128 )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float PressureGaugeInitialHUDPositionY { get; set; } = -128;


		////

		[DefaultValue( true )]
		public bool RivetLauncherRecipeEnabled { get; set; } = true;

		[DefaultValue( true )]
		public bool BoilerOBurdenRecipeEnabled { get; set; } = true;

		[DefaultValue( true )]
		public bool PortABoilerRecipeEnabled { get; set; } = true;

		[DefaultValue( true )]
		public bool SteamBallRecipeEnabled { get; set; } = true;


		////
		
		[Range( 0f, 100f )]
		[DefaultValue( 24f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float BoilerWaterFillRatePerSecond { get; set; } = 24f;

		////

		[Range( 0f, 1f )]
		[DefaultValue( 0.15f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float RiveterSteamPercentChargeUpRatePerSecond { get; set; } = 0.15f;
		
		[Range( 0f, 100f )]
		[DefaultValue( 3.5f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float RiveterDamagerPerPressureUnit { get; set; } = 3.5f;

		////

		[Range( 0f, 10f )]
		[DefaultValue( 0.65f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float RiveterBackfireDamagePerSteamUnit { get; set; } = 0.65f;

		////
		
		[Range( 0f, 100f )]
		[DefaultValue( 100f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float UnpinnableNpcDamagePerSecondBase { get; set; } = 100f;

		////

		[Range( 0f, 100f )]
		[DefaultValue( 0.5f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float RivetSquirmDamageScale { get; set; } = 0.5f;

		[Range( 0f, 10000f )]
		[DefaultValue( 0f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float RivetSquirmDamagePerSecondBase { get; set; } = 0f;


		////

		[Range( 0f, 100f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BoilerWaterHeatXferRatePerSecond { get; set; } = 1f;

		//[Range( 0f, 100f )]
		//[DefaultValue( 0.9f )]
		//[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		//public float BoilerTempXferRateReducedPerSteamPercentPerSecondPerTank { get; set; } = 0.9f;

		[Range( 0f, 1f )]
		[DefaultValue( 0.05f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float WaterHeatPercentDecayRatePerSecond { get; set; } = 0.05f;

		[Range( 0f, 1f )]
		[DefaultValue( 0.01f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BoilerHeatPercentDecayRatePerSecond { get; set; } = 0.01f;

		[Range( 0f, 1f )]
		[DefaultValue( 0.05f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float RiveterPressurePercentDecayRatePerSecond { get; set; } = 0.05f;
	}
}