using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Net;


namespace SteampunkArsenal.Items {
	public partial class SteamPoweredRivetLauncherItem : ModItem, ISteamPressureSource {
		internal static void RunHeldBehavior_Local( Player wielderPlayer, Item launcherItem ) {
			if( wielderPlayer.whoAmI != Main.myPlayer ) {
				return;
			}

			var myitem = launcherItem.modItem as SteamPoweredRivetLauncherItem;

			if( Main.mouseRight ) {
				var config = SteampunkArsenalConfig.Instance;

				float tickAmt = config.Get<float>( nameof(config.BaseRiveterPressurizationRatePerTick) );

				myitem.TransferPressureToMeFromSource( wielderPlayer.GetModPlayer<SteamArsePlayer>(), tickAmt );
			}

			//

			myitem.CheckPressure_Local( wielderPlayer );
		}



		////////////////
		
		internal void ApplyLaunchedRivetStats_NonServer_Syncs( int projectileIdx, Projectile projectile ) {
			if( Main.netMode == NetmodeID.Server ) {
				return;
			}

			float pressure = this.GetPressurePercent();

			projectile.damage = (int)pressure;

			//

			ProjectileDamageSyncProtocol.BroadcastFromClientToAll( projectileIdx, (int)pressure );
		}


		////////////////

		private bool CheckPressure_Local( Player wielderPlayer ) {
			if( wielderPlayer.whoAmI != Main.myPlayer ) {
				return false;
			}

			//

			bool pressureChanged = false;

			if( this.SteamPressure >= 100f ) {
				var myplayer = wielderPlayer.GetModPlayer<SteamArsePlayer>();
				float myPressure = this.GetPressurePercent();

				myplayer.ApplySteamDamage_Local_Syncs( myPressure );

				//

				this.AddPressurePercent( -myPressure );

				pressureChanged = true;
			}

			return pressureChanged;
		}
	}
}