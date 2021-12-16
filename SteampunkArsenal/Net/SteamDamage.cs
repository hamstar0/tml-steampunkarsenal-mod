using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;


namespace SteampunkArsenal.Net {
	class SteamDamageProtocol : SimplePacketPayload {
		public static void BroadcastFromClientToAll( int playerWho, float steamAmount ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not a client." );
			}

			var payload = new SteamDamageProtocol( playerWho, steamAmount );

			SimplePacket.SendToServer( payload );
		}



		////////////////

		public int PlayerWho;
		public float SteamAmount;



		////////////////

		private SteamDamageProtocol() { }

		private SteamDamageProtocol( int playerWho, float steamAmount ) {
			this.PlayerWho = playerWho;
			this.SteamAmount = steamAmount;
		}


		////////////////

		private void Receive() {
			Player plr = Main.player[ this.PlayerWho ];
			var myplr = plr.GetModPlayer<SteamArsePlayer>();

			myplr.ApplySteamDamage( this.SteamAmount );
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			this.Receive();

			SimplePacket.SendToClient( this, -1, fromWho );
		}

		public override void ReceiveOnClient() {
			this.Receive();
		}
	}
}
