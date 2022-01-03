using System;
using Terraria;
using SteampunkArsenal.Items;
using SteampunkArsenal.Items.Armor;
using SteampunkArsenal.Items.Accessories;


namespace SteampunkArsenal.Logic.Steam {
	public abstract class SteamSource {
		public static float CapacityUsed( float water, float heat ) {
			float steamPressure = water * heat;
			return Math.Max( steamPressure, water );
		}


		////////////////

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
			float currCapacityUse = SteamSource.CapacityUsed( source.Water, source.WaterTemperature );
			float addedCapacityUse = SteamSource.CapacityUsed( addedWaterAmount, addedWaterHeatAmount );

			// Enforce capacity
			if( ( addedCapacityUse + currCapacityUse ) > source.Capacity ) {
				float capacityOverflow = ( addedCapacityUse + currCapacityUse ) - source.Capacity;
				waterOverflow = capacityOverflow / addedWaterHeatAmount;

				addedWaterAmount = ( source.Capacity - currCapacityUse ) / addedWaterHeatAmount;
			} else {
				waterOverflow = 0;
			}

			//

			float waterPercentAdded = source.Water > 0f
				? addedWaterAmount / source.Water
				: addedWaterAmount;

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