using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using ModLibsCore.Classes.Loadable;
using HUDElementsLib;


namespace SteampunkArsenal.HUD {
	class PressureGaugeHUD : HUDElement, ILoadable {
		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() {
			var config = SteampunkArsenalConfig.Instance;
			var posX = config.Get<float>( nameof(config.PressureGaugeHUDPositionX) );
			var posY = config.Get<float>( nameof(config.PressureGaugeHUDPositionY) );

			Texture2D tex = SteamArseMod.Instance.GetTexture( "PressureGaugeBG" );

			HUDElementsLibAPI.AddWidget(
				new PressureGaugeHUD( new Vector2(posX, posY), new Vector2(tex.Width, tex.Height) )
			);
		}

		////

		public PressureGaugeHUD( Vector2 position, Vector2 dimensions ) : base(
					name: "PressureGauge",
					position: position,
					dimensions: dimensions ) {
		}


		protected override void DrawSelf( SpriteBatch sb ) {
			var mymod = SteamArseMod.Instance;
			Texture2D bg = mymod.GetTexture( "PressureGaugeBG" );
			Texture2D fg = mymod.GetTexture( "PressureGaugeFG" );
			Texture2D pin = mymod.GetTexture( "PressureGaugePin" );

			Vector2 pos = this.GetHUDComputedPosition( true );
			Vector2 pinOrigin = new Vector2(
				bg.Width / 2,
				bg.Width / 2
			);

			//

			sb.Draw(
				texture: bg,
				position: pos,
				color: Color.White
			);

			sb.Draw(
				texture: pin,
				position: pos,
				sourceRectangle: null,
				color: Color.White,
				rotation: Main.rand.NextFloat() * (float)Math.PI,
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
		}
	}
}
