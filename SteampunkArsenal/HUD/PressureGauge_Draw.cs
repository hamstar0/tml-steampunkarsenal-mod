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
			var mymod = SteamArseMod.Instance;
			Texture2D bg = this.AnimState == 0
				? mymod.GetTexture( "HUD/PressureGaugeBG_A" )
				: mymod.GetTexture( "HUD/PressureGaugeBG_B" );
			Texture2D fg = mymod.GetTexture( "HUD/PressureGaugeFG" );
			Texture2D pin = mymod.GetTexture( "HUD/PressureGaugePin" );

			Vector2 pos = this.GetHUDComputedPosition( true );
			Vector2 pinOrigin = new Vector2(
				pin.Width / 2,
				pin.Height / 2
			);

			//

			var myplayer = Main.LocalPlayer.GetModPlayer<SteamArsePlayer>();
			float pressure = myplayer.MyBoiler.SteamPressure;
			float gaugePercent = pressure / 100f;
			float gauge = (float)Math.PI * 0.5f;
			gauge += gaugePercent * (float)Math.PI * 1.5f;

			//

			sb.Draw(
				texture: bg,
				position: pos,
				color: Color.White
			);

			sb.Draw(
				texture: pin,
				position: pos + new Vector2(bg.Width, bg.Height) * 0.5f,
				sourceRectangle: null,
				color: Color.White,
				rotation: gauge,
				origin: pinOrigin,
				scale: 1f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);

			sb.Draw(
				texture: fg,
				position: pos,
				color: Color.White
			);

			//

			return false;
		}
	}
}
