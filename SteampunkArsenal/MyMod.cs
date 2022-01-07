using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Services.ProjectileOwner;
using SteampunkArsenal.Items;
using SteampunkArsenal.Projectiles;


namespace SteampunkArsenal {
	public class SteamArseMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-steampunkarsenal-mod";



		////////////////
		
		public static SteamArseMod Instance => ModContent.GetInstance<SteamArseMod>();



		////////////////

		public SoundEffectInstance WaterDraw;



		////////////////

		public override void PostSetupContent() {
			this.WaterDraw = Main.soundLiquid[1].CreateInstance();

			//

			ProjectileOwner.AddOwnerSetHook( ( projectileIdx, isManual ) => {
				if( !isManual ) {
					Projectile proj = Main.projectile[ projectileIdx ];

					if( proj?.active == true && proj.type == ModContent.ItemType<RivetLauncherItem>() ) {
						RivetProjectile.ApplyRivetStats_LocalOnly( projectileIdx, proj );
					}
				}
			} );
		}


		////////////////

		public override void AddRecipeGroups() {
			var advWatches = new RecipeGroup( () => "Gold or Platinum Watch", ItemID.GoldWatch, ItemID.PlatinumWatch );
			var copperTierBars = new RecipeGroup( () => "Copper or Tin Bar", ItemID.CopperBar, ItemID.TinBar );
			var ironTierBars = new RecipeGroup( () => "Iron or Lead Bar", ItemID.IronBar, ItemID.LeadBar );

			RecipeGroup.RegisterGroup( "SteampunkArsenal:AdvWatches", advWatches );
			RecipeGroup.RegisterGroup( "SteampunkArsenal:CopperTierBars", copperTierBars );
			RecipeGroup.RegisterGroup( "SteampunkArsenal:IronTierBars", ironTierBars );
		}
	}
}