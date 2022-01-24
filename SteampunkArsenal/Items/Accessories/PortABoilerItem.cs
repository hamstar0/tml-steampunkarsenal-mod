using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Logic.Steam;
using SteampunkArsenal.Logic.Steam.SteamSources.Boilers;
using SteampunkArsenal.Recipes;


namespace SteampunkArsenal.Items.Accessories {
	[AutoloadEquip( EquipType.Waist )]
	public class PortABoilerItem : ModItem, ISteamContainerItem {
		public SteamSource SteamSupply { get; } = new TankBoiler( PlumbingType.Worn );



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Port-A-Boiler" );
			this.Tooltip.SetDefault(
				"Heats water for steam-powered machinery. Fun-sized."
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
