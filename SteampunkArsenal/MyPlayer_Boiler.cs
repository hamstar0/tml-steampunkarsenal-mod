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
			this.UpdateBoilerRefillState();

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
			var config = SteampunkArsenalConfig.Instance;
			SoundEffectInstance waterDrawSnd = SteamArseMod.Instance.WaterDraw;

			float intendedFillAmtPerSec = config.Get<float>( nameof(config.BoilerWaterFillRatePerSecond) );
			float intendedFillAmtPerTick = intendedFillAmtPerSec / 60f;
			float actualFillAmt = this.ImplicitConvergingBoiler.AddWater_If( intendedFillAmtPerTick, 1f, out _ );

			//

			if( actualFillAmt >= (intendedFillAmtPerTick - 0.0001f) ) {	// floating point shenanigans
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