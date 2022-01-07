using System;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class TankBoiler : Boiler {
		public override float Water => this._Water;

		public override float WaterTemperature => this._WaterTemperature;

		public override float BoilerTemperature => this._BoilerTemperature;

		public override float Capacity => this._Capacity;


		////////////////

		private float _Water = 0f;

		private float _WaterTemperature = 1f;

		private float _BoilerTemperature = 1f;

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

		public override float DrainWater( float waterAmount, out float waterUnderflow ) {
			(float drainedWater, _) = SteamSource.CalculateWaterAdded(
				source: this,
				addedWaterAmount: -waterAmount,
				addedWaterHeatAmount: 1f,
				waterOverflow: out waterUnderflow
			);

			this._Water += drainedWater;

			return drainedWater;
		}


		////

		public override void SetBoilerHeat( float heatAmount ) {
			this._BoilerTemperature = heatAmount;
		}
	}
}