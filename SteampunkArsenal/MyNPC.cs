using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SteampunkArsenal {
	partial class SteamArseNPC : GlobalNPC {
		private int OldAiStyle = -1;


		////////////////////

		public Projectile RivetedTo { get; private set; } = null;

		public Vector2 RivetOffset { get; private set; }


		////////////////////

		public override bool CloneNewInstances => false;

		public override bool InstancePerEntity => true;




		////////////////////

		public override bool PreAI( NPC npc ) {
			if( this.RivetedTo == null ) {
				if( this.OldAiStyle != -1 ) {
					npc.aiStyle = this.OldAiStyle;

					this.OldAiStyle = -1;
				}
			}

			return base.PreAI( npc );
		}


		public override void PostAI( NPC npc ) {
			if( this.RivetedTo != null ) {
				if( this.OldAiStyle == -1 ) {
					this.OldAiStyle = npc.aiStyle;

					npc.aiStyle = 0;
				}

				if( this.RivetedTo?.active != true ) {
					this.RivetedTo = null;
				} else {
					npc.Center = this.RivetedTo.Center + this.RivetOffset;
					npc.velocity = default;

					this.ApplySquirmingRivetDamageIf( npc );
				}
			}
		}


		////////////////////

		public void ApplySquirmingRivetDamageIf( NPC npc ) {
			var config = SteampunkArsenalConfig.Instance;

			float squirmScale = config.Get<float>( nameof(config.SquirmPerSecondLifeScale) );
			float squirmPerSecond = (float)npc.lifeMax / 2;

			this.RivetedTo.timeLeft -= (int)(squirmPerSecond / 60f);
			if( this.RivetedTo.timeLeft < 0 ) {
				this.RivetedTo.timeLeft = 0;
			}
		}
	}
}