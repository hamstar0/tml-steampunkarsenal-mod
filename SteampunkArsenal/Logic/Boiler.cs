using System;
using Terraria;
using SteampunkArsenal.Items;
using SteampunkArsenal.Items.Armor;


namespace SteampunkArsenal {
	public class Boiler {
		public static Boiler GetBoilerForItem( Item item ) {
			if( item?.active != true || item.modItem == null ) {
				return null;
			}

			if( item.modItem is SteamPoweredRivetLauncherItem ) {
				return ((SteamPoweredRivetLauncherItem)item.modItem).MyBoiler;
			}

			if( item.modItem is BoilerOBurdenItem ) {
				return ((BoilerOBurdenItem)item.modItem).MyBoiler;
			}

			//if( !(item.modItem is PortABoilerItem) ) {
			//	return ((PortABoilerItem)item.modItem).MyBoiler;
			//}

			return null;
		}


		////////////////
		
		public static float CapacityUsed( float water, float heat ) {
			float steamPressure = water * heat;
			return Math.Max( steamPressure, water );
		}



		////////////////

		public virtual bool IsActive => this.Capacity > 0f;

		public virtual float Water { get; protected internal set; } = 0f;

		public virtual float WaterTemperature { get; protected internal set; } = 0f;

		public virtual float BoilerTemperature { get; protected internal set; } = 0f;

		public virtual float Capacity { get; protected internal set; } = 100f;


		////////////////

		public float SteamPressure => this.Water * this.WaterTemperature;

		public float WaterCapacityUsed => Boiler.CapacityUsed( this.Water, this.WaterTemperature );



		////////////////

		public virtual float AddWater( float waterAmount, float heatAmount, out float waterOverflow ) {
			float currCapacityUse = this.WaterCapacityUsed;
			float addedCapacityUse = Boiler.CapacityUsed( waterAmount, heatAmount );
			
			// Enforce capacity
			if( (addedCapacityUse + currCapacityUse) > this.Capacity ) {
				float capacityOverflow = (addedCapacityUse + currCapacityUse) - this.Capacity;
				waterOverflow = capacityOverflow / heatAmount;

				waterAmount = (this.Capacity - currCapacityUse) / heatAmount;
			} else {
				waterOverflow = 0;
			}

			//

			float waterPercentAdded = this.Water > 0f
				? waterAmount / this.Water
				: waterAmount;

			this.WaterTemperature += waterPercentAdded * heatAmount;

			//

			this.Water += waterAmount;

			return waterAmount;
		}


		////

		public virtual void SetBoilerHeat( float heatAmount ) {
			this.BoilerTemperature = heatAmount;
		}


		////

		public float TransferPressureToMeFromSource( Boiler source, float pressureAmount, out float waterOverflow ) {
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


		////////////////

		internal protected virtual void Update() {
			float addedTemp = this.Water > 0f
				? this.BoilerTemperature / this.Water	// more water? add more heat!
				: 0f;

			this.WaterTemperature += addedTemp;

			if( this.WaterTemperature > this.BoilerTemperature ) {
				this.WaterTemperature = this.BoilerTemperature;
			}
		}
	}
}