using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;


namespace SteampunkArsenal.Net {
	class NPCPinProtocol : SimplePacketPayload {
		public static void SendToClients( int npcWho, int rivetProjOwner, int rivetProjId, int pinStrength, Vector2 offset ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not a client." );
			}

			var payload = new NPCPinProtocol( npcWho, rivetProjOwner, rivetProjId, pinStrength, offset );

			SimplePacket.SendToClient( payload );
		}



		////////////////

		public int NpcWho;
		public int RivetProjOwner;
		public int RivetProjId;
		public int PinStrength;
		public Vector2 Offset;



		////////////////

		private NPCPinProtocol() { }

		private NPCPinProtocol( int npcWho, int rivetProjOwner, int rivetProjId, int pinStrength, Vector2 offset ) {
			this.NpcWho = npcWho;
			this.RivetProjOwner = rivetProjOwner;
			this.RivetProjId = rivetProjId;
			this.PinStrength = pinStrength;
			this.Offset = offset;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException( "NPCPinProtocol can't receive on server" );
		}

		public override void ReceiveOnClient() {
			NPC npc = Main.npc[ this.NpcWho ];
			if( npc?.active != true ) {
				throw new ModLibsException( "No such NPC to rivet for index "+this.NpcWho );
			}
			
			int projWho = Projectile.GetByUUID( this.RivetProjOwner, this.RivetProjId );
			if( projWho == -1 ) {
				throw new ModLibsException( "No rivet found for id "+this.RivetProjOwner+","+this.RivetProjId );
			}

			var mynpc = npc.GetGlobalNPC<SteamArseNPC>();
			mynpc.SyncRivetPinFor( projWho, this.PinStrength, this.Offset );
		}
	}
}
