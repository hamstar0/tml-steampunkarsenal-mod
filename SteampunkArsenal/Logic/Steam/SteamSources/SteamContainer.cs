using System;
using Terraria;
using Terraria.ModLoader.IO;


namespace SteampunkArsenal.Logic.Steam.SteamSources {
	public class SteamContainer : SteamSource {
		public override float Water => this._Water;

		public override float WaterHeat => this._WaterHeat;

		public override float PressurePercentLeakPerTick => this._PressurePercentLeakPerTick;

		public override float TotalCapacity => this._Capacity;

		public override float SteamPressure => this.TotalPressure;

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

		internal protected override bool CanSave() {
			return true;
		}

		internal protected override void Load( TagCompound tag ) {
			this._Water = tag.GetFloat( "water" );
			this._WaterHeat = tag.GetFloat( "water_heat" );
		}

		internal protected override TagCompound Save() {
			return new TagCompound {
				{ "water", this.Water },
				{ "water_heat", this.WaterHeat },
			};
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