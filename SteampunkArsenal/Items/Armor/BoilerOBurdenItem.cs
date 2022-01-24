using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Recipes;
using SteampunkArsenal.Logic.Steam;
using SteampunkArsenal.Logic.Steam.SteamSources.Boilers;


namespace SteampunkArsenal.Items.Armor {
	[AutoloadEquip( EquipType.Body )]
	public class BoilerOBurdenItem : ModItem, ISteamContainerItem {
		//public Boiler MyBoiler { get; } = new TankBoiler( PlumbingType.Worn );
		public SteamSource SteamSupply { get; } = new TankBoiler( PlumbingType.Worn );



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Boiler-O-Burden" );
			this.Tooltip.SetDefault(
				"Heats water for steam-powered machinery. Travel-sized."
			);
		}

		public override void SetDefaults() {
			this.item.width = 22;
			this.item.height = 22;
			this.item.defense = 4;
			this.item.rare = ItemRarityID.LightRed;
			this.item.value = Item.sellPrice( 0, 5, 0, 0 );
		}


		////////////////

		public override void AddRecipes() {
			var recipe = new BoilerOBurdenRecipe( this );
			recipe.AddRecipe();
		}


		////////////////

		public override void UpdateEquip( Player player ) {
			float capacityUsePercent = this.SteamSupply.TotalPressure / this.SteamSupply.TotalCapacity;

			//

			//var myplayer = player.GetModPlayer<SteamArsePlayer>();
			//myplayer.CurrentBodyLayerShakeAmount = capacityUsePercent;

			//

			this.EmitSteam_If( player, capacityUsePercent );
		}


		////////////////

		public bool EmitSteam_If( Player player, float steamPercent ) {
			float rand = Main.rand.NextFloat();
			rand = 1.05f - (rand * rand * rand);

			if( rand > steamPercent ) {
				return false;
			}

			//

			Vector2 pos = player.MountedCenter;
			pos.X += player.direction >= 0
				? -12f
				: 12f;
			pos.Y += player.gravDir >= 0f
				? -12f
				: 12f;

			Vector2 vel = player.velocity * 0.5f;
			vel.X += player.direction >= 0
				? -1f
				: 1f;

			//

			Fx.CreateSmallSteamFx(
				position: pos,
				velocity: vel,
				dispersalRadius: 0f,
				velocityNoise: 3f,
				puffs: 1,
				scale: 0.3f
			);

			return true;
		}
	}
}
