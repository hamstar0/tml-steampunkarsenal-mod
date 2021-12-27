using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Items.Armor;


namespace SteampunkArsenal.Recipes {
	public class BoilerOBurdenRecipe : ModRecipe {
		public BoilerOBurdenRecipe( BoilerOBurdenItem resultItem ) : base( SteamArseMod.Instance ) {
			this.AddRecipeGroup( "SteampunkArsenal:CopperTierBars", 10 );
			this.AddRecipeGroup( "SteampunkArsenal:IronTierBars", 10 );
			//this.AddIngredient( ItemID.CopperBar, 10 );
			//this.AddIngredient( ItemID.IronBar, 10 );
			this.AddIngredient( ItemID.RocketBoots, 1 );

			this.AddTile( TileID.Anvils );

			this.SetResult( resultItem );
		}


		public override bool RecipeAvailable() {
			var config = SteampunkArsenalConfig.Instance;

			return config.Get<bool>( nameof(config.BoilerOBurdenRecipeEnabled) );
		}
	}
}