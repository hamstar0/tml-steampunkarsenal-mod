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

		[DefaultValue( true )]
		public bool RivetLauncherRecipeEnabled { get; set; } = true;

		////

		[Range( 0f, 100f )]
		[DefaultValue( 2f / 3f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float BaseRiveterPressurizationRatePerTick { get; set; } = 2f / 3f;

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