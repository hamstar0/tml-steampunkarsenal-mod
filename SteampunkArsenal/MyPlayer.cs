using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Logic.Steam;
using SteampunkArsenal.Logic.Steam.SteamSources;
using SteampunkArsenal.Logic.Steam.SteamSources.Boilers;
using SteampunkArsenal.Items;


namespace SteampunkArsenal {
	partial class SteamArsePlayer : ModPlayer {
		internal ConvergentBoiler AllBoilers { get; private set; } = new ConvergentBoiler( false );



		////////////////

		public override void PreUpdate() {
			// Update non-convergent boilers in inventory
			foreach( Item item in this.player.inventory ) {
				Boiler boiler = Boiler.GetBoilerForItem( item );

				if( boiler?.IsActive ?? false ) {
					boiler.PreUpdate( this.player, false );
					boiler.PostUpdate( this.player, false );
				}
			}

			//
			
			this.UpdateBoiler();

			if( SteampunkArsenalConfig.Instance.DebugModeInfo && this.player.whoAmI == Main.myPlayer ) {
				DebugLibraries.Print(
					"boilers",
					"Water: " + this.AllBoilers.Water.ToString( "N2" )
						+ ", BTemp: " + this.AllBoilers.BoilerTemperature.ToString( "N2" )
						+ ", WTemp: " + this.AllBoilers.WaterTemperature.ToString( "N2" )
						+ ", Steam: " + this.AllBoilers.SteamPressure.ToString( "N2" )
						+ ", Gun: " + SteamSource.GetSteamSourceForItem( this.player.HeldItem )?.SteamPressure.ToString( "N2" )
				);
			}
		}


		////////////////
		
		public override bool PreItemCheck() {
			Item item = this.player.HeldItem;

			if( item?.active == true ) {
				if( item.type == ModContent.ItemType<RivetLauncherItem>() ) {
					RivetLauncherItem.RunHeldBehavior_Local_If( this.player, item );
				}
			}

			return base.PreItemCheck();
		}
	}
}