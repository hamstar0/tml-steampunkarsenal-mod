using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Net;


namespace SteampunkArsenal.Items {
	public partial class RivetLauncherItem : ModItem {
		internal static void RunHeldBehavior_Local_If( Player wielderPlayer, Item launcherItem ) {
			if( wielderPlayer.whoAmI != Main.myPlayer ) {
				return;
			}

			//

			var myitem = launcherItem.modItem as RivetLauncherItem;

			//

			if( Main.mouseRight ) {
				RivetLauncherItem.ChargeSteamFromPlayerSteam( wielderPlayer, myitem );
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

			if( this.MyBoiler.SteamPressure >= this.MyBoiler.Capacity ) {
				var myplayer = wielderPlayer.GetModPlayer<SteamArsePlayer>();

				myplayer.ApplySteamDamage_Local_Syncs( this.MyBoiler.SteamPressure );

				//

				this.MyBoiler.DrainWater( this.MyBoiler.Water, out _ );

				pressureChanged = true;
			}

			return pressureChanged;
		}
	}
}