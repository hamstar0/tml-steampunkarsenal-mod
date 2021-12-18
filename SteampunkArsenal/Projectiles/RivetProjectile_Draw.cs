using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal.Projectiles {
	public partial class RivetProjectile : ModProjectile {
		public override bool PreDraw( SpriteBatch spriteBatch, Color lightColor ) {
			Texture2D tex = Main.projectileTexture[projectile.type];
			float oldPosLen = projectile.oldPos.Length;

			Vector2 drawOrigin = new Vector2(tex.Width, projectile.height) * 0.5f;

			for( int k = 0; k < projectile.oldPos.Length; k++ ) {
				Vector2 drawPos = projectile.oldPos[k]
					- Main.screenPosition
					+ drawOrigin
					+ new Vector2( 0f, projectile.gfxOffY );

				float opacity = (oldPosLen - (float)k) / oldPosLen;
				Color color = projectile.GetAlpha( lightColor ) * opacity;

				//Redraw the projectile with the color not influenced by light
				spriteBatch.Draw(
					tex,
					drawPos,
					null,
					color,
					projectile.rotation,
					drawOrigin,
					projectile.scale,
					SpriteEffects.None,
					0f
				);
			}

			return true;
		}
	}
}
