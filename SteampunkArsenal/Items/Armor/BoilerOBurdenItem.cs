using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Recipes;


namespace SteampunkArsenal.Items.Armor {
	[AutoloadEquip( EquipType.Body )]
	public class BoilerOBurdenItem : ModItem {
		public Boiler MyBoiler { get; } = new TankBoiler();



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Boiler-O-Burden" );

			// We can use vanilla language keys to copy the tooltip from HiveBackpack
			this.Tooltip.SetDefault(
				"Generates hot water for steam-powered machinery. Travel-sized."
			);
		}

		public override void SetDefaults() {
			this.item.width = 22;
			this.item.height = 22;
			this.item.defense = 4;
			this.item.rare = ItemRarityID.LightRed;
			this.item.value = Item.sellPrice( 0, 5, 0, 0 );
		}


		////////////////

		public override void AddRecipes() {
			var recipe = new BoilerOBurdenRecipe( this );
			recipe.AddRecipe();
		}
	}
}
