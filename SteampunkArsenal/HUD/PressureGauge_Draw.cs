using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using HUDElementsLib;
using SteampunkArsenal.Logic.Steam.SteamSources;


namespace SteampunkArsenal.HUD {
	partial class PressureGaugeHUD : HUDElement, ILoadable {
		protected override bool PreDrawSelf( SpriteBatch sb ) {
			Vector2 pos = this.GetHUDComputedPosition( true );

			//

			this.DrawGaugeBG( sb, pos );
			this.DrawGaugeWater( sb, pos );
			this.DrawGaugePins( sb, pos );

			//
			
			if( this.PopupTextElapsed > 0 ) {
				if( PressureGaugeHUD.DrawPopupText_If(sb, pos, this.PopupText, this.PopupTextColor, this.PopupTextElapsed) ) {
					this.PopupTextElapsed++;
				} else {
					this.PopupTextElapsed = 0;
				}
			}

			//

			return false;
		}


		////////////////
		

		private void DrawGaugePins( SpriteBatch sb, Vector2 pos ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<SteamArsePlayer>();

			float boilersGaugePercent = myplayer.AllBoilers.SteamPressure / myplayer.AllBoilers.SteamCapacity;

			this.DrawGaugePin( sb, pos, 1, boilersGaugePercent );

			//

			SteamContainer container = myplayer.GetHeldRivetLauncherSteam();
			float gunGaugePercent = container != null
				? container.SteamPressure / container.SteamCapacity
				: 0f;

			this.DrawGaugePin( sb, pos, 2, gunGaugePercent );
		}


		////////////////

		private void DrawGaugeBG( SpriteBatch sb, Vector2 pos ) {
			var mymod = SteamArseMod.Instance;
			Texture2D bg = this.AnimState == 0
				? mymod.GetTexture( "HUD/PressureGaugeBG_A" )
				: mymod.GetTexture( "HUD/PressureGaugeBG_B" );

			//

			sb.Draw(
				texture: bg,
				position: pos,
				color: Color.White
			);
		}


		private void DrawGaugeWater( SpriteBatch sb, Vector2 pos ) {
			var mymod = SteamArseMod.Instance;
			Texture2D waterTex = mymod.GetTexture( "HUD/PressureGaugeWater" );

			//

			var myplayer = Main.LocalPlayer.GetModPlayer<SteamArsePlayer>();
			float waterPerc = myplayer.AllBoilers.Water / myplayer.AllBoilers.SteamCapacity;
			if( waterPerc <= 0f ) {
				return;
			}

			//

			float waterTexSpanHeight = 50f;

			int texHeight = (int)(waterPerc * waterTexSpanHeight);

			float texOffsetY = 24f;
			texOffsetY += (1f - waterPerc) * waterTexSpanHeight;

			//

			var offset = new Vector2( 0f, texOffsetY );
			var rect = new Rectangle( 0, (int)texOffsetY, waterTex.Width, texHeight );

			//
			
			sb.Draw(
				texture: waterTex,
				position: pos + offset,
				sourceRectangle: rect,
				color: Color.White * 0.35f,
				rotation: 0f,
				origin: default,
				scale: 1f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}


		private void DrawGaugePin( SpriteBatch sb, Vector2 pos, int pinNum, float percent ) {
			var mymod = SteamArseMod.Instance;
			Texture2D fg = mymod.GetTexture( "HUD/PressureGaugeFG" );
			Texture2D pin = mymod.GetTexture( "HUD/PressureGaugePin"+pinNum );

			//

			Vector2 pinOffset = new Vector2(fg.Width, fg.Height) * 0.5f;
			Vector2 pinOrigin = new Vector2(
				pin.Width / 2,
				pin.Height / 2
			);

			//

			float gauge = (float)Math.PI * -0.25f;
			gauge += percent * (float)Math.PI * 1.5f;

			//
			
			sb.Draw(
				texture: pin,
				position: pos + pinOffset,
				sourceRectangle: null,
				color: Color.White,
				rotation: gauge,
				origin: pinOrigin,
				scale: 1f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);

			//

			sb.Draw(
				texture: fg,
				position: pos,
				color: Color.White
			);
		}
	}
}
