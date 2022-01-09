using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SteampunkArsenal.Logic.Steam {
	public abstract partial class SteamSource {
		public static (float computedAddedWaterAmount, float computedAddedWaterHeatAmount) CalculateWaterAdded(
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

			float waterPercentAdded = destination.Water != 0f
				? addedWaterAmount / destination.Water
				: 1f;
			float addedWaterHeatPercent = MathHelper.Clamp( waterPercentAdded, -1f, 1f );

			// Heat amount after diffusing into existing temperature
			float computedAddedWaterHeatAmount = addedWaterHeatPercent * addedWaterHeatAmount;

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