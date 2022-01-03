using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SteampunkArsenal.Items.Accessories {
	[AutoloadEquip( EquipType.Waist )]
	class PortABoilerItem : ModItem {
		public Boiler MyBoiler { get; } = new TankBoiler();



		////////////////

		public override void SetStaticDefaults() {
			this.Tooltip.SetDefault(
				"Generates hot water for steam-powered machinery. Fun-sized."
			);
		}

		public override void SetDefaults() {
			this.item.width = 22;
			this.item.height = 22;
			this.item.accessory = true;
			this.item.rare = ItemRarityID.Orange;
			this.item.value = Item.sellPrice( 0, 15, 0, 0 );
		}
	}
}
