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

			//

			this._Water -= this._WaterLeakPerTick;
			if( this._Water < 0f ) {
				this._Water = 0f;
			}
		}


		////

		internal protected override void PostUpdate( Player owner, bool isChild ) {
			if( !isChild ) {
				if( this.TotalPressure > this.TotalCapacity ) {
					this.EmitSteam( owner.MountedCenter, this.TotalPressure - this.TotalCapacity );
				}
			}
		}


		////////////////

		private void UpdateTemperatures() {
			var config = SteampunkArsenalConfig.Instance;
			float waterPercTempDrainRateS = config.Get<float>( nameof(config.WaterHeatPercentDecayRatePerSecond) );
			float boilerPercTempDrainRateS = config.Get<float>( nameof(config.BoilerHeatPercentDecayRatePerSecond) );
			float boilerTempXferRateS = config.Get<float>( nameof(config.BoilerWaterHeatXferRatePerSecond) );
			float waterPercTempDrainRateT = waterPercTempDrainRateS / 60f;
			float boilerPercTempDrainRateT = boilerPercTempDrainRateS / 60f;
			float boilerTempXferRateT = boilerTempXferRateS / 60f;

			// Slow drain temperature from water
			if( this._WaterHeat > 1f ) {
				this._WaterHeat -= this._WaterHeat * waterPercTempDrainRateT;
//if( float.IsNaN(this._WaterHeat) ) {
//	LogLibraries.LogOnce("NAN 1");
//}

				if( this._WaterHeat < 1f ) {
					this._WaterHeat = 1f;
				}
			}

			// Slow drain temperature from water
			if( this._BoilerHeat > 1f ) {
				this._BoilerHeat -= this._BoilerHeat * boilerPercTempDrainRateT;

				if( this._BoilerHeat < 1f ) {
					this._BoilerHeat = 1f;
				}
			}

			// Slow transfer boiler temperature to water
			if( this._WaterHeat < this._BoilerHeat ) {
				this._WaterHeat += boilerTempXferRateT;

				if( this._WaterHeat > this._BoilerHeat ) {
					this._WaterHeat = this._BoilerHeat;
				}

				if( this.TotalPressure > this.TotalCapacity ) {
					float excessPressure = this.TotalPressure - this.TotalCapacity;

					this._WaterHeat -= excessPressure / this._Water;
				}
//if( float.IsNaN(this._WaterHeat) ) {
//	LogLibraries.LogOnce("NAN 2");
//}
			}
		}
	}
}