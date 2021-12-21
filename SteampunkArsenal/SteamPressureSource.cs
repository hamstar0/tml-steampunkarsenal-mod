using Terraria;


namespace SteampunkArsenal {
	public class SteamPressureSource {
		public float Water { get; private set; } = 0f;

		public float Heat { get; private set; } = 1f;


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

		public void TransferPressureToMeFromSource( SteamPressureSource source, float amount ) {
			float srcWater = source.Water;
			float srcHeat = source.Heat;

			float srcWaterDrawPerc = amount / ( srcWater * srcHeat );

			source.AddWater( srcWaterDrawPerc * -srcWater, srcHeat );

			//

			this.AddWater( srcWaterDrawPerc * srcWater, srcHeat );
		}
	}
}