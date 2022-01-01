using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Items;


namespace SteampunkArsenal {
	partial class SteamArsePlayer : ModPlayer {
		internal ConvergentBoiler MyBoiler { get; private set; } = new ConvergentBoiler();



		////////////////

		public override void PreUpdate() {
			this.MyBoiler.RefreshConnectedBoilers( this.player );
			
			//

			foreach( Item item in this.player.inventory ) {
				Boiler boiler = Boiler.GetBoilerForItem( item );
				if( boiler?.IsActive ?? false ) {
					boiler?.Update();
				}
			}

			//

			if( this.MyBoiler.IsActive ) {
				this.UpdateBoiler();
			}
DebugLibraries.Print(
	"boiler",
	"Water: "+this.MyBoiler.Water.ToString("N2")
		+", Heat: "+this.MyBoiler.Heat.ToString("N2")
		+", Steam: "+this.MyBoiler.SteamPressure.ToString("N2")
		+", Gun: "+Boiler.GetBoilerForItem(this.player.HeldItem)?.SteamPressure.ToString("N2")
);
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