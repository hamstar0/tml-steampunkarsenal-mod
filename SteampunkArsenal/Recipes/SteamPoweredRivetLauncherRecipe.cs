using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Items;


namespace SteampunkArsenal.Recipes {
	public class SteamPoweredRivetLauncherRecipe : ModRecipe {
		public static int GetMagitech_WeakRef() {
			return ModContent.ItemType<ModLibsUtilityContent.Items.MagitechScrapItem>();
		}


		////////////////

		public SteamPoweredRivetLauncherRecipe( RivetLauncherItem resultItem ) : base( SteamArseMod.Instance ) {
			var ucMod = ModLoader.GetMod( "ModLibsUtilityContent" );
			if( ucMod != null ) {
				//this.AddIngredient( ucMod.GetItem("MagitechScrapItem"), 1 );
				this.AddIngredient( SteamPoweredRivetLauncherRecipe.GetMagitech_WeakRef(), 1 );
			} else {
				this.AddRecipeGroup( "SteampunkArsenal:AdvWatches", 1 );
			}

			this.AddRecipeGroup( "SteampunkArsenal:CopperTierBars", 10 );
			this.AddRecipeGroup( "SteampunkArsenal:IronTierBars", 10 );
			//this.AddIngredient( ItemID.CopperBar, 10 );
			//this.AddIngredient( ItemID.IronBar, 10 );
			this.AddIngredient( ItemID.IllegalGunParts, 1 );

			this.AddTile( TileID.Anvils );

			this.SetResult( resultItem );
		}


		////

		public override bool RecipeAvailable() {
			var config = SteampunkArsenalConfig.Instance;

			return config.Get<bool>( nameof(config.RivetLauncherRecipeEnabled) );
		}
	}
}