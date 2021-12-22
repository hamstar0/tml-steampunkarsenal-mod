using System.Collections.Generic;
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

		private float OldRotation;

		private ISet<NPC> HitTargets = new HashSet<NPC>();


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Hot Rivet" );
			ProjectileID.Sets.TrailCacheLength[ projectile.type ] = 5;
			ProjectileID.Sets.TrailingMode[ projectile.type ] = 0;
		}

		public override void SetDefaults() {
			this.projectile.width = 4;
			this.projectile.height = 4;

			this.projectile.friendly = true;
			this.projectile.hostile = false;
			this.projectile.ranged = true;

			this.projectile.ignoreWater = true;
			this.projectile.tileCollide = true;

			this.projectile.extraUpdates = 4;	//Set to above 0 if you want the projectile to update multiple time in a frame

			this.projectile.penetrate = 9;		//How many monsters the projectile can penetrate
			this.projectile.timeLeft = 600 * this.projectile.extraUpdates;

			this.projectile.alpha = 255;
			//this.projectile.light = 0.5f;		//How much light emit around the projectile

			this.projectile.aiStyle = 1;
			this.aiType = ProjectileID.Bullet;
		}


		////////////////

		public override bool? CanHitNPC( NPC target ) {
			return this.projectile.velocity != default
				&& !target.townNPC;
		}

		public override bool CanHitPlayer( Player target ) {
			return this.projectile.velocity != default
				&& target.hostile;
		}


		////////////////
		
		public override void PostAI() {
			if( this.projectile.velocity != default ) {
				this.UpdateMoving();
			} else {
				this.UpdateStopped();
			}
		}

		////

		private void UpdateMoving() {
			this.OldRotation = this.projectile.rotation;
		}

		private void UpdateStopped() {
			this.projectile.rotation = this.OldRotation;

			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				foreach( NPC npc in this.HitTargets ) {
					SteamArseNPC.ApplyRivetToIf_SyncsFromServer( npc, this.projectile );
				}

				this.HitTargets.Clear();
			}
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
	}
}
