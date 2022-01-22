using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal.Items {
	public partial class RivetLauncherItem : ModItem {
		private static bool ChargeSteamFromPlayerSteam( Player wielderPlayer, RivetLauncherItem launcherModItem ) {
			var config = SteampunkArsenalConfig.Instance;
			var myplayer = wielderPlayer.GetModPlayer<SteamArsePlayer>();

			float steamAmtPerTick = config.Get<float>( nameof(config.BaseRiveterPressurizationRatePerTick) );

			float transferredSteam = launcherModItem.SteamSupply.TransferSteamToMeFromSource(
				source: myplayer.ImplicitConvergingBoiler,
				intendedSteamPressureXferAmt: steamAmtPerTick,
				waterUnderflow: out _,
				waterOverflow: out float waterOverflow
			);

			if( waterOverflow > 0f ) {
				myplayer.ImplicitConvergingBoiler.AddWater(
					waterOverflow,
					myplayer.ImplicitConvergingBoiler.WaterHeat,
					out _
				);
			}

			return transferredSteam > 0f;
		}
	}
}