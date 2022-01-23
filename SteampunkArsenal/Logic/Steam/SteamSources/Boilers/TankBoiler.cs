using System;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class TankBoiler : Boiler {
		public override float Water => this._Water;

		public override float WaterHeat => this._WaterHeat;

		public override float BoilerHeat => this._BoilerHeat;

		public override float WaterLeakPerTick => this._WaterLeakPerTick;

		public override float TotalCapacity => this._Capacity;


		////////////////

		private float _Water = 0f;

		private float _WaterHeat = 1f;

		private float _BoilerHeat = 1f;

		private float _WaterLeakPerTick = 0f;

		private float _Capacity = 100f;



		////////////////

		public TankBoiler() : base( true ) { }



		////////////////

		protected override float AddWater( float addedWater, float heatOfAddedWater, out float waterOverflow ) {
			(float computedAddedWater, float computedAddedWaterHeat) = SteamSource.CalculateWaterAdded(
				destination: this,
				addedWater: addedWater,
				heatOfAddedWater: heatOfAddedWater,
				waterOverflow: out waterOverflow
			);
			
//LogLibraries.LogOnce("ADDEDWATER ["+this._Water+", "+this._WaterHeat+"] "
//	+addedWater+" -> "+computedAddedWater+" | "
//	+heatOfAddedWater+" + "+computedAddedWaterHeat
//);
			this._Water += computedAddedWater;
			this._WaterHeat += computedAddedWaterHeat;

			return computedAddedWater;
		}

		protected override float DrainWater( float waterDrained, out float waterUnderflow ) {
			float drainedWater = SteamSource.CalculateWaterDrained(
				source: this,
				waterDrained: waterDrained,
				waterUnderflow: out waterUnderflow
			);

			this._Water -= drainedWater;

			return drainedWater;
		}


		////

		protected override void SetBoilerHeat( float heat ) {
			this._BoilerHeat = heat;
		}
	}
}