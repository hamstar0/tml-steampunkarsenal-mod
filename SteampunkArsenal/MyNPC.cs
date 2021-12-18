using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SteampunkArsenal {
	partial class SteamArseNPC : GlobalNPC {
		public static void ApplyRivetToIf( NPC npc, Projectile rivetProjectile ) {
			if( npc?.active != true || rivetProjectile?.active != true ) {
				return;
			}
			if( npc.GetGlobalNPC<SteamArseNPC>().RivetedTo != null ) {
				return;
			}

			Vector2 diff = npc.Center - rivetProjectile.Center;
			float distSqr = diff.LengthSquared();

			if( distSqr < (128f * 128f) ) {
				SteamArseNPC.ApplyRivetTo( npc, rivetProjectile );
			}
		}

		private static void ApplyRivetTo( NPC npc, Projectile rivetProjectile ) {
			float npcDim = (npc.width + npc.height) * 0.5f;

			Vector2 diff = npc.Center - rivetProjectile.Center;
			Vector2 offset = Vector2.Normalize(diff) * npcDim * 0.75f;

			var mynpc = npc.GetGlobalNPC<SteamArseNPC>();

			mynpc.RivetedTo = rivetProjectile;
			mynpc.RivetOffset = offset;
		}



		////////////////////

		public Projectile RivetedTo { get; private set; } = null;

		public Vector2 RivetOffset { get; private set; }


		////////////////////

		public override bool CloneNewInstances => false;

		public override bool InstancePerEntity => true;




		////////////////////

		public override void PostAI( NPC npc ) {
			if( this.RivetedTo != null ) {
				if( this.RivetedTo?.active != true ) {
					this.RivetedTo = null;
				} else {
					npc.Center = this.RivetedTo.Center + this.RivetOffset;

					this.ApplySquirmingDamageIf( npc );
				}
			}
		}


		////////////////////

		public void ApplySquirmingDamageIf( NPC npc ) {

		}
	}
}