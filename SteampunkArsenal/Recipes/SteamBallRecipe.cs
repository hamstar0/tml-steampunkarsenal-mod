using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Items;
using SteampunkArsenal.Items.Armor;


namespace SteampunkArsenal.Recipes {
	public class SteamBallRecipe : ModRecipe {
		public SteamBallRecipe( SteamBallItem resultItem ) : base( SteamArseMod.Instance ) {
			var ucMod = ModLoader.GetMod( "ModLibsUtilityContent" );
			if( ucMod != null ) {
				this.AddIngredient( SteamPoweredRivetLauncherRecipe.GetMagitech_WeakRef(), 1 );
			} else {
				this.AddRecipeGroup( "SteampunkArsenal:AdvWatches", 1 );
			}

			this.AddIngredient( ItemID.InletPump, 1 );
			this.AddRecipeGroup( "SteampunkArsenal:CopperTierBars", 5 );
			this.AddRecipeGroup( "SteampunkArsenal:IronTierBars", 5 );
			this.AddIngredient( ItemID.LavaBucket, 1 );

			this.AddTile( TileID.Anvils );

			this.SetResult( resultItem );
		}


		public override bool RecipeAvailable() {
			var config = SteampunkArsenalConfig.Instance;

			return config.Get<bool>( nameof(config.SteamBallRecipeEnabled ) );
		}
	}
}