using System;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public class TankBoiler : Boiler {
		public override float Water => this._Water;

		public override float WaterTemperature => this._WaterTemperature;

		public override float BoilerTemperature => this._BoilerTemperature;

		public override float Capacity => this._Capacity;


		////////////////

		private float _Water = 0f;

		private float _WaterTemperature = 0f;

		private float _BoilerTemperature = 0f;

		private float _Capacity = 0f;



		////////////////

		public override float AddWater( float waterAmount, float heatAmount, out float waterOverflow ) {
			(float addedWater, float addedHeat) = SteamSource.CalculateWaterAdded(
				this,
				waterAmount,
				heatAmount,
				out waterOverflow
			);

			this._WaterTemperature += addedHeat;

			return addedWater;
		}


		////

		public override void SetBoilerHeat( float heatAmount ) {
			this._BoilerTemperature = heatAmount;
		}


		////////////////

		internal protected override void Update( Player owner ) {
			float addedTemp = this.Water > 0f
				? this.BoilerTemperature / this.Water	// more water? add more heat!
				: 0f;

			this._WaterTemperature += addedTemp;

			if( this.WaterTemperature > this.BoilerTemperature ) {
				this._WaterTemperature = this.BoilerTemperature;
			}
		}
	}
}