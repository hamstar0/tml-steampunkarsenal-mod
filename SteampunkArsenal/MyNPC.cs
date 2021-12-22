using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.NPCs;
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


		////

		private bool UpdateRivetBehavior( NPC npc ) {
			Projectile rivet = this.RivetedTo.Keys.FirstOrDefault();
			if( rivet == null ) {
				return false;
			}

			//

			bool isUnpinnable = npc.realLife > 0 || npc.boss;

			if( !isUnpinnable ) {
				this.UpdateRivetPinnings( rivet, npc );
			}

			//

			this.ApplySquirmingBehavior( npc, isUnpinnable );

			return true;
		}

		////

		private void UpdateRivetPinnings( Projectile rivet, NPC npc ) {
			if( this.OldAiStyle == -1 ) {
				this.OldAiStyle = npc.aiStyle;

				npc.aiStyle = 0;
			}

			Vector2 offset = this.RivetedTo[rivet];

			npc.Center = rivet.Center + offset;
			npc.velocity = default;
		}


		////////////////////

		public void ApplySquirmingBehavior( NPC npc, bool isUnpinnable ) {
			if( isUnpinnable ) {
				this.ApplySquirmingNpcDamage( npc );
			}
			
			this.ApplySquirmingRivetDamage( npc );
		}

		////

		private float _BufferedSquirmDamageToNpc = 0f;

		private void ApplySquirmingNpcDamage( NPC npc ) {
			var config = SteampunkArsenalConfig.Instance;
			
			float squirmPerSecond = config.Get<float>( nameof(config.SquirmUnpinnableNpcDamagePerSecondBase) );
			float squirmPerPerSecond = squirmPerSecond * (float)this.RivetedTo.Count;

			this._BufferedSquirmDamageToNpc += squirmPerPerSecond / 60f;

			if( this._BufferedSquirmDamageToNpc >= 1f ) {
				NPCLibraries.RawHurt( npc, (int)this._BufferedSquirmDamageToNpc, false );

				this._BufferedSquirmDamageToNpc -= (int)this._BufferedSquirmDamageToNpc;
			}
		}

		////

		 private float _BufferedSquirmDamageToRivet = 0f;

		private void ApplySquirmingRivetDamage( NPC npc ) {
			var config = SteampunkArsenalConfig.Instance;

			float squirmDamageScale = config.Get<float>( nameof(config.SquirmRivetDamageScale) );
			float squirmPerSecond = (float)npc.lifeMax * squirmDamageScale;
			squirmPerSecond += config.Get<float>( nameof(config.SquirmRivetDamagePerSecondBase) );
			float squirmPerPerSecond = squirmPerSecond / (float)this.RivetedTo.Count;

			foreach( Projectile rivet in this.RivetedTo.Keys ) {
				this._BufferedSquirmDamageToRivet += squirmPerPerSecond / 60f;

				if( this._BufferedSquirmDamageToRivet >= 1f ) {
					rivet.timeLeft -= (int)this._BufferedSquirmDamageToRivet;
					if( rivet.timeLeft < 0 ) {
						rivet.timeLeft = 0;
					}

					this._BufferedSquirmDamageToRivet -= (int)this._BufferedSquirmDamageToRivet;
				}
			}
		}
	}
}