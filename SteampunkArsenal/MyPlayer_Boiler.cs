using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal {
	partial class SteamArsePlayer : ModPlayer {
		private void UpdateBoiler() {
			SoundEffectInstance waterDrawSnd = SteamArseMod.Instance.WaterDraw;
			bool isWet = this.player.wet && !this.player.honeyWet && !this.player.lavaWet;
			bool isInterrupted = false;

			if( isWet ) {
				float fillAmt = this.MyBoiler.AddWater( 1f, 1f );

				//

				if( fillAmt > 0f ) {
					switch( waterDrawSnd.State ) {
					case SoundState.Stopped:
						waterDrawSnd.Play();

						Main.NewText( "Refilling boiler...", Color.CornflowerBlue );
						break;
					case SoundState.Paused:
						waterDrawSnd.Resume();

						Main.NewText( "Refilling boiler...", Color.DarkSeaGreen );
						break;
					}
				} else {
					isInterrupted = waterDrawSnd.State == SoundState.Playing;
				}
			} else if( waterDrawSnd.State == SoundState.Playing ) {
				isInterrupted = true;
			}
			
			if( isInterrupted ) {
				waterDrawSnd.Stop();

				Main.NewText( "Refilling interrupted.", Color.DarkOrchid );
			}

			//

			this.MyBoiler.Update();
		}
	}
}