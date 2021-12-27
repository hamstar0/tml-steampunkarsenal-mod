using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Projectiles;


namespace SteampunkArsenal {
	partial class SteamArseNPC : GlobalNPC {
		private int OldAiStyle = -1;

		////

		private IDictionary<Projectile, Vector2> RivetedTo = new Dictionary<Projectile, Vector2>();


		////////////////////

		public override bool CloneNewInstances => false;

		public override bool InstancePerEntity => true;




		////////////////////

		public override bool PreAI( NPC npc ) {
			if( this.RivetedTo.Count == 0 ) {
				if( this.OldAiStyle != -1 ) {
					npc.aiStyle = this.OldAiStyle;

					this.OldAiStyle = -1;
				}
			}

			return base.PreAI( npc );
		}


		public override void PostAI( NPC npc ) {
			bool hasRivet = false;
			int rivetProjType = ModContent.ProjectileType<RivetProjectile>();

			foreach( Projectile rivetProj in this.RivetedTo.Keys.ToArray() ) {
				if( !rivetProj.active || rivetProj.type != rivetProjType ) {
					this.RivetedTo.Remove( rivetProj );
				} else {
					hasRivet = true;
				}
			}

			if( hasRivet ) {
				if( !this.UpdateRivetBehavior(npc) ) {
					LogLibraries.WarnOnce( "NPC expects rivet pin, but none found!" );
				}
			}
		}
	}
}