﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Recipes;


namespace SteampunkArsenal.Items.Armor {
	[AutoloadEquip( EquipType.Body )]
	public class BoilerOBurdenItem : ModItem {
		public Boiler MyBoiler { get; } = new Boiler();



		////////////////

		public override void SetStaticDefaults() {
			base.SetStaticDefaults();

			// We can use vanilla language keys to copy the tooltip from HiveBackpack
			this.Tooltip.SetDefault(
				"Generates hot water for steam-powered machinery. Portable."
			);
		}

		public override void SetDefaults() {
			this.item.width = 22;
			this.item.height = 22;
			this.item.rare = ItemRarityID.Orange;
			this.item.value = Item.sellPrice( 0, 5, 0, 0 );
		}


		////////////////

		public override void AddRecipes() {
			var recipe = new BoilerOBurdenRecipe( this );
			recipe.AddRecipe();
		}
	}
}