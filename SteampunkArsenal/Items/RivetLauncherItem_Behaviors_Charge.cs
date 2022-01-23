using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal.Items {
	public partial class RivetLauncherItem : ModItem {
		private static bool ChargeSteamFromPlayerSteam(
					Player wielderPlayer,
					RivetLauncherItem launcherModItem,
					out float availableSteam ) {
			var config = SteampunkArsenalConfig.Instance;
			var myplayer = wielderPlayer.GetModPlayer<SteamArsePlayer>();

			float steamAmtPerTick = config.Get<float>( nameof(config.BaseRiveterPressurizationRatePerTick) );

			float transferredSteam = launcherModItem.SteamSupply.TransferSteamToMeFromSource_If(
				source: myplayer.ImplicitConvergingBoiler,
				intendedSteamXferAmt: steamAmtPerTick,
				waterUnderflow: out _,
				waterOverflow: out float waterOverflow
			);

			if( waterOverflow > 0f ) {
				myplayer.ImplicitConvergingBoiler.AddWater_If(
					waterOverflow,
					myplayer.ImplicitConvergingBoiler.WaterHeat,
					out _
				);
			}

			availableSteam = myplayer.ImplicitConvergingBoiler.SteamPressure;
			return transferredSteam > 0f;
		}
	}
}