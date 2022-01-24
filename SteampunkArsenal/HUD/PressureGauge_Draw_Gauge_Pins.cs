using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using HUDElementsLib;
using SteampunkArsenal.Logic.Steam;


namespace SteampunkArsenal.HUD {
	partial class PressureGaugeHUD : HUDElement, ILoadable {
		private void DrawGaugePins( SpriteBatch sb, Vector2 pos ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<SteamArsePlayer>();
			var boilers = myplayer.ImplicitConvergingBoiler;

			float boilersGaugePercent = boilers.SteamPressure / (boilers.TotalCapacity - boilers.Water);

			this.DrawGaugePin( sb, pos, 1, boilersGaugePercent );

			//

			SteamSource container = myplayer.GetHeldRivetLauncherSteam();
			float gunGaugePercent = container != null
				? container.TotalPressure / container.TotalCapacity
				: 0f;

			this.DrawGaugePin( sb, pos, 2, gunGaugePercent );
		}


		////////////////


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
