using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SteampunkArsenal.Net;

namespace SteampunkArsenal {
	partial class SteamArsePlayer : ModPlayer, ISteamPressureSource {
		public void AddPressurePercent( float amount ) {
		}

		public float GetPressurePercent() {
			return 0f;
		}

		public float TransferPressureToMeFromSource( ISteamPressureSource source, float amount ) {
			return amount;
		}


		////////////////

		public void ApplySteamDamage_Local_Syncs( float steamAmount ) {
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

			this.CreateSteamFx( steamAmount );
		}


		////////////////

		public void CreateSteamFx( float steamAmount ) {
			if( Main.netMode == NetmodeID.Server ) {
				return;
			}

			Vector2 pos, vel;

			//

			void RollPosVel() {
				pos = this.player.MountedCenter + new Vector2( Main.rand.Next(-16, 16), Main.rand.Next(-24, 24) );
				vel = new Vector2( Main.rand.Next(-8, 8), Main.rand.Next(-8, 8) );
			}

			//

			int smallPuffs = (int)steamAmount / 5;
			for( int i=0; i<smallPuffs; i++ ) {
				RollPosVel();

				int goreIdx = Gore.NewGore( pos, vel, 11 );
				Main.gore[goreIdx].alpha = 128;
			}

			int medPuffs = (int)steamAmount / 20;
			for( int i=0; i<medPuffs; i++ ) {
				RollPosVel();

				int goreIdx = Gore.NewGore( pos, vel, 11, 2f );
				Main.gore[goreIdx].alpha = 128;
			}

			int bigPuffs = (int)steamAmount / 35;
			for( int i=0; i<bigPuffs; i++ ) {
				RollPosVel();

				int goreIdx = Gore.NewGore( pos, vel, 862 );
				Main.gore[goreIdx].alpha = 128;
			}
		}
	}
}