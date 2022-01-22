using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.HUD;


namespace SteampunkArsenal.Items {
	public partial class RivetLauncherItem : ModItem {
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

			if( steam > 0f ) {
				this.SteamSupply.DrainWater( this.SteamSupply.Water, out _ );

				//

				Fx.CreateSteamEruptionFx(
					position: position,
					dispersalRadius: 0f,
					steamAmount: 35f
				);

				//
				
				Main.PlaySound(
					this.mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/SteamHiss")
						.WithVolume(0.5f)
				);
			}

			//

			damage = (int)steam;

			return steam > 0f;
		}
	}
}