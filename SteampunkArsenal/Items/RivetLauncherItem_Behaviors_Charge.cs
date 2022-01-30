using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal.Items {
	public partial class RivetLauncherItem : ModItem {
		private static bool ChargeSteamFromPlayerSteam(
					Player wielderPlayer,
					RivetLauncherItem launcherModItem ) {
			var config = SteampunkArsenalConfig.Instance;
			var myplayer = wielderPlayer.GetModPlayer<SteamArsePlayer>();

			float steamAmtPerSec = config.Get<float>( nameof(config.RiveterChargeUpRatePerSecond) );
			float steamAmtPerTick = steamAmtPerSec / 60f;

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

			return transferredSteam > 0.0001f;	// floating point shenanigans?
		}
	}
}