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

			if( item.modItem is RivetLauncherItem ) {
				return ( (RivetLauncherItem)item.modItem ).MyBoiler;
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

		public bool IsActive => this.Capacity > 0f;

		////

		public abstract float Water { get; }

		public abstract float WaterTemperature { get; }

		public abstract float Capacity { get; }


		////////////////

		public virtual float SteamPressure => this.Water * this.WaterTemperature;



		////////////////

		public abstract float AddWater( float waterAmount, float heatAmount, out float waterOverflow );

		public abstract float DrainWater( float waterAmount, out float waterUnderflow );


		////

		public float TransferPressureToMeFromSource( SteamSource source, float pressureAmount, out float waterOverflow ) {
			if( source.SteamPressure <= 0 ) {
				waterOverflow = 0f;
				return 0f;
			}

			//

			float srcHeat = source.WaterTemperature;
			float srcWaterDrawAmt = pressureAmount / srcHeat;

			srcWaterDrawAmt = source.DrainWater( srcWaterDrawAmt, out _ );

			//

			float waterAdded = this.AddWater( srcWaterDrawAmt, srcHeat, out waterOverflow );

			//

			return waterAdded;
		}
	}
}