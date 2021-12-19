using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SteampunkArsenal {
	partial class SteamArseNPC : GlobalNPC {
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
					Vector2 squirm = npc.velocity;

					npc.Center = this.RivetedTo.Center + this.RivetOffset;
					npc.velocity = default;

					this.ApplySquirmingDamageIf( npc, squirm );
				}
			}
		}


		////////////////////

		public void ApplySquirmingDamageIf( NPC npc, Vector2 squirm ) {

		}
	}
}