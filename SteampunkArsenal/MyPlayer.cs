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
		internal ConvergentBoiler MyBoiler { get; private set; } = new ConvergentBoiler();



		////////////////

		public override void PreUpdate() {
			foreach( Item item in this.player.inventory ) {
				Boiler boiler = Boiler.GetBoilerForItem( item );
				if( boiler?.IsActive ?? false ) {
					boiler?.Update( this.player );
				}
			}

			//

			if( this.MyBoiler.IsActive ) {
				this.UpdateBoiler();
			}

			if( SteampunkArsenalConfig.Instance.DebugModeInfo && this.player.whoAmI == Main.myPlayer ) {
				DebugLibraries.Print(
					"my boiler",
					"Water: " + this.MyBoiler.Water.ToString( "N2" )
						+ ", Heat: " + this.MyBoiler.WaterTemperature.ToString( "N2" )
						+ ", Steam: " + this.MyBoiler.SteamPressure.ToString( "N2" )
						+ ", Gun: " + SteamSource.GetSteamSourceForItem( this.player.HeldItem )?.SteamPressure.ToString( "N2" )
				);
			}
		}


		////////////////
		
		public override bool PreItemCheck() {
			Item item = this.player.HeldItem;
			if( item?.active == true && item.type == ModContent.ItemType<SteamPoweredRivetLauncherItem>() ) {
				SteamPoweredRivetLauncherItem.RunHeldBehavior_Local_If( this.player, item );
			}

			return base.PreItemCheck();
		}
	}
}