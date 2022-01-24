using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal.Items {
	public partial class RivetLauncherItem : ModItem {
		internal static void RunHeldBehavior_Local_If( Player wielderPlayer, Item launcherItem ) {
			if( wielderPlayer.whoAmI != Main.myPlayer ) {
				return;
			}

			//

			var myitem = launcherItem.modItem as RivetLauncherItem;
			bool isCharging = false;

			//

			if( Main.mouseRight ) {
				isCharging = RivetLauncherItem.ChargeSteamFromPlayerSteam( wielderPlayer, myitem );
			}

			//
			
			myitem.CheckPressure_Local( wielderPlayer );

			//

			myitem.RunFx( wielderPlayer, isCharging );
		}



		////////////////
		
		private bool CheckPressure_Local( Player wielderPlayer ) {
			if( wielderPlayer.whoAmI != Main.myPlayer ) {
				return false;
			}

			//

			bool pressureChanged = false;

			if( this.SteamSupply.TotalPressure >= this.SteamSupply.TotalCapacity ) {
				var myplayer = wielderPlayer.GetModPlayer<SteamArsePlayer>();

				myplayer.ApplySteamDamage_Local_Syncs( this.SteamSupply.TotalPressure );

				//

				float drainedAmt = this.SteamSupply.DrainWater_If( this.SteamSupply.Water, out _ );

				pressureChanged = drainedAmt > 0f;
			}

			return pressureChanged;
		}
	}
}