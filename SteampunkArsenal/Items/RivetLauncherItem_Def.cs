using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Logic.Steam.SteamSources;
using SteampunkArsenal.HUD;
using SteampunkArsenal.Recipes;
using SteampunkArsenal.Projectiles;


namespace SteampunkArsenal.Items {
	public partial class RivetLauncherItem : ModItem {
		internal SteamContainer SteamSupply { get; private set; }



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
			var config = SteampunkArsenalConfig.Instance;
			this.SteamSupply = new SteamContainer( false, config.Get<float>( nameof(config.BoilerTempDrainRatePerSecondPerTank) ) );

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

		public override void AddRecipes() {
			var recipe = new SteamPoweredRivetLauncherRecipe( this );
			recipe.AddRecipe();
		}


		////////////////

		public override void ModifyWeaponDamage( Player player, ref float add, ref float mult, ref float flat ) {
			float steam = this.SteamSupply?.SteamPressure ?? 0f;

			if( !float.IsNaN(steam) && !float.IsInfinity(steam) ) {
				flat = steam;
			} else {
				flat = 0f;
			}
		}


		////////////////

		public override bool CanUseItem( Player player ) {
			if( this.SteamSupply.SteamPressure >= 10f ) {
				return true;
			}

			PressureGaugeHUD.DisplayAlertPopup( "Needs steam.", Color.Yellow );
			return false;
		}


		////////////////

		public override bool Shoot(
					Player player,
					ref Vector2 position,
					ref float speedX,
					ref float speedY,
					ref int type,
					ref int damage,
					ref float knockBack ) {
			float steam = this.SteamSupply.SteamPressure;

			if( steam > 0f ) {
				this.SteamSupply.DrainWater( this.SteamSupply.Water, out _ );
			}

			//

			damage = (int)steam;

			return steam > 0f;
		}
	}
}