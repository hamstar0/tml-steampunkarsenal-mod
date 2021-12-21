using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Items;


namespace SteampunkArsenal {
	partial class SteamArsePlayer : ModPlayer {
		internal SteamPressureSource Boiler { get; private set; } = new SteamPressureSource();



		////////////////

		public override void PreUpdate() {
Item heldItem = this.player.HeldItem;
var myitem = heldItem.modItem as SteamPoweredRivetLauncherItem;
DebugLibraries.Print( "steam", this.Boiler.SteamPressure.ToString("N2")+", "+myitem?.Boiler.SteamPressure.ToString("N2") );

this.Boiler.AddWater( 1f, 1f );
		}


		////////////////

		public override bool PreItemCheck() {
			Item item = this.player.HeldItem;
			if( item?.active == true && item.type == ModContent.ItemType<SteamPoweredRivetLauncherItem>() ) {
				SteamPoweredRivetLauncherItem.RunHeldBehavior_Local( this.player, item );
			}

			return base.PreItemCheck();
		}
	}
}