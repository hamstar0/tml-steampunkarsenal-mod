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
		public static PressureGaugeHUD Instance { get; private set; }



		////////////////

		private static PressureGaugeHUD InitializePressureGaugeHUD() {
			var config = SteampunkArsenalConfig.Instance;


			var posOffset = new Vector2(
				config.Get<float>( nameof(config.PressureGaugeInitialHUDPositionX) ),
				config.Get<float>( nameof(config.PressureGaugeInitialHUDPositionY) )
			);
			var posPerc = new Vector2(
				posOffset.X < 0f ? 1f : 0f,
				posOffset.Y < 0f ? 1f : 0f
			);

			Texture2D tex = SteamArseMod.Instance.GetTexture( "HUD/PressureGaugeBG_A" );
			var hudElem = new PressureGaugeHUD( posOffset, posPerc, new Vector2( tex.Width, tex.Height ) );

			hudElem.OnClick += ( evt, listeningElement ) => {
				if( hudElem.AttemptButtonPress() ) {
					hudElem.AnimState = 15;
				}
			};

			HUDElementsLibAPI.AddWidget( hudElem );

			return hudElem;
		}



		////////////////
		
		private int AnimState = 0;

		////

		private int PopupTextElapsed = 0;
		private string PopupText = "";
		private Color PopupTextColor = default;

		////

		private bool IsHoveringSinceLastCheck = false;



		////////////////

		private PressureGaugeHUD() : base( "PressureGaugeSingleton", default, default, default, () => true ) { }

		public PressureGaugeHUD( Vector2 positionOffset, Vector2 positionPercent, Vector2 dimensions ) : base(
					name: "PressureGauge",
					positionOffset: positionOffset,
					positionPercent: positionPercent,
					dimensions: dimensions,
					enabler: () => true ) {
		}


		////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() {
			PressureGaugeHUD.Instance = null;
		}

		void ILoadable.OnPostModsLoad() {
			if( !Main.dedServ && Main.netMode != NetmodeID.Server ) {
				PressureGaugeHUD.Instance = PressureGaugeHUD.InitializePressureGaugeHUD();
			}
		}


		////////////////
		
		public override bool IsEnabled() {
			var myplayer = Main.LocalPlayer.GetModPlayer<SteamArsePlayer>();
			bool isEnabled = myplayer.ImplicitConvergingBoiler?.IsActive ?? false;

			//

			// Failsafe
			if( !isEnabled ) {
				if( this.IsHoveringSinceLastCheck ) {
					this.IsHoveringSinceLastCheck = false;

					Main.LocalPlayer.mouseInterface = false;
				}
			}

			//

			return isEnabled;
		}


		////////////////

		protected override void PreUpdateWhileActive() {
			if( this.IsMouseHovering ) {
				this.IsHoveringSinceLastCheck = true;

				Main.LocalPlayer.mouseInterface = true;
			} else {
				if( this.IsHoveringSinceLastCheck ) {
					this.IsHoveringSinceLastCheck = false;

					Main.LocalPlayer.mouseInterface = false;
				}
			}

			//

			if( this.AnimState > 0 ) {
				this.AnimState--;
			}
		}
	}
}
