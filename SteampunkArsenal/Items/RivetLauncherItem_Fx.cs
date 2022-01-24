using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Timers;
using SteampunkArsenal.HUD;


namespace SteampunkArsenal.Items {
	public partial class RivetLauncherItem : ModItem {
		public const float MinPressureTransferSoundVolume = 0.05f;
		public const float MaxPressureTransferSoundVolume = 0.25f;

		public const float MinPressureIdleSoundVolume = 0.01f;
		public const float MaxPressureIdleSoundVolume = 0.1f;



		////////////////

		private void RunFx( Player wielderPlayer, bool isCharging ) {
			var myplayer = wielderPlayer.GetModPlayer<SteamArsePlayer>();
			float percent = this.SteamSupply.TotalPressure / this.SteamSupply.TotalCapacity;

			myplayer.CurrentBodyLayerShakeAmount = percent;

			//

			if( !isCharging ) {
				this.RunFx_Idle( wielderPlayer, percent );
			}

			//

			this.RunFx_Charging_State( isCharging, myplayer.ImplicitConvergingBoiler.SteamPressure );
		}

		////

		private void RunFx_Idle( Player wielderPlayer, float steamPercent ) {
			if( steamPercent > 0f ) {
				this.RunFx_Idle_State( steamPercent );
			}
		}

		private void RunFx_Idle_State( float steamPercent ) {
			if( steamPercent > 0f ) {
				float min = RivetLauncherItem.MinPressureIdleSoundVolume;
				float max = RivetLauncherItem.MaxPressureIdleSoundVolume;
				float volume = min + ((max - min) * steamPercent);

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

		private void RunFx_Charging_State( bool isCharging, float availableSteam ) {
			float perc = this.SteamSupply.TotalPressure / this.SteamSupply.TotalCapacity;

			//

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

				float volume = min + ((max - min) * perc);

				SteamArseMod.Instance.BoilerUpInst1.Volume = volume;

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

					if( perc >= 0.9999f ) {
						PressureGaugeHUD.DisplayAlertPopup( "Tank full", Color.Yellow );
					} else if( availableSteam <= 0.0001f ) {
						PressureGaugeHUD.DisplayAlertPopup( "No steam available", Color.Yellow );
					}
				}
			}
		}
	}
}