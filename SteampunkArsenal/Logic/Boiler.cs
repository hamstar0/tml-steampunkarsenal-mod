using Terraria;


namespace SteampunkArsenal {
	public class Boiler {
		public virtual float Water { get; protected internal set; } = 0f;

		public virtual float Heat { get; protected internal set; } = 1f;


		////////////////

		public float SteamPressure => this.Water * this.Heat;



		////////////////

		public void AddWater( float waterAmount, float heatAmount ) {
			if( waterAmount > 0f ) {
				this.AddHeat( waterAmount, heatAmount );
			}

			this.Water += waterAmount;
		}

		public void AddHeat( float waterAmount, float heatAmount ) {
			float percentWaterToXferHeatFrom = waterAmount / (this.Water + waterAmount);
			float percentHeatAdded = heatAmount * percentWaterToXferHeatFrom;

			this.Heat -= this.Heat * percentWaterToXferHeatFrom;

			this.Heat += percentHeatAdded;
		}


		////////////////

		public void TransferPressureToMeFromSource( Boiler source, float amount ) {
			float srcWater = source.Water;
			float srcHeat = source.Heat;

			float srcWaterDrawPerc = amount / ( srcWater * srcHeat );

			source.AddWater( srcWaterDrawPerc * -srcWater, srcHeat );

			//

			this.AddWater( srcWaterDrawPerc * srcWater, srcHeat );
		}
	}
}