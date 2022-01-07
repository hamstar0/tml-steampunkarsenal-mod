using System;
using System.Collections.Generic;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources {
	public class SteamContainer : SteamSource {
		public override float Water => this._Water;

		public override float WaterTemperature => this._WaterTemperature;

		public override float Capacity => this._Capacity;


		////////////////

		private float _Water = 0f;

		private float _WaterTemperature = 1f;

		private float _Capacity = 100f;



		////////////////

		public override float AddWater( float waterAmount, float heatAmount, out float waterOverflow ) {
			(float addedWater, float addedHeat) = SteamSource.CalculateWaterAdded(
				source: this,
				addedWaterAmount: waterAmount,
				addedWaterHeatAmount: heatAmount,
				waterOverflow: out waterOverflow
			);

			this._Water += addedWater;

			if( addedWater > 0f ) {
				this._WaterTemperature += addedHeat;
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
	}
}