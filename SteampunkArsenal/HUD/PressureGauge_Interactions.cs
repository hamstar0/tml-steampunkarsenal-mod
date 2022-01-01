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
using SteampunkArsenal.Items.Armor;


namespace SteampunkArsenal.HUD {
	partial class PressureGaugeHUD : HUDElement, ILoadable {
		public bool AttemptButtonPress() {
			Main.LocalPlayer.mouseInterface = true;

			//

			if( this.AnimState > 0 ) {
				return false;
			}

			Item boilerItem = Main.LocalPlayer.armor[1];
			var myboiler = boilerItem?.modItem as BoilerOBurdenItem;
			if( boilerItem?.active != true || boilerItem.type != ModContent.ItemType<BoilerOBurdenItem>() || myboiler == null ) {
				return false;
			}

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

			myboiler.MyBoiler.AddHeat( 1f, 100f );

			//

			Main.PlaySound( SoundID.Item111, Main.LocalPlayer.MountedCenter );

			return true;
		}
	}
}
