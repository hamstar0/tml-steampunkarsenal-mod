using System;
using Terraria;
using SteampunkArsenal.Items;
using SteampunkArsenal.Items.Armor;
using SteampunkArsenal.Items.Accessories;


namespace SteampunkArsenal.Logic.Steam {
	public abstract partial class SteamSource {
		public static SteamSource GetSteamSourceForItem( Item item ) {
			if( item?.active != true || item.modItem == null ) {
				return null;
			}

			if( item.modItem is SteamPoweredRivetLauncherItem ) {
				return ( (SteamPoweredRivetLauncherItem)item.modItem ).MyBoiler;
			}

			if( item.modItem is BoilerOBurdenItem ) {
				return ( (BoilerOBurdenItem)item.modItem ).MyBoiler;
			}

			if( item.modItem is PortABoilerItem ) {
				return ( (PortABoilerItem)item.modItem ).MyBoiler;
			}

			if( item.modItem is SteamBallItem ) {
				return ( (SteamBallItem)item.modItem ).Storage;
			}

			return null;
		}


		////////////////

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



		////////////////

		public bool IsActive => this.Capacity > 0f;

		////

		public abstract float Water { get; }

		public abstract float WaterTemperature { get; }

		public abstract float Capacity { get; }


		////////////////

		public virtual float SteamPressure => this.Water * this.WaterTemperature;



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