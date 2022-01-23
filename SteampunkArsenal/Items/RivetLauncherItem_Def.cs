using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Logic.Steam.SteamSources;
using SteampunkArsenal.Recipes;
using SteampunkArsenal.Projectiles;
using SteampunkArsenal.Net;


namespace SteampunkArsenal.Items {
	public partial class RivetLauncherItem : ModItem {
		internal void UpdateLaunchedRivetProjectileStats_NonServer_Syncs( int projectileIdx, Projectile projectile ) {
			if( Main.netMode == NetmodeID.Server ) {
				return;
			}

			float pressure = this.SteamSupply.TotalPressure;

			projectile.damage = (int)pressure;

			//

			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				ProjectileDamageSyncProtocol.BroadcastFromClientToAll( projectileIdx, (int)pressure );
			}
		}


		////////////////

		internal SteamContainer SteamSupply { get; private set; }


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Steam-Driven Rivet Launcher" );
			this.Tooltip.SetDefault(
				"Fires a high velocity, penetrating rivet"
				+"\nMay pinning objects to nearby surfaces"
				+"\nRequires steam pressure to launch rivets"
				+"\nHold right-click to pressurize unit (requires a steam source)"
				+"\nWarning: Over-pressurization may be hazardous!"
			);
		}

		public override void SetDefaults() {
			var config = SteampunkArsenalConfig.Instance;

			float decayRate = config.Get<float>( nameof(config.RiveterPressureDecayRatePerSecond) );

			//
			
			this.SteamSupply = new SteamContainer( false, 0f, decayRate / 60f );

			//

			this.item.ranged = true;
			this.item.autoReuse = false;

			this.item.width = 38;
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

		////

		public override ModItem NewInstance( Item itemClone ) {
			var myitem = (RivetLauncherItem)base.NewInstance( itemClone );
			myitem.SteamSupply = this.SteamSupply;

			return myitem;
		}


		////////////////

		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			int idx = tooltips.FindIndex( t => t.Name == "Damage" );

			if( idx != -1 ) {
				string[] segs = tooltips[idx].text.Split( ' ' );

				segs[0] = ((int)(this.SteamSupply?.TotalPressure ?? 1f)).ToString();

				tooltips[idx].text = string.Join( " ", segs )
					+ " (requires steam)";
			}
		}


		////////////////

		public override void AddRecipes() {
			var recipe = new SteamPoweredRivetLauncherRecipe( this );
			recipe.AddRecipe();
		}


		////////////////

		public override void UpdateInventory( Player player ) {
			this.SteamSupply.PreUpdate( player, false );
			this.SteamSupply.PostUpdate( player, false );
		}
	}
}