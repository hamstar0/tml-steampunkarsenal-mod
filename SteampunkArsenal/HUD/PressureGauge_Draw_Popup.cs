using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using HUDElementsLib;


namespace SteampunkArsenal.HUD {
	partial class PressureGaugeHUD : HUDElement, ILoadable {
		public static void DisplayAlertPopup( string text, Color color ) {
			var instance = PressureGaugeHUD.Instance;
			
			instance.PopupTextElapsed = 1;
			instance.PopupText = text;
			instance.PopupTextColor = color;
		}



		////////////////

		public static bool DrawPopupText_If( SpriteBatch sb, Vector2 pos, string text, Color color, int ticksElapsed ) {
			float percent = (float)ticksElapsed / 45f;
			if( percent >= 1f ) {
				return false;
			}

			//

			var offset = new Vector2( 0f, -32f * percent );

			color *= 1f - percent;

			//

			Vector2 textDim = Main.fontMouseText.MeasureString( text );
			if( pos.X + textDim.X > Main.screenWidth ) {
				pos.X = Main.screenWidth - textDim.X;
			}

			//
			
			sb.DrawString(
				spriteFont: Main.fontMouseText,
				text: text,
				position: pos + offset,
				color: color
			);

			//

			return true;
		}
	}
}
