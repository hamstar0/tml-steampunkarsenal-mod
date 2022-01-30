using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.HUD;


namespace SteampunkArsenal.Items {
	public partial class RivetLauncherItem : ModItem {
		public static float GetRiveterDamage( float maxCapacity, float capacityPercent ) {
			var config = SteampunkArsenalConfig.Instance;
			float dmgScale = config.Get<float>( nameof(config.RiveterDamagerPerPressureUnit) );

			return maxCapacity * capacityPercent * capacityPercent * dmgScale;
		}



		////////////////

		public override void ModifyWeaponDamage( Player player, ref float add, ref float mult, ref float flat ) {
			float steam = this.SteamSupply?.SteamPressure ?? 0f;

			if( !float.IsNaN(steam) && !float.IsInfinity(steam) ) {
				float capacity = this.SteamSupply.TotalCapacity;

				flat = RivetLauncherItem.GetRiveterDamage(
					maxCapacity: capacity,
					capacityPercent: steam / capacity
				);
			} else {
				flat = 0f;
			}
		}


		////////////////

		public override bool CanUseItem( Player player ) {
			float minSteam = 10f;

			if( this.SteamSupply.SteamPressure >= minSteam ) {
				return true;
			}

			PressureGaugeHUD.DisplayAlertPopup( "Needs steam", Color.Yellow );
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

			float drainedWater = this.SteamSupply.DrainWater_If( this.SteamSupply.Water, out _ );
			if( drainedWater <= 0f ) {
				Main.NewText( "Could not acquire steam.", Color.Yellow );
				return false;
			}

			//

			/*//var config = SteampunkArsenalConfig.Instance;
			//float dmgScale = config.Get<float>( nameof(config.RiveterDamagerPerPressureUnit) );

			//damage = (int)(totalPressure * dmgScale);
			damage = RivetLauncherItem.GetRiveterDamage( maxPressure, pressure / maxPressure );*/

			if( steam <= 0 ) {
				Main.NewText( "No steam available.", Color.Yellow );
			}

			//

			Fx.CreateSteamEruptionFx(
				position: position,
				dispersalRadius: 0f,
				steamAmount: 35f
			);

			Main.PlaySound(
				this.mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/SteamHiss")
					.WithVolume(0.25f)
			);

			//

			return steam > 0f;
		}
	}
}