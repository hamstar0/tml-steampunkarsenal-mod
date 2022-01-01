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

		public virtual bool IsActive => true;

		public virtual float Water { get; protected internal set; } = 0f;

		public virtual float Heat { get; protected internal set; } = 1f;


		////////////////

		public float SteamPressure => this.Water * this.Heat;



		////////////////

		public float AddWater( float waterAmount, float heatAmount ) {
			this.Water += waterAmount;

			if( waterAmount > 0f ) {
				this.AddHeat( waterAmount, heatAmount );
			}

			return waterAmount;
		}

		public float AddHeat( float waterAmount, float heatAmount ) {
			float percWaterAdded = waterAmount / (this.Water + waterAmount);
			float heatAdded = heatAmount * percWaterAdded;

			this.Heat += heatAdded;

			return heatAdded;
		}


		////

		public float TransferPressureToMeFromSource( Boiler source, float amount ) {
			if( source.SteamPressure <= 0 ) {
				return 0f;
			}

			//

			float srcHeat = source.Heat;

			float srcWaterDrawAmt = amount / srcHeat;

			source.AddWater( -srcWaterDrawAmt, srcHeat );
			this.AddWater( srcWaterDrawAmt, srcHeat );

			//

			return srcWaterDrawAmt;
		}


		////////////////

		internal protected virtual void Update() { }
	}
}