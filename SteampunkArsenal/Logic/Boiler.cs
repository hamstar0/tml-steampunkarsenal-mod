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

		public void AddWater( float waterAmount, float heatAmount ) {
			this.Water += waterAmount;

			if( waterAmount > 0f ) {
				this.AddHeat( waterAmount, heatAmount );
			}
		}

		public void AddHeat( float waterAmount, float heatAmount ) {
			float percentWaterToXferHeatFrom = waterAmount / (this.Water + waterAmount);
			float percentHeatAdded = heatAmount * percentWaterToXferHeatFrom;

			this.Heat -= this.Heat * percentWaterToXferHeatFrom;

			this.Heat += percentHeatAdded;
		}


		////

		public void TransferPressureToMeFromSource( Boiler source, float amount ) {
			float srcWater = source.Water;
			float srcHeat = source.Heat;

			float srcWaterDrawPerc = amount / ( srcWater * srcHeat );

			source.AddWater( srcWaterDrawPerc * -srcWater, srcHeat );

			//

			this.AddWater( srcWaterDrawPerc * srcWater, srcHeat );
		}


		////////////////

		internal protected virtual void Update() { }
	}
}