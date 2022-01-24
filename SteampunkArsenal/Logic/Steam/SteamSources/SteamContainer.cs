using System;
using System.Collections.Generic;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources {
	public class SteamContainer : SteamSource {
		public override float Water => this._Water;

		public override float WaterHeat => this._WaterHeat;

		public override float WaterLeakPerTick => this._WaterLeakPerTick;

		public override float TotalCapacity => this._Capacity;

		////

		public float HeatPercentDecayRatePerTick { get; private set; }


		////////////////

		private float _Water = 0f;

		private float _WaterHeat = 1f;

		private float _WaterLeakPerTick = 0f;

		private float _Capacity = 100f;



		////////////////

		public SteamContainer(
						PlumbingType plumbingType,
						float heatPercentDecayRatePerTick,
						float waterLeakPerTick )
					: base( plumbingType ) {
			this.HeatPercentDecayRatePerTick = heatPercentDecayRatePerTick;
			this._WaterLeakPerTick = waterLeakPerTick;
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

		public void TransferPressureToMeFromSourcesUntilFull_If( IEnumerable<SteamSource> sources ) {
			if( this.IsActive ) {
				this.TransferPressureToMeFromSourcesUntilFull( sources );
			}
		}

		private void TransferPressureToMeFromSourcesUntilFull( IEnumerable<SteamSource> sources ) {
			foreach( SteamSource steamSrc in sources ) {
				if( this.TotalPressure >= this.TotalCapacity ) {
					break;
				}

				//

				if( steamSrc.TotalPressure == 0f ) {
					continue;
				}

				//

				this.TransferContentsToMeFromSource_If(
					source: steamSrc,
					intendedContentsXferAmt: steamSrc.TotalPressure,
					waterUnderflow: out _,
					waterOverflow: out float overflow
				);

				if( overflow > 0f ) {
					steamSrc.AddWater_If( overflow, steamSrc.WaterHeat, out _ );
				}
			}
		}


		////////////////

		protected internal override void PreUpdate( Player owner, bool isChild ) {
			if( this.WaterHeat > 1f ) {
				this._WaterHeat -= this._WaterHeat * this.HeatPercentDecayRatePerTick;

				if( this.WaterHeat < 1f ) {
					this._WaterHeat = 1f;
				}
			}

			this._Water -= this._WaterLeakPerTick;
			if( this._Water < 0f ) {
				this._Water = 0f;
			}
		}

		protected internal override void PostUpdate( Player owner, bool isChild ) { }
	}
}