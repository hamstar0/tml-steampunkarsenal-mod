using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SteampunkArsenal.Items.Accessories {
	[AutoloadEquip( EquipType.Back )]
	class BoilerOBurdenItem : ModItem {
		private Boiler Boiler = new Boiler();



		////////////////

		public override void SetStaticDefaults() {
			// We can use vanilla language keys to copy the tooltip from HiveBackpack
			this.Tooltip.SetDefault(
				"Generates hot water for steam-powered machinery. Portable."
			);
		}

		public override void SetDefaults() {
			sbyte realBackSlot = this.item.backSlot;

			this.item.CloneDefaults( ItemID.HiveBackpack );
			this.item.value = Item.sellPrice( 0, 5, 0, 0 );
			// CloneDefaults will clear out the autoloaded Back slot, so we need to preserve it this way.
			this.item.backSlot = realBackSlot;
		}
	}
}
