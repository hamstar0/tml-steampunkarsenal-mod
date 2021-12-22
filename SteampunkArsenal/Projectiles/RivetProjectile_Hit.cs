using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal.Projectiles {
	public partial class RivetProjectile : ModProjectile {
		public override void OnHitNPC( NPC target, int damage, float knockback, bool crit ) {
			if( this.projectile.velocity != default && !target.townNPC ) {
				this.HitTargets.Add( target );
			}
		}


		////////////////

		public override bool OnTileCollide( Vector2 oldVelocity ) {
			Vector2 vel;

			if( oldVelocity.LengthSquared() < 0.1f ) {
				vel = default;
			} else {
				this.projectile.position -= oldVelocity;

				vel = oldVelocity * 0.25f;
			}
			
			this.projectile.velocity = vel;

			return false;
		}
	}
}
