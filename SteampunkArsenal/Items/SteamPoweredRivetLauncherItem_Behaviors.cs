using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Net;

namespace SteampunkArsenal.Items {
	public partial class SteamPoweredRivetLauncherItem : ModItem, ISteamPressureSource {
		internal static void RunHeldBehavior( Player wielderPlayer, Item launcherItem ) {
			if( wielderPlayer.whoAmI == Main.myPlayer ) {
				SteamPoweredRivetLauncherItem.RunHeldBehavior_Local( wielderPlayer, launcherItem );
			}
		}
		
		private static void RunHeldBehavior_Local( Player wielderPlayer, Item launcherItem ) {
			var myitem = launcherItem.modItem as SteamPoweredRivetLauncherItem;

			if( Main.mouseRight ) {
				var config = SteampunkArsenalConfig.Instance;

				float tickAmt = config.Get<float>( nameof(config.BaseRiveterPressurizationRatePerTick) );

				myitem.TransferPressureToMeFromSource( wielderPlayer.GetModPlayer<SteamArsePlayer>(), tickAmt );
			}
		}



		////////////////
		
		internal void ApplyLaunchedRivetStats_Local_Syncs( int projectileIdx, Projectile projectile ) {
			float pressure = this.GetPressurePercent();

			projectile.damage = (int)pressure;

			//

			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				ProjectileDamageSyncProtocol.BroadcastFromClientToAll( projectileIdx, (int)pressure );
			}
		}


		////////////////

		private bool CheckPressure( Player wielderPlayer ) {
			bool pressureChanged = false;

			if( this.SteamPressure >= 100f ) {
				var myplayer = wielderPlayer.GetModPlayer<SteamArsePlayer>();

				myplayer.ApplySteamBackfire_Syncs( this.SteamPressure );

				this.SteamPressure = 0f;

				pressureChanged = true;
			}

			return pressureChanged;
		}
	}
}