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

			float shake = Main.LocalPlayer
				.GetModPlayer<SteamArsePlayer>()
				.CurrentBodyLayerShakePercent;

			var shakeOffset = new Vector2(
				(Main.rand.NextFloat() * shake * 2f) - 1f,
				(Main.rand.NextFloat() * shake * 2f) - 1f
			);

			//

			this.DrawGauge( sb, pos + shakeOffset );

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
	}
}
