using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Items.Armor;
using SteampunkArsenal.Items.Accessories;


namespace SteampunkArsenal.Recipes {
	public class PortABoilerRecipe : ModRecipe {
		public PortABoilerRecipe( PortABoilerItem resultItem ) : base( SteamArseMod.Instance ) {
			this.AddIngredient( ModContent.ItemType<BoilerOBurdenItem>(), 1 );
			this.AddIngredient( ItemID.InletPump, 1 );
			this.AddIngredient( ItemID.OutletPump, 1 );
			this.AddIngredient( ItemID.HellstoneBar, 5 );

			this.AddTile( TileID.Anvils );

			this.SetResult( resultItem );
		}


		public override bool RecipeAvailable() {
			var config = SteampunkArsenalConfig.Instance;

			return config.Get<bool>( nameof(config.PortABoilerRecipeEnabled ) );
			return config.Get<bool>( nameof(config.BoilerOBurdenRecipeEnabled) );
		}
	}
}