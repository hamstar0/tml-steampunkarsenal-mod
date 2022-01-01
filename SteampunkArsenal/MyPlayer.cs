using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Items;


namespace SteampunkArsenal {
	partial class SteamArsePlayer : ModPlayer {
		internal ConvergentBoiler MyBoiler { get; private set; } = new ConvergentBoiler();



		////////////////

		public override void PreUpdate() {
			this.MyBoiler.RefreshConnectedBoilers( this.player );
/*Item heldItem = this.player.HeldItem;
var myitem = heldItem.modItem as SteamPoweredRivetLauncherItem;
DebugLibraries.Print( "steam", this.Boiler.SteamPressure.ToString("N2")+", "+myitem?.Boiler.SteamPressure.ToString("N2") );

this.Boiler.AddWater( 1f, 1f );*/
			
			//

			foreach( Item item in this.player.inventory ) {
				Boiler boiler = Boiler.GetBoilerForItem( item );
				if( boiler?.IsActive ?? false ) {
					boiler?.Update();
				}
			}

			//

			if( this.MyBoiler.IsActive ) {
				this.UpdateBoiler();
			}
DebugLibraries.Print(
	"boiler",
	"Water: "+this.MyBoiler.Water.ToString("N2")
		+", Heat: "+this.MyBoiler.Heat.ToString("N2")
		+", Steam: "+this.MyBoiler.SteamPressure.ToString("N2")
		+", Gun: "+Boiler.GetBoilerForItem(this.player.HeldItem)?.SteamPressure.ToString("N2")
);
		}


		////

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


		////////////////
		
		public override bool PreItemCheck() {
			Item item = this.player.HeldItem;
			if( item?.active == true && item.type == ModContent.ItemType<SteamPoweredRivetLauncherItem>() ) {
				SteamPoweredRivetLauncherItem.RunHeldBehavior_Local_If( this.player, item );
			}

			return base.PreItemCheck();
		}
	}
}