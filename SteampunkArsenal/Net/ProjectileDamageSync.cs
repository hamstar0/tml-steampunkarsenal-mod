using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using SteampunkArsenal.Projectiles;


namespace SteampunkArsenal.Net {
	class ProjectileDamageSyncProtocol : SimplePacketPayload {
		public static void BroadcastFromClientToAll( int projectileWho, int damage ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not a client." );
			}

			var payload = new ProjectileDamageSyncProtocol( projectileWho, damage );

			SimplePacket.SendToServer( payload );
		}



		////////////////

		public int ProjectileWho;
		public int Damage;



		////////////////

		private ProjectileDamageSyncProtocol() { }

		private ProjectileDamageSyncProtocol( int projectileWho, int damage ) {
			this.ProjectileWho = projectileWho;
			this.Damage = damage;
		}


		////////////////

		private bool Receive() {
			Projectile proj = Main.projectile[this.ProjectileWho];

			if( proj?.active != true ) {
				LogLibraries.Alert( "Could not sync rivet projectile damage - No projectile" );
				return false;
			}
			if( proj.type != ModContent.ProjectileType<RivetProjectile>() ) {
				LogLibraries.Alert( "Could not sync rivet projectile damage - Invalid projectile" );
				return false;
			}

			proj.damage = this.Damage;

			return true;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			if( this.Receive() ) {
				SimplePacket.SendToClient( this, -1, fromWho );
			}
		}

		public override void ReceiveOnClient() {
			this.Receive();
		}
	}
}
