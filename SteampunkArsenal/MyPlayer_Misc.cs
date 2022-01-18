using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Items;
using SteampunkArsenal.Logic.Steam.SteamSources;


namespace SteampunkArsenal {
	partial class SteamArsePlayer : ModPlayer {
		public SteamContainer GetHeldRivetLauncherSteam() {
			Item item = this.player.HeldItem;

			if( item?.active == true ) {
				if( item.type == ModContent.ItemType<RivetLauncherItem>() ) {
					return (item.modItem as RivetLauncherItem).SteamSupply;
				}
			}

			return null;
		}
	}
}