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

		[Range( -4096f, 4096f )]
		[DefaultValue( -96f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float PressureGaugeHUDPositionX { get; set; } = -96f;

		[Range( -2048, 2048 )]
		[DefaultValue( -128 )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float PressureGaugeHUDPositionY { get; set; } = -128;


		////

		[DefaultValue( true )]
		public bool RivetLauncherRecipeEnabled { get; set; } = true;

		[DefaultValue( true )]
		public bool BoilerOBurdenRecipeEnabled { get; set; } = true;

		[DefaultValue( true )]
		public bool PortABoilerRecipeEnabled { get; set; } = true;

		////
		
		[Range( 0f, 100f )]
		[DefaultValue( 1f / 60f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float BaseRiveterPressurizationRatePerTick { get; set; } = 1f / 60f;

		////
		
		[Range( 0f, 100f )]
		[DefaultValue( 100f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float SquirmUnpinnableNpcDamagePerSecondBase { get; set; } = 100f;

		////

		[Range( 0f, 100f )]
		[DefaultValue( 0.5f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float SquirmRivetDamageScale { get; set; } = 0.5f;

		[Range( 0f, 10000f )]
		[DefaultValue( 0f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float SquirmRivetDamagePerSecondBase { get; set; } = 0f;
	}
}