using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using HUDElementsLib;


namespace SteampunkArsenal.HUD {
	partial class PressureGaugeHUD : HUDElement, ILoadable {
		protected override bool PreDrawSelf( SpriteBatch sb ) {
			Vector2 pos = this.GetHUDComputedPosition( true );

			//

			this.DrawGaugeBG( sb, pos );
			this.DrawGaugeWater( sb, pos );
			this.DrawGaugePin( sb, pos );

			//

			return false;
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
			float waterPerc = myplayer.AllBoilers.Water / myplayer.AllBoilers.Capacity;
			if( waterPerc <= 0f ) {
				return;
			}

			//

			float waterTexSpanHeight = 50f;

			int texHeight = (int)(waterPerc * waterTexSpanHeight);

			float texOffsetY = 24f;
			texOffsetY += (1f - waterPerc) * waterTexSpanHeight;

			//

			sb.Draw(
				texture: waterTex,
				position: pos + new Vector2(waterTex.Width, texOffsetY),
				sourceRectangle: new Rectangle( 0, (int)texOffsetY, waterTex.Width, texHeight ),
				color: Color.White * 0.25f,
				rotation: 0f,
				origin: default,
				scale: 1f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}


		private void DrawGaugePin( SpriteBatch sb, Vector2 pos ) {
			var mymod = SteamArseMod.Instance;
			Texture2D fg = mymod.GetTexture( "HUD/PressureGaugeFG" );
			Texture2D pin = mymod.GetTexture( "HUD/PressureGaugePin" );

			//

			Vector2 pinOffset = new Vector2(fg.Width, fg.Height) * 0.5f;
			Vector2 pinOrigin = new Vector2(
				pin.Width / 2,
				pin.Height / 2
			);

			//

			var myplayer = Main.LocalPlayer.GetModPlayer<SteamArsePlayer>();
			float pressure = myplayer.AllBoilers.SteamPressure;
			float gaugePercent = pressure / 100f;
			float gauge = (float)Math.PI * 0.5f;
			gauge += gaugePercent * (float)Math.PI * 1.5f;

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
