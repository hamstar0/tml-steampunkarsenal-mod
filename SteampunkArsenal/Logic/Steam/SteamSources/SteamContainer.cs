using System;
using System.Collections.Generic;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources {
	public class SteamContainer : SteamSource {
		public override float Water => this._Water;

		public override float WaterHeat => this._WaterHeat;

		public override float PressurePercentLeakPerTick => this._PressurePercentLeakPerTick;

		public override float TotalCapacity => this._Capacity;

		////

		public float HeatPercentDecayRatePerTick { get; private set; }


		////////////////

		private float _Water = 0f;

		private float _WaterHeat = 1f;

		private float _PressurePercentLeakPerTick = 0f;

		private float _Capacity = 100f;



		////////////////

		public SteamContainer(
						PlumbingType plumbingType,
						float heatPercentDecayRatePerTick,
						float pressurePercentLeakPerTick )
					: base( plumbingType ) {
			this.HeatPercentDecayRatePerTick = heatPercentDecayRatePerTick;
			this._PressurePercentLeakPerTick = pressurePercentLeakPerTick;
		}



		////////////////

		protected override float AddWater( float addedWater, float heatOfAddedWater, out float waterOverflow ) {
			(float computedAddedWater, float addedHeat) = SteamSource.CalculateWaterAdded(
				destination: this,
				addedWater: addedWater,
				heatOfAddedWater: heatOfAddedWater,
				waterOverflow: out waterOverflow
			);

			this._Water += computedAddedWater;
			this._WaterHeat += addedHeat;

			return computedAddedWater;
		}

		protected override float DrainWater( float waterDrained, out float waterUnderflow ) {
			float computedWaterDrained = SteamSource.CalculateWaterDrained(
				source: this,
				waterDrained: waterDrained,
				waterUnderflow: out waterUnderflow
			);

			this._Water -= computedWaterDrained;

			return computedWaterDrained;
		}


		////////////////

		protected internal override void PreUpdate( Player owner, bool isChild ) {
			if( this.WaterHeat > 1f ) {
				this._WaterHeat -= this._WaterHeat * this.HeatPercentDecayRatePerTick;

				if( this.WaterHeat < 1f ) {
					this._WaterHeat = 1f;
				}
			}

			this._Water -= (this._Water * this._PressurePercentLeakPerTick) / this._WaterHeat;
		}

		protected internal override void PostUpdate( Player owner, bool isChild ) { }
	}
}