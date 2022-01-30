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
		private void DrawGaugeCurrentGelGauge( SpriteBatch sb, Vector2 pos ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<SteamArsePlayer>();

			int gels = (int)Math.Ceiling(myplayer.ImplicitConvergingBoiler.BoilerHeat) - 1;

			this.DrawGaugeGelGauge( sb, pos, gels );
		}


		////////////////

		private void DrawGaugeGelGauge( SpriteBatch sb, Vector2 pos, int gels ) {
			var mymod = SteamArseMod.Instance;
			Texture2D gelTex = mymod.GetTexture( "HUD/PressureGaugeGelMeter" );

			//

			int height = Math.Min( gels * 2, gelTex.Height );
			float offsetY = gelTex.Height - height;

			//

			sb.Draw(
				texture: gelTex,
				position: pos + new Vector2(26f, 18f + offsetY),
				sourceRectangle: new Rectangle(0, (int)offsetY, gelTex.Width, height ),
				color: Color.White,
				rotation: 0f,
				origin: default,
				scale: 1f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}
	}
}
