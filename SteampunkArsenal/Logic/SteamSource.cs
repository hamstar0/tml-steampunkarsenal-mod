using System;
using Terraria;


namespace SteampunkArsenal.Logic {
	public abstract class SteamSource {
		public static float CapacityUsed( float water, float heat ) {
			float steamPressure = water * heat;
			return Math.Max( steamPressure, water );
		}



		////////////////

		public bool IsActive => this.Capacity > 0f;

		////

		public abstract float Water { get; }

		public abstract float WaterTemperature { get; }

		public abstract float Capacity { get; }


		////////////////

		public float SteamPressure => this.Water * this.WaterTemperature;



		////////////////

		public abstract float AddWater( float waterAmount, float heatAmount, out float waterOverflow );


		////

		public float TransferPressureToMeFromSource( SteamSource source, float pressureAmount, out float waterOverflow ) {
			if( source.SteamPressure <= 0 ) {
				waterOverflow = 0f;
				return 0f;
			}

			//

			float srcHeat = source.WaterTemperature;

			float srcWaterDrawAmt = pressureAmount / srcHeat;

			source.AddWater( -srcWaterDrawAmt, srcHeat, out _ );
			float waterAdded = this.AddWater( srcWaterDrawAmt, srcHeat, out waterOverflow );

			//

			return waterAdded;
		}
	}
}