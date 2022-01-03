using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Players;
using HUDElementsLib;
using SteampunkArsenal.Logic;


namespace SteampunkArsenal.HUD {
	partial class PressureGaugeHUD : HUDElement, ILoadable {
		public bool AttemptButtonPress() {
			Main.LocalPlayer.mouseInterface = true;

			//

			if( this.AnimState > 0 ) {
				return false;
			}

			Boiler boiler = Main.LocalPlayer.GetModPlayer<SteamArsePlayer>().MyBoiler;
			if( !boiler.IsActive ) {
				Main.NewText( "No boiler.", Color.Yellow );

				return false;
			}

			//

			int fuelType = ItemID.Gel;
			int fuelAmt = 1;

			int totalFuel = Main.LocalPlayer.inventory.Sum(
				i => i?.active == true && i.type == fuelType
					? i.stack
					: 0
			);

			if( totalFuel < fuelAmt ) {
				Main.NewText( "Not enough fuel (gels).", Color.Yellow );

				return false;
			}

			//
			
			PlayerItemLibraries.RemoveInventoryItemQuantity( Main.LocalPlayer, fuelType, fuelAmt );

			//

			float newTemp = boiler.BoilerTemperature + 1f;
			boiler.SetBoilerHeat( newTemp );

			//

			Main.PlaySound( SoundID.Item111, Main.LocalPlayer.MountedCenter );

			return true;
		}
	}
}
