using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal {
	partial class SteamArsePlayer : ModPlayer {
		private static bool HasDisplayedRefillingAlert = false;



		////////////////

		private void UpdateBoiler() {
			if( !this.player.dead ) {
				this.UpdateBoilerRefillState();
			}

			//

			this.ImplicitConvergingBoiler.PreUpdate( this.player, false );
			this.ImplicitConvergingBoiler.PostUpdate( this.player, false );
		}


		////////////////

		private void UpdateBoilerRefillState() {
			SoundEffectInstance waterDrawSnd = SteamArseMod.Instance.WaterDraw;
			bool isWet = this.player.wet && !this.player.honeyWet && !this.player.lavaWet;
			bool isInterrupted = false;

			if( !this.player.dead && isWet ) {
				this.ApplyBoilerRefill( ref isInterrupted );
			} else if( waterDrawSnd.State == SoundState.Playing ) {
				isInterrupted = true;
			}
			
			if( isInterrupted ) {
				waterDrawSnd.Stop();

				if( !SteamArsePlayer.HasDisplayedRefillingAlert ) {
					SteamArsePlayer.HasDisplayedRefillingAlert = true;

					Main.NewText( "Refilling interrupted.", Color.DarkOrchid );
				}
			}
		}


		////////////////

		private void ApplyBoilerRefill( ref bool isInterrupted ) {
			SoundEffectInstance waterDrawSnd = SteamArseMod.Instance.WaterDraw;

			float intendedFillAmt = 8f / 60f;
			float actualFillAmt = this.ImplicitConvergingBoiler.AddWater( intendedFillAmt, 1f, out _ );

			//

			if( actualFillAmt > 0.0001f ) {	// floating point shenanigans
				switch( waterDrawSnd.State ) {
				case SoundState.Stopped:
					waterDrawSnd.Play();

					if( !SteamArsePlayer.HasDisplayedRefillingAlert ) {
						Main.NewText( "Refilling boiler...", Color.CornflowerBlue );
					}
					break;
				case SoundState.Paused:
					waterDrawSnd.Resume();

					if( !SteamArsePlayer.HasDisplayedRefillingAlert ) {
						Main.NewText( "Refilling boiler...", Color.DarkSeaGreen );
					}
					break;
				}
			} else {
				isInterrupted = waterDrawSnd.State == SoundState.Playing;
			}
		}
	}
}