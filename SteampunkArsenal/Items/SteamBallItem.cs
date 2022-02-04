using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Logic.Steam;
using SteampunkArsenal.Logic.Steam.SteamSources;
using SteampunkArsenal.Recipes;


namespace SteampunkArsenal.Items {
	public class SteamBallItem : ModItem, ISteamContainerItem {
		public SteamSource SteamSupply { get; private set; }



		////////////////

		public override void SetStaticDefaults() {
			this.Tooltip.SetDefault( "Stores steam" );
		}

		public override void SetDefaults() {
			this.SteamSupply = new SteamContainer(
				plumbingType: PlumbingType.Inventory,
				heatPercentDecayRatePerTick: 0f,
				pressurePercentLeakPerTick: 0f,
				maxCapacity: 50f
			);

			this.item.scale = 0.25f;
			this.item.width = 18;
			this.item.height = 18;
			this.item.rare = ItemRarityID.LightPurple;
			this.item.value = Item.sellPrice( 0, 5, 0, 0 );
		}


		////////////////

		public override void AddRecipes() {
			var recipe = new SteamBallRecipe( this );
			recipe.AddRecipe();
		}
	}
}
