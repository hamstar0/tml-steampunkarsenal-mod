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
			var posX = config.Get<float>( nameof( config.PressureGaugeHUDPositionX ) );
			var posY = config.Get<float>( nameof( config.PressureGaugeHUDPositionY ) );

			Texture2D tex = SteamArseMod.Instance.GetTexture( "HUD/PressureGaugeBG_A" );
			var hudElem = new PressureGaugeHUD( new Vector2( posX, posY ), new Vector2( tex.Width, tex.Height ) );

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



		////////////////
		
		private PressureGaugeHUD() : base( "PressureGaugeSingleton", default, default ) { }

		public PressureGaugeHUD( Vector2 position, Vector2 dimensions ) : base(
					name: "PressureGauge",
					position: position,
					dimensions: dimensions ) {
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
			return myplayer.MyBoiler?.IsActive ?? false;
		}


		////////////////

		protected override void PreUpdateWhileActive() {
			if( this.AnimState > 0 ) {
				this.AnimState--;
			}
		}
	}
}
