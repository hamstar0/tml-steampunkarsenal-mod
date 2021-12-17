using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Recipes;
using SteampunkArsenal.Projectiles;


namespace SteampunkArsenal.Items {
	public partial class SteamPoweredRivetLauncherItem : ModItem {
		internal SteamPressureSource Boiler { get; private set; } = new SteamPressureSource();



		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Steam-Driven Rivet Launcher" );
			this.Tooltip.SetDefault(
				"Fires a high velocity, penetrating rivet"
				+"\nGood for pinning objects together"
				+"\nRequires steam pressure to launch rivets"
				+"\nHold right-click to pressurize the unit (requires a boiler or steam ball)"
				+"\nStored pressure is released when rivet launches or unit re-pressurizes"
				+"\nWarning: Over-pressurizing unit may be hazardous"
			);
		}

		public override void SetDefaults() {
			this.item.ranged = true;
			this.item.autoReuse = false;

			this.item.width = 40;
			this.item.height = 20;

			this.item.damage = 1;
			this.item.knockBack = 1f;

			this.item.shoot = ModContent.ProjectileType<RivetProjectile>();
			this.item.shootSpeed = 16f;

			this.item.useTime = 20;
			this.item.useAnimation = 20;
			this.item.useStyle = ItemUseStyleID.HoldingOut;

			// 38: Distant shotgun
			// 61: Mechanical Kerchunk
			// 89: Air-pressured shot
			// 98-99: Mechanical shot
			// 110: Distant shot
			this.item.UseSound = SoundID.Item61;

			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
			this.item.rare = ItemRarityID.LightRed;
		}


		////////////////

		public override void ModifyWeaponDamage( Player player, ref float add, ref float mult, ref float flat ) {
			flat = this.Boiler.SteamPressure;
		}


		////////////////

		public override void AddRecipes() {
			var recipe = new SteamPoweredRivetLauncherRecipe( this );

			recipe.AddRecipe();
		}
	}
}