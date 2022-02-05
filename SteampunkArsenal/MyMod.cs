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

		public SoundEffectInstance WaterDraw = null;
		public SoundEffectInstance SteamHissLoop = null;

		public SoundEffectInstance BoilerUpInst1 = null;
		public SoundEffectInstance BoilerUpInst2 = null;



		////////////////

		public override void PostSetupContent() {
			if( Main.netMode != NetmodeID.Server && !Main.dedServ ) {
				SoundEffect boilerUpSFX = this.GetSound( "Sounds/Custom/BoilerUp" );
				SoundEffect steamHissSFX = this.GetSound( "Sounds/Custom/SteamHissLoop" );

				this.WaterDraw = Main.soundLiquid[1].CreateInstance();
				this.BoilerUpInst1 = boilerUpSFX.CreateInstance();
				this.BoilerUpInst2 = boilerUpSFX.CreateInstance();

				this.SteamHissLoop = steamHissSFX.CreateInstance();

				//

				this.WaterDraw.IsLooped = true;
				this.BoilerUpInst1.IsLooped = true;
				this.BoilerUpInst2.IsLooped = true;
				this.SteamHissLoop.IsLooped = true;
			}

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