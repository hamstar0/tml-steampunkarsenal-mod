using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Items;


namespace SteampunkArsenal {
	class SteamArsePlayer : ModPlayer, ISteamPressureSource {
		public override bool PreItemCheck() {
			Item item = this.player.HeldItem;
			if( item?.active == true && item.type == ModContent.ItemType<SteamPoweredRivetLauncherItem>() ) {
				SteamPoweredRivetLauncherItem.RunHeldBehavior( this.player, item );
			}

			return base.PreItemCheck();
		}


		////////////////

		public void AddPressurePercent( float amount ) {
		}

		public float GetPressurePercent() {
			return 0f;
		}

		public float TransferPressureToMeFromSource( ISteamPressureSource source, float amount ) {
			return amount;
		}


		////////////////

		internal void ApplySteamBackfire_Syncs( float steamAmount ) {
			f
		}
	}
}