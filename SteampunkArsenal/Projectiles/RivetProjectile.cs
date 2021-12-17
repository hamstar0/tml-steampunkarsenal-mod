using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.ProjectileOwner;
using SteampunkArsenal.Items;


namespace SteampunkArsenal.Projectiles {
	public partial class RivetProjectile : ModProjectile {
		public static void ApplyRivetStats_LocalOnly( int projectileIdx, Projectile projectile ) {
			Player plrOwner = projectile.GetPlayerOwner();
			if( plrOwner == null ) {
				LogLibraries.Warn( "Could not assign rivet an owner." );
				return;
			}

			if( Main.netMode == NetmodeID.Server ) {
				return;
			}
			if( plrOwner.whoAmI != Main.myPlayer ) {
				return;
			}

			Item riveter = plrOwner.HeldItem;
			if( riveter?.active != true || riveter.type != ModContent.ItemType<SteamPoweredRivetLauncherItem>() ) {
				LogLibraries.Warn( "Could not get rivet's launcher." );
				return;
			}

			var myriveter = riveter.modItem as SteamPoweredRivetLauncherItem;
			if( myriveter == null ) {
				LogLibraries.Warn( "Could not get rivet's launcher (ModItem)." );
				return;
			}

			//

			myriveter.ApplyLaunchedRivetStats_NonServer_Syncs( projectileIdx, projectile );
		}



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Hot Rivet" );
			ProjectileID.Sets.TrailCacheLength[ projectile.type ] = 5;
			ProjectileID.Sets.TrailingMode[ projectile.type ] = 0;
		}

		public override void SetDefaults() {
			this.projectile.width = 8;
			this.projectile.height = 8;
			this.projectile.aiStyle = 1;
			this.projectile.friendly = true;
			this.projectile.hostile = false;
			this.projectile.ranged = true;
			this.projectile.penetrate = 9;		//How many monsters the projectile can penetrate
			this.projectile.timeLeft = 600;
			this.projectile.alpha = 255;
			this.projectile.light = 0.5f;		//How much light emit around the projectile
			this.projectile.ignoreWater = true;
			this.projectile.tileCollide = true;
			this.projectile.extraUpdates = 7;	//Set to above 0 if you want the projectile to update multiple time in a frame
			this.aiType = ProjectileID.Bullet;
		}


		////////////////

		public override bool OnTileCollide( Vector2 oldVelocity ) {
Main.NewText("hit");
			return false;
		}


		////////////////

		public override void Kill( int timeLeft ) {
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(
				projectile.position + projectile.velocity,
				projectile.velocity,
				projectile.width,
				projectile.height
			);

			Main.PlaySound( SoundID.Item10, projectile.position );
		}


		////////////////

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
