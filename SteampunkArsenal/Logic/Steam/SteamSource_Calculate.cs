using System;
using Terraria;


namespace SteampunkArsenal.Logic.Steam {
	public abstract partial class SteamSource {
		public static (float computedAddedWaterAmount, float computedWaterHeatAmount) CalculateWaterAdded(
					SteamSource destination,
					float addedWaterAmount,
					float addedWaterHeatAmount,
					out float waterOverflow ) {
			float currSteam = destination.Water * destination.WaterTemperature;
			float addedSteam = addedWaterAmount * addedWaterHeatAmount;
			float predictSteam = currSteam + addedSteam;

			// Enforce capacity
			if( predictSteam > destination.Capacity ) {
				float capacityOverflow = predictSteam - destination.Capacity;

				waterOverflow = capacityOverflow / addedWaterHeatAmount;

				addedWaterAmount = (destination.Capacity - currSteam) / addedWaterHeatAmount;
			} else if( predictSteam < 0f ) {
				waterOverflow = predictSteam / destination.WaterTemperature;

				addedWaterAmount = -destination.Water;
			} else {
				waterOverflow = 0;
			}

			//

			if( destination.Water == 0f ) {
				return (addedWaterAmount, addedWaterHeatAmount);
			}

			//

			float percent = addedWaterAmount / destination.Water;
			float newTemp = destination.WaterTemperature + (addedWaterHeatAmount * percent);
			newTemp /= 1f + percent;

			//

			return (addedWaterAmount, newTemp);
		}


		public static float CalculateWaterDrained( SteamSource source, float waterDrainAmount, out float waterUnderflow ) {
			float currSteam = source.Water * source.WaterTemperature;
			float drainedSteam = waterDrainAmount * source.WaterTemperature;
			float predictSteam = currSteam - drainedSteam;

			// Enforce capacity
			if( predictSteam < 0f ) {
				waterUnderflow = -predictSteam / source.WaterTemperature;

				waterDrainAmount = source.Water;
			} else {
				waterUnderflow = 0;
			}

			return waterDrainAmount;
		}
	}
}