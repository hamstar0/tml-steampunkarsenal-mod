using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Net;


namespace SteampunkArsenal.Items {
	public partial class SteamPoweredRivetLauncherItem : ModItem {
		internal static void RunHeldBehavior_Local( Player wielderPlayer, Item launcherItem ) {
			if( wielderPlayer.whoAmI != Main.myPlayer ) {
				return;
			}

			var myitem = launcherItem.modItem as SteamPoweredRivetLauncherItem;

			if( Main.mouseRight ) {
				var config = SteampunkArsenalConfig.Instance;

				float tickAmt = config.Get<float>( nameof(config.BaseRiveterPressurizationRatePerTick) );

				myitem.Boiler.TransferPressureToMeFromSource(
					wielderPlayer.GetModPlayer<SteamArsePlayer>().Boiler,
					tickAmt
				);
			}

			//

			myitem.CheckPressure_Local( wielderPlayer );
		}



		////////////////
		
		internal void ApplyLaunchedRivetStats_NonServer_Syncs( int projectileIdx, Projectile projectile ) {
			if( Main.netMode == NetmodeID.Server ) {
				return;
			}

			float pressure = this.Boiler.SteamPressure;

			projectile.damage = (int)pressure;

			//
			
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				ProjectileDamageSyncProtocol.BroadcastFromClientToAll( projectileIdx, (int)pressure );
			}
		}


		////////////////

		private bool CheckPressure_Local( Player wielderPlayer ) {
			if( wielderPlayer.whoAmI != Main.myPlayer ) {
				return false;
			}

			//

			bool pressureChanged = false;

			if( this.Boiler.SteamPressure >= 100f ) {
				var myplayer = wielderPlayer.GetModPlayer<SteamArsePlayer>();

				myplayer.ApplySteamDamage_Local_Syncs( this.Boiler.SteamPressure );

				//

				this.Boiler.AddBoilerWater( -this.Boiler.BoilerWater, 1f );

				pressureChanged = true;
			}

			return pressureChanged;
		}
	}
}