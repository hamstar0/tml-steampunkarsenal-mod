using System;
using System.Collections.Generic;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources {
	public class SteamContainer : SteamSource {
		public override float Water => this._Water;

		public override float WaterTemperature => this._WaterTemperature;

		public override float Capacity => this._Capacity;


		////////////////

		private float _Water = 0f;

		private float _WaterTemperature = 0f;

		private float _Capacity = 0f;



		////////////////

		public override float AddWater( float waterAmount, float heatAmount, out float waterOverflow ) {
			(float addedWater, float addedHeat) = SteamSource.CalculateWaterAdded(
				this,
				waterAmount,
				heatAmount,
				out waterOverflow
			);

			this._WaterTemperature += addedHeat;

			return addedWater;
		}


		////////////////

		public void TransferPressureToMeFromSourcesUntilFull( IEnumerable<SteamSource> sources ) {
			foreach( SteamSource steamSrc in sources ) {
				if( this.SteamPressure >= this.Capacity ) {
					break;
				}

				//

				if( steamSrc.SteamPressure == 0f ) {
					continue;
				}

				//

				this.TransferPressureToMeFromSource( steamSrc, steamSrc.SteamPressure, out float overflow );

				if( overflow > 0f ) {
					steamSrc.AddWater( overflow, steamSrc.WaterTemperature, out _ );
				}
			}
		}
	}
}