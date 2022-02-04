using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
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
			var config = SteampunkArsenalConfig.Instance;
			float dmgScale = config.Get<float>( nameof(config.RiveterBackfireDamagePerSteamUnit) );
			int damage = (int)(steamAmount * dmgScale);

			this.player.Hurt(
				damageSource: PlayerDeathReason.ByCustomReason(
					this.player.name+" cleared "+(this.player.Male?"his":"her")+" pores" ),
				Damage: damage,
				hitDirection: 0
			);

			//

			if( Main.netMode != NetmodeID.Server ) {
				Fx.CreateSteamEruptionFx( this.player.MountedCenter, 12f, steamAmount );

				Main.PlaySound( SoundID.Item14, this.player.MountedCenter );

				Main.PlaySound(
					type: this.mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/SteamPuff"),
					position: this.player.MountedCenter
				);
			}
		}
	}
}