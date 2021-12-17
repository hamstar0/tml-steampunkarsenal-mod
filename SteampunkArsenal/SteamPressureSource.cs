using Terraria;


namespace SteampunkArsenal {
	public class SteamPressureSource {
		public float BoilerWater { get; private set; } = 0f;

		public float BoilerHeat { get; private set; } = 1f;


		////////////////

		public float SteamPressure => this.BoilerWater * this.BoilerHeat;



		////////////////

		public void AddBoilerWater( float waterAmount, float heatAmount ) {
			if( waterAmount > 0f ) {
				float percentAdded = waterAmount / ( this.BoilerWater + waterAmount );
				float percentHeatAdded = heatAmount * percentAdded;

				this.BoilerHeat -= this.BoilerHeat * percentAdded;

				this.BoilerHeat += percentHeatAdded;
			}

			this.BoilerWater += waterAmount;
		}

		public void AddBoilerHeat( float amount ) {
			this.BoilerHeat += amount;
		}


		////////////////

		public void TransferPressureToMeFromSource( SteamPressureSource source, float amount ) {
			float srcWater = source.BoilerWater;
			float srcHeat = source.BoilerHeat;

			float srcWaterDrawPerc = amount / ( srcWater * srcHeat );

			source.AddBoilerWater( srcWaterDrawPerc * -srcWater, srcHeat );

			//

			this.AddBoilerWater( srcWaterDrawPerc * srcWater, srcHeat );
		}
	}
}