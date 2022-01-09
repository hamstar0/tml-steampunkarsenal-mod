using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SteampunkArsenal.Net;


namespace SteampunkArsenal {
	partial class SteamArsePlayer : ModPlayer {
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
				damageSource: PlayerDeathReason.ByCustomReason( this.player.name+" cleared their pores" ),
				Damage: (int)steamAmount,
				hitDirection: 0
			);

			this.CreateSteamFx_If( steamAmount );
		}


		////////////////

		public void CreateSteamFx_If( float steamAmount ) {
			if( Main.netMode == NetmodeID.Server ) {
				return;
			}

			Fx.CreateSteamEruptionFx( this.player.MountedCenter, steamAmount );
		}
	}
}