using System;
using Terraria;


namespace SteampunkArsenal.Logic.Steam {
	public abstract partial class SteamSource {
		public static (float computedAddedWaterAmount, float computedAddedWaterHeatAmount) CalculateWaterAdded(
					SteamSource source,
					float addedWaterAmount,
					float addedWaterHeatAmount,
					out float waterOverflow ) {
			float currSteam = source.Water * source.WaterTemperature;
			float addedSteam = addedWaterAmount * addedWaterHeatAmount;
			float predictSteam = currSteam + addedSteam;

			// Enforce capacity
			if( predictSteam > source.Capacity ) {
				float capacityOverflow = predictSteam - source.Capacity;

				waterOverflow = capacityOverflow / addedWaterHeatAmount;

				addedWaterAmount = (source.Capacity - currSteam) / addedWaterHeatAmount;
			} else if( predictSteam < 0f ) {
				waterOverflow = predictSteam / source.WaterTemperature;

				addedWaterAmount = -source.Water;
			} else {
				waterOverflow = 0;
			}

			//

			float waterPercentAdded = source.Water != 0f
				? addedWaterAmount / source.Water
				: 1f;

			// Heat amount after diffusing into existing temperature
			float computedAddedWaterHeatAmount = waterPercentAdded * addedWaterHeatAmount;

			return (addedWaterAmount, computedAddedWaterHeatAmount);
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