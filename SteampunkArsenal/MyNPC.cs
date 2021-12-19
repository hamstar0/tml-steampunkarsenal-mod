using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SteampunkArsenal {
	partial class SteamArseNPC : GlobalNPC {
		private int OldAiStyle = -1;


		////////////////////

		public IDictionary<Projectile, Vector2> RivetedTo { get; private set; } = new Dictionary<Projectile, Vector2>();


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
			if( this.RivetedTo.Count > 0 ) {
				foreach( Projectile eachRivet in this.RivetedTo.Keys.ToArray() ) {
					if( !eachRivet.active ) {
						this.RivetedTo.Remove( eachRivet );
					}
				}

				if( this.OldAiStyle == -1 ) {
					this.OldAiStyle = npc.aiStyle;

					npc.aiStyle = 0;
				}

				Projectile rivet = this.RivetedTo.Keys.First();
				Vector2 offset = this.RivetedTo[ rivet ];

				npc.Center = rivet.Center + offset;
				npc.velocity = default;

				this.ApplySquirmingRivetDamageIf( npc );
			}
		}


		////////////////////

		public void ApplySquirmingRivetDamageIf( NPC npc ) {
			var config = SteampunkArsenalConfig.Instance;

			float squirmScale = config.Get<float>( nameof(config.SquirmPerSecondLifeScale) );
			float squirmPerSecond = (float)npc.lifeMax * squirmScale;
			float squirmPerRivetPerSecond = squirmPerSecond / (float)this.RivetedTo.Count;

			foreach( Projectile rivet in this.RivetedTo.Keys ) {
				rivet.timeLeft -= (int)( squirmPerRivetPerSecond / 60f );
				if( rivet.timeLeft < 0 ) {
					rivet.timeLeft = 0;
				}
			}
		}
	}
}