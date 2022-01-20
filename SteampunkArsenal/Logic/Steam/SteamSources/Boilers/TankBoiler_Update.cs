using System;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class TankBoiler : Boiler {
		internal protected override void PreUpdate( Player owner, bool isChild ) {
			if( owner.dead ) {
				this._Water = 0f;
				this._WaterHeat = 1f;
				this._BoilerHeat = 1f;

				return;
			}

			//

			this.UpdateTemperatures();
		}


		////

		internal protected override void PostUpdate( Player owner, bool isChild ) {
			if( !isChild ) {
				if( this.SteamPressure > this.SteamCapacity ) {
					this.EmitSteam( owner.MountedCenter, this.SteamPressure - this.SteamCapacity );
				}
			}
		}


		////////////////

		private void UpdateTemperatures() {
			var config = SteampunkArsenalConfig.Instance;
			float waterTempDrainRate = config.Get<float>( nameof(config.WaterTempDrainRatePerSecondPerTank) );
			float boilerTempDrainRate = config.Get<float>( nameof(config.BoilerTempDecayRatePerSecondPerTank) );
			float boilerTempXferRate = config.Get<float>( nameof(config.BoilerWaterTempXferRatePerSecondPerTank) );

			// Slow drain temperature from water
			if( this._WaterHeat > 1f ) {
				this._WaterHeat -= waterTempDrainRate / 60f;
//if( float.IsNaN(this._WaterHeat) ) {
//	LogLibraries.LogOnce("NAN 1");
//}

				if( this._WaterHeat < 1f ) {
					this._WaterHeat = 1f;
				}
			}

			// Slow drain temperature from water
			if( this._BoilerHeat > 1f ) {
				this._BoilerHeat -= boilerTempDrainRate / 60f;

				if( this._BoilerHeat < 1f ) {
					this._BoilerHeat = 1f;
				}
			}

			// Slow transfer boiler temperature to water
			if( this._WaterHeat < this._BoilerHeat ) {
				float rate = this._BoilerHeat * (boilerTempXferRate / 60f);

				this._WaterHeat += rate;

				if( this._WaterHeat > this._BoilerHeat ) {
					this._WaterHeat = this._BoilerHeat;
				}
//if( float.IsNaN(this._WaterHeat) ) {
//	LogLibraries.LogOnce("NAN 2");
//}
			}
		}
	}
}