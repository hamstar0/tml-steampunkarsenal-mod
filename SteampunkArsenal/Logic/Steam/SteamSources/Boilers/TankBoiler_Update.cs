using System;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class TankBoiler : Boiler {
		internal protected override void PreUpdate( Player owner, bool isChild ) {
			if( owner.dead ) {
				this._Water = 0f;
				this._WaterTemperature = 1f;
				this._BoilerTemperature = 1f;

				return;
			}

			//

			this.UpdateTemperatures();

			//

			float addedTemp = this.Water > 0f
				? this.BoilerTemperature / this.Water   // more water? more heat needed!
				: 0f;

			this._WaterTemperature += addedTemp;
		}


		////

		internal protected override void PostUpdate( Player owner, bool isChild ) {
			if( !isChild ) {
				if( this.SteamPressure > this.Capacity ) {
					this.EmitSteam( owner.MountedCenter, this.SteamPressure - this.Capacity );
				}
			}
		}


		////////////////

		private void UpdateTemperatures() {
			var config = SteampunkArsenalConfig.Instance;
			float boilerTempXferBaseRate = config.Get<float>( nameof(config.BoilerWaterTempIncreaseRatePerHeatUnitPerSecondPerTank) );
			float waterTempDrainRate = config.Get<float>( nameof(config.WaterTempDrainRatePerSecondPerTank) );
			float boilerTempDrainRate = config.Get<float>( nameof(config.BoilerTempDrainRatePerSecondPerTank) );

			// Slow drain temperature from water
			if( this._WaterTemperature > 1f ) {
				this._WaterTemperature -= waterTempDrainRate / 60f;

				if( this._WaterTemperature < 1f ) {
					this._WaterTemperature = 1f;
				}
			}

			// Slow transfer boiler temperature to water
			if( this._WaterTemperature > this._BoilerTemperature ) {
				float rate = this._BoilerTemperature * (boilerTempXferBaseRate / 60f);

				this._WaterTemperature += rate;

				if( this._WaterTemperature > this._BoilerTemperature ) {
					this._WaterTemperature = this._BoilerTemperature;
				}
			}

			// Slow drain temperature from water
			if( this._BoilerTemperature > 1f ) {
				this._BoilerTemperature -= boilerTempDrainRate / 60f;

				if( this._BoilerTemperature < 1f ) {
					this._BoilerTemperature = 1f;
				}
			}
		}
	}
}