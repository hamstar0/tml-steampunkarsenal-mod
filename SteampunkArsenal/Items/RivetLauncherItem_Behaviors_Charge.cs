using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal.Items {
	public partial class RivetLauncherItem : ModItem {
		private static void ChargeSteamFromPlayerSteam( Player wielderPlayer, RivetLauncherItem launcherModItem ) {
			var config = SteampunkArsenalConfig.Instance;
			var myplayer = wielderPlayer.GetModPlayer<SteamArsePlayer>();

			float steamAmtPerTick = config.Get<float>( nameof(config.BaseRiveterPressurizationRatePerTick) );

			launcherModItem.SteamSupply.TransferPressureToMeFromSource(
				source: myplayer.AllBoilers,
				steamAmount: steamAmtPerTick,
				waterOverflow: out float waterOverflow
			);

			if( waterOverflow > 0f ) {
				myplayer.AllBoilers.AddWater( waterOverflow, myplayer.AllBoilers.WaterTemperature, out _ );
			}
		}
	}
}