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
		private void DrawGauge( SpriteBatch sb, Vector2 pos ) {
			this.DrawGaugeBG( sb, pos );
			this.DrawGaugeLiquids( sb, pos );
			this.DrawGaugePins( sb, pos );
			this.DrawGaugeCurrentGelGauge( sb, pos );
		}


		////////////////
		
		private void DrawGaugeBG( SpriteBatch sb, Vector2 pos ) {
			var mymod = SteamArseMod.Instance;
			Texture2D bg = this.AnimState == 0
				? this.IsMouseHovering
					? mymod.GetTexture( "HUD/PressureGaugeBG_B" )
					: mymod.GetTexture( "HUD/PressureGaugeBG_A" )
				: mymod.GetTexture( "HUD/PressureGaugeBG_C" );

			//

			sb.Draw(
				texture: bg,
				position: pos,
				color: Color.White
			);
		}
	}
}
