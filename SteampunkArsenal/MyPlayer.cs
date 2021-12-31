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
/*Item heldItem = this.player.HeldItem;
var myitem = heldItem.modItem as SteamPoweredRivetLauncherItem;
DebugLibraries.Print( "steam", this.Boiler.SteamPressure.ToString("N2")+", "+myitem?.Boiler.SteamPressure.ToString("N2") );

this.Boiler.AddWater( 1f, 1f );*/
			
			foreach( Item item in this.player.inventory ) {
				Boiler boiler = Boiler.GetBoilerForItem( item );
				boiler?.Update();
			}

			if( this.player.wet && !this.player.honeyWet && !this.player.lavaWet ) {
				this.MyBoiler.AddWater( 1f, 1f );
			}

			this.MyBoiler.Update();
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