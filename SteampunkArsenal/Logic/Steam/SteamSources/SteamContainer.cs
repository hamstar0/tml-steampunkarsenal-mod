using System;
using System.Collections.Generic;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources {
	public class SteamContainer : SteamSource {
		public override float Water => this._Water;

		public override float WaterTemperature => this._WaterTemperature;

		public override float Capacity => this._Capacity;

		////

		public float TemperatureDecayRatePerTick { get; private set; }


		////////////////

		private float _Water = 0f;

		private float _WaterTemperature = 1f;

		private float _Capacity = 100f;



		////////////////

		public SteamContainer( bool canConverge, float temperatureDecayRatePerTick ) : base( canConverge ) {
			this.TemperatureDecayRatePerTick = temperatureDecayRatePerTick;
		}



		////////////////

		public override float AddWater( float waterAmount, float heatAmount, out float waterOverflow ) {
			(float addedWater, float newHeat) = SteamSource.CalculateWaterAdded(
				destination: this,
				addedWaterAmount: waterAmount,
				addedWaterHeatAmount: heatAmount,
				waterOverflow: out waterOverflow
			);

			this._Water += addedWater;

			if( addedWater > 0f ) {
				this._WaterTemperature = newHeat;
			}

			return addedWater;
		}

		public override float DrainWater( float waterDrainAmount, out float waterUnderflow ) {
			float drainedWater = SteamSource.CalculateWaterDrained(
				source: this,
				waterDrainAmount: waterDrainAmount,
				waterUnderflow: out waterUnderflow
			);

			this._Water -= drainedWater;

			return drainedWater;
		}


		////////////////

		public void TransferPressureToMeFromSourcesUntilFull( IEnumerable<SteamSource> sources ) {
			foreach( SteamSource steamSrc in sources ) {
				if( this.SteamPressure >= this.Capacity ) {
					break;
				}

				//

				if( steamSrc.SteamPressure == 0f ) {
					continue;
				}

				//

				this.TransferPressureToMeFromSource( steamSrc, steamSrc.SteamPressure, out float overflow );

				if( overflow > 0f ) {
					steamSrc.AddWater( overflow, steamSrc.WaterTemperature, out _ );
				}
			}
		}


		////////////////

		protected internal override void PreUpdate( Player owner, bool isChild ) {
			if( this.WaterTemperature > 1f ) {
				this._WaterTemperature -= this.TemperatureDecayRatePerTick;

				if( this.WaterTemperature < 1f ) {
					this._WaterTemperature = 1f;
				}
			}
		}

		protected internal override void PostUpdate( Player owner, bool isChild ) { }
	}
}