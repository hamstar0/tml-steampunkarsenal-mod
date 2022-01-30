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
using SteampunkArsenal.Logic.Steam.SteamSources;


namespace SteampunkArsenal.HUD {
	partial class PressureGaugeHUD : HUDElement, ILoadable {
		public const int FuelItemType = ItemID.Gel;
		


		////////////////

		public static bool CanAddFuel( Player player, Boiler boiler, out string result ) {
			if( player.dead ) {
				result = "Interaction disabled.";
				return false;
			}
			if( !boiler.IsActive ) {
				result = "No boiler.";
				return false;
			}

			//

			int totalFuel = player.inventory.Sum(
				i => i?.active == true && i.type == PressureGaugeHUD.FuelItemType
					? i.stack
					: 0
			);

			if( totalFuel < 1 ) {
				result = "Not enough fuel (gels).";
				return false;
			}

			result = "Success.";
			return true;
		}


		////////////////

		public bool AttemptButtonPress() {
			if( this.AnimState > 0 ) {
				return false;
			}

			Player player = Main.LocalPlayer;
			if( player.dead ) {
				return false;
			}

			Boiler boiler = player.GetModPlayer<SteamArsePlayer>().ImplicitConvergingBoiler;
			
			if( !PressureGaugeHUD.CanAddFuel(player, boiler, out string result) ) {
				PressureGaugeHUD.DisplayAlertPopup( result, Color.Yellow );

				return false;
			}

			//

			PlayerItemLibraries.RemoveInventoryItemQuantity( player, PressureGaugeHUD.FuelItemType, 1 );

			//

			float newTemp = boiler.BoilerHeat + 1f;
			
			if( boiler.SetBoilerHeat_If(newTemp) ) {
				PressureGaugeHUD.DisplayAlertPopup( "Fuel added (-1 gel)", Color.Lime );

				Main.PlaySound( SoundID.Item111, player.MountedCenter );

				//

				return true;
			}

			return false;
		}
	}
}
