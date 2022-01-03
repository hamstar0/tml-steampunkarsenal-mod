﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Logic.Steam.SteamSources;
using SteampunkArsenal.Logic.Steam.SteamSources.Boilers;
using SteampunkArsenal.Recipes;


namespace SteampunkArsenal.Items.Accessories {
	[AutoloadEquip( EquipType.Waist )]
	public class PortABoilerItem : ModItem {
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
			this.item.rare = ItemRarityID.Lime;
			this.item.value = Item.sellPrice( 0, 7, 0, 0 );
		}


		////////////////

		public override void AddRecipes() {
			var recipe = new PortABoilerRecipe( this );
			recipe.AddRecipe();
		}
	}
}
