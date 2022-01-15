using System;
using System.Collections.Generic;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources {
	public class SteamContainer : SteamSource {
		public override float Water => this._Water;

		public override float WaterHeat => this._WaterHeat;

		public override float SteamCapacity => this._Capacity;

		////

		public float HeatDecayRatePerTick { get; private set; }


		////////////////

		private float _Water = 0f;

		private float _WaterHeat = 1f;

		private float _Capacity = 100f;



		////////////////

		public SteamContainer( bool canConverge, float heatDecayRatePerTick ) : base( canConverge ) {
			this.HeatDecayRatePerTick = heatDecayRatePerTick;
		}



		////////////////

		public override float AddWater( float addedWater, float heatOfAddedWater, out float waterOverflow ) {
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

		public override float DrainWater( float waterDrained, out float waterUnderflow ) {
			float computedWaterDrained = SteamSource.CalculateWaterDrained(
				source: this,
				waterDrained: waterDrained,
				waterUnderflow: out waterUnderflow
			);

			this._Water -= computedWaterDrained;

			return computedWaterDrained;
		}


		////////////////

		public void TransferPressureToMeFromSourcesUntilFull( IEnumerable<SteamSource> sources ) {
			foreach( SteamSource steamSrc in sources ) {
				if( this.SteamPressure >= this.SteamCapacity ) {
					break;
				}

				//

				if( steamSrc.SteamPressure == 0f ) {
					continue;
				}

				//

				this.TransferPressureToMeFromSource(
					source: steamSrc,
					steam: steamSrc.SteamPressure,
					waterUnderflow: out _,
					waterOverflow: out float overflow
				);

				if( overflow > 0f ) {
					steamSrc.AddWater( overflow, steamSrc.WaterHeat, out _ );
				}
			}
		}


		////////////////

		protected internal override void PreUpdate( Player owner, bool isChild ) {
			if( this.WaterHeat > 1f ) {
				this._WaterHeat -= this.HeatDecayRatePerTick;

				if( this.WaterHeat < 1f ) {
					this._WaterHeat = 1f;
				}
			}
		}

		protected internal override void PostUpdate( Player owner, bool isChild ) { }
	}
}