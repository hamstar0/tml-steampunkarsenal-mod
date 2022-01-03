using System;
using Terraria;
using SteampunkArsenal.Items;
using SteampunkArsenal.Items.Armor;


namespace SteampunkArsenal {
	public abstract class Boiler {
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

		public bool IsActive => this.Capacity > 0f;

		////

		public abstract float Water { get; }

		public abstract float WaterTemperature { get; }

		public abstract float BoilerTemperature { get; }

		public abstract float Capacity { get; }


		////////////////

		public float SteamPressure => this.Water * this.WaterTemperature;



		////////////////

		public abstract float AddWater( float waterAmount, float heatAmount, out float waterOverflow );

		public abstract void SetBoilerHeat( float heatAmount );


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

		internal protected abstract void Update( Player owner );
	}
}