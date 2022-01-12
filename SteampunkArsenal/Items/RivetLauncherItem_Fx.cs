using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Timers;


namespace SteampunkArsenal.Items {
	public partial class RivetLauncherItem : ModItem {
		public const float MinPressureTransferSoundVolume = 0.05f;
		public const float MaxPressureTransferSoundVolume = 0.35f;

		public const float MinPressureIdleSoundVolume = 0.01f;
		public const float MaxPressureIdleSoundVolume = 0.1f;



		////////////////

		private void RunFx_Idle() {
			float percent = this.SteamSupply.SteamPressure / this.SteamSupply.Capacity;

			if( percent > 0f ) {
				this.RunFx_Idle_State( percent );
			}
		}

		private void RunFx_Idle_State( float percent ) {
			if( percent > 0f ) {
				float min = RivetLauncherItem.MinPressureIdleSoundVolume;
				float max = RivetLauncherItem.MaxPressureIdleSoundVolume;
				float volume = min + ((max - min) * percent);

				SteamArseMod.Instance.BoilerUpInst2.Volume = volume;

				//

				if( SteamArseMod.Instance.BoilerUpInst2.State != SoundState.Playing ) {
					SteamArseMod.Instance.BoilerUpInst2.Play();
				}

				//

				// Cleanup
				Timers.SetTimer( "SteampunkRiveterIdle", 2, false, () => {
					if( SteamArseMod.Instance.BoilerUpInst2.State == SoundState.Playing ) {
						SteamArseMod.Instance.BoilerUpInst2.Stop();
					}

					return false;
				} );
			} else {
				if( SteamArseMod.Instance.BoilerUpInst2.State == SoundState.Playing ) {
					SteamArseMod.Instance.BoilerUpInst2.Stop();
				}
			}
		}


		////////////////

		private void RunFx_Charging_State( bool isCharging ) {
			if( isCharging ) {
				//if( Timers.GetTimerTickDuration("SteampunkRivetChargeSound") == 0 ) {
				//	Timers.SetTimer( "SteampunkRivetChargeSound", 5, false, () => false );

				//	SoundEffectInstance sndInst = SteamArseMod.Instance.BoilerUp.CreateInstance();
				//	sndInst.Volume = 0.1f;
				//	sndInst.Play();

				//	Main.PlaySound(
				//		SoundLoader.customSoundType,
				//		-1,
				//		-1,
				//		SteamArseMod.Instance.GetSoundSlot(SoundType.Custom, "Sounds/Custom/BoilerUp")
				//	);
				//}

				float min = RivetLauncherItem.MinPressureTransferSoundVolume;
				float max = RivetLauncherItem.MaxPressureTransferSoundVolume;
				float perc = this.SteamSupply.SteamPressure / this.SteamSupply.Capacity;
				perc = min + ((max - min) * perc);

				SteamArseMod.Instance.BoilerUpInst1.Volume = perc;

				if( SteamArseMod.Instance.BoilerUpInst1.State != SoundState.Playing ) {
					SteamArseMod.Instance.BoilerUpInst1.Play();
				}

				//

				Timers.SetTimer( "SteampunkRiveterChargeup", 2, false, () => {
					if( SteamArseMod.Instance.BoilerUpInst1.State == SoundState.Playing ) {
						SteamArseMod.Instance.BoilerUpInst1.Stop();
					}

					return false;
				} );
			} else {
				if( SteamArseMod.Instance.BoilerUpInst1.State == SoundState.Playing ) {
					SteamArseMod.Instance.BoilerUpInst1.Stop();
				}
			}
		}
	}
}