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

			float waterPerc = myboiler.Water / myboiler.TotalCapacity;

			if( waterPerc > 0f ) {
				this.DrawGaugeWater( sb, pos, waterPerc );
			}

			//

			float waterPressure = myboiler.TotalPressure - myboiler.SteamPressure;
			float steamPerc = myboiler.SteamPressure / (myboiler.TotalCapacity - waterPressure);

			if( steamPerc > 0f ) {
				this.DrawGaugeSteam( sb, pos, waterPerc, steamPerc );
			}
		}


		////////////////

		private void DrawGaugeWater( SpriteBatch sb, Vector2 pos, float waterPercent ) {
			var mymod = SteamArseMod.Instance;
			Texture2D waterTex = mymod.GetTexture( "HUD/PressureGaugeWater" );

			//

			float waterTexSpanHeight = 50f;

			int texHeight = (int)( waterPercent * waterTexSpanHeight );

			float texOffsetY = 24f;
			texOffsetY += ( 1f - waterPercent ) * waterTexSpanHeight;

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


		////////////////

		 private int _SteamAnimationFrame = 1;

		private void DrawGaugeSteam( SpriteBatch sb, Vector2 pos, float waterPercent, float steamPercent ) {
			var mymod = SteamArseMod.Instance;
			Texture2D tex = mymod.GetTexture( "HUD/PressureGaugeSteam"+this._SteamAnimationFrame );

			//

			if( ++this._SteamAnimationFrame > 6 ) {
				this._SteamAnimationFrame = 1;
			}

			//

			float height = (float)tex.Height * waterPercent;
			height = 1f - height;

			Rectangle rect = new Rectangle(
				x: 0,
				y: 0,
				width: tex.Width,
				height: (int)height
			);

			//

			sb.Draw(
				texture: tex,
				position: pos,
				sourceRectangle: rect,
				color: Color.White * steamPercent * 0.75f,
				rotation: 0f,
				origin: default,
				scale: 1f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}
	}
}
