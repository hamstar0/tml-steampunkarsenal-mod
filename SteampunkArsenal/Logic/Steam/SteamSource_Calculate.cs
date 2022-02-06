using System;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal.Logic.Steam {
	public abstract partial class SteamSource {
		public static double CalculateWaterHeatAdded(
					double addedWater,
					double heatOfAddedWater,
					double currentWater,
					double currentWaterHeat ) {
			if( currentWater == 0d ) {
				return heatOfAddedWater - currentWaterHeat;
			}

			double addedWaterPercent = addedWater / currentWater;

			double computedWaterHeat = currentWaterHeat;
			computedWaterHeat += heatOfAddedWater * addedWaterPercent;
			computedWaterHeat /= 1d + addedWaterPercent;

			return computedWaterHeat - currentWaterHeat;
		}


		////

		public static (float computedAddedWater, float computedAddedWaterHeat) CalculateWaterAdded(
					SteamSource destination,
					float addedWater,
					float heatOfAddedWater,
					out float waterOverflow ) {
			float predictedWater = destination.Water + addedWater;

			float computedAddedWater, computedAddedWaterHeat;

			// Enforce capacity
			if( predictedWater > destination.WaterCapacity ) {
				waterOverflow = predictedWater - destination.WaterCapacity;

				//

				computedAddedWater = addedWater - waterOverflow;
				computedAddedWaterHeat = (float)SteamSource.CalculateWaterHeatAdded(
					addedWater: computedAddedWater,
					heatOfAddedWater: heatOfAddedWater,
					currentWater: destination.Water,
					currentWaterHeat: destination.WaterHeat
				);
			} else if( predictedWater < 0f ) {
				waterOverflow = predictedWater;  // negative value

				//

				computedAddedWater = -destination.Water;
				computedAddedWaterHeat = 0f;	// removed water does not alter temperature
			} else {
				waterOverflow = 0;

				//

				computedAddedWater = addedWater;
				computedAddedWaterHeat = (float)SteamSource.CalculateWaterHeatAdded(
					addedWater: computedAddedWater,
					heatOfAddedWater: heatOfAddedWater,
					currentWater: destination.Water,
					currentWaterHeat: destination.WaterHeat
				);
			}

			//

			return (computedAddedWater, computedAddedWaterHeat);
		}


		public static float CalculateWaterDrained( SteamSource source, float waterDrained, out float waterUnderflow ) {
			if( waterDrained > source.Water ) {
				waterUnderflow = waterDrained - source.Water;

				waterDrained = source.Water;
			} else {
				waterUnderflow = 0;
			}

			return waterDrained;
		}
	}
}