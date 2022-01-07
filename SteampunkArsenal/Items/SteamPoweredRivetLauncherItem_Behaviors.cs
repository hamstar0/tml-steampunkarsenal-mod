using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Net;


namespace SteampunkArsenal.Items {
	public partial class SteamPoweredRivetLauncherItem : ModItem {
		internal static void RunHeldBehavior_Local_If( Player wielderPlayer, Item launcherItem ) {
			if( wielderPlayer.whoAmI != Main.myPlayer ) {
				return;
			}

			var myitem = launcherItem.modItem as SteamPoweredRivetLauncherItem;

			//

			if( Main.mouseRight ) {
				var config = SteampunkArsenalConfig.Instance;
				var myplayer = wielderPlayer.GetModPlayer<SteamArsePlayer>();

				float steamAmtPerTick = config.Get<float>( nameof(config.BaseRiveterPressurizationRatePerTick) );

				myitem.MyBoiler.TransferPressureToMeFromSource(
					source: myplayer.AllBoilers,
					pressureAmount: steamAmtPerTick,
					waterOverflow: out float waterOverflow
				);

				if( waterOverflow > 0f ) {
					myplayer.AllBoilers.AddWater( waterOverflow, myplayer.AllBoilers.WaterTemperature, out _ );
				}
			}

			//

			myitem.CheckPressure_Local( wielderPlayer );
		}



		////////////////
		
		internal void ApplyLaunchedRivetStats_NonServer_Syncs( int projectileIdx, Projectile projectile ) {
			if( Main.netMode == NetmodeID.Server ) {
				return;
			}

			float pressure = this.MyBoiler.SteamPressure;

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

			if( this.MyBoiler.SteamPressure >= 100f ) {
				var myplayer = wielderPlayer.GetModPlayer<SteamArsePlayer>();

				myplayer.ApplySteamDamage_Local_Syncs( this.MyBoiler.SteamPressure );

				//

				this.MyBoiler.AddWater( -this.MyBoiler.Water, 1f, out _ );

				pressureChanged = true;
			}

			return pressureChanged;
		}
	}
}