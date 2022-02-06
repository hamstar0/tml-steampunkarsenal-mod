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
		private void DrawGaugeLiquids( SpriteBatch sb, Vector2 pos ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<SteamArsePlayer>();
			Boiler myboiler = myplayer.ImplicitConvergingBoiler;

			//

			float waterPerc = myboiler.Water / myboiler.WaterCapacity;

			if( waterPerc > 0f ) {
				this.DrawGaugeWater( sb, pos, waterPerc );
			}

			//

			float nonSteamUsage = myboiler.WaterCapacity - myboiler.Water;
			float steamPerc = myboiler.SteamPressure / nonSteamUsage;

			if( steamPerc > 0f ) {
				this.DrawGaugeSteam( sb, pos, waterPerc, steamPerc );
			}
		}


		////////////////

		private void DrawGaugeWater( SpriteBatch sb, Vector2 pos, float waterPercent ) {
			var mymod = SteamArseMod.Instance;
			Texture2D waterTex = mymod.GetTexture( "HUD/PressureGaugeWater" );

			//

			float waterTexSpanHeight = waterTex.Height;

			int texHeight = (int)( waterPercent * waterTexSpanHeight );

			float texOffsetY = (1f - waterPercent) * waterTexSpanHeight;

			//

			var offset = new Vector2( 30f, 22f + texOffsetY );
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


		////////////////

		 private int _PrevFrame = 1;
		private void DrawGaugeSteam( SpriteBatch sb, Vector2 pos, float waterPercent, float steamPercent ) {
			int frame;
			do {
				frame = Main.rand.Next(6) + 1;
			} while( frame == this._PrevFrame );
			this._PrevFrame = frame;

			//

			Texture2D tex = SteamArseMod.Instance.GetTexture( "HUD/PressureGaugeSteam"+frame );
			var origin = new Vector2( tex.Width / 2, tex.Height / 2 );

			//

			Rectangle rect = new Rectangle(
				x: 0,
				y: 0,
				width: tex.Width,
				height: (int)((1f - waterPercent) * (float)tex.Height)
			);

			//

			sb.Draw(
				texture: tex,
				position: pos + new Vector2(30f, 22f) + origin,
				sourceRectangle: rect,
				color: Color.White * steamPercent,
				rotation: 0f,	//Main.rand.NextBool() ? 0f : MathHelper.Pi,
				origin: origin,
				scale: 1f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}
	}
}
