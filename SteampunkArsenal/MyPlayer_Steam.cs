using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SteampunkArsenal.Net;
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


		////////////////

		internal void ApplySteamDamage_Local_Syncs( float steamAmount ) {
			if( this.player.whoAmI != Main.myPlayer ) {
				return;
			}

			this.ApplySteamDamage( steamAmount );

			//

			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				SteamDamageProtocol.BroadcastFromClientToAll( this.player.whoAmI, steamAmount );
			}
		}

		internal void ApplySteamDamage( float steamAmount ) {
			this.player.Hurt(
				damageSource: PlayerDeathReason.ByCustomReason(
					this.player.name+" cleared "+(this.player.Male?"his":"her")+" pores" ),
				Damage: (int)steamAmount,
				hitDirection: 0
			);

			//

			if( Main.netMode != NetmodeID.Server ) {
				Fx.CreateSteamEruptionFx( this.player.MountedCenter, steamAmount );
			}
		}
	}
}