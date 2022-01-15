using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class ConvergentBoiler : Boiler {
		private void NormalizeSteamPressureIncrementally() {
			float xferWaterRate = 1f / 60f;

			//

			IEnumerable<Boiler> boilers = this.ConnectedSteamSources
				.Where( ss => ss is Boiler )
				.Select( ss => ss as Boiler );
			Boiler prevBoiler = null;

			foreach( Boiler boiler in boilers ) {
				if( prevBoiler == null ) {
					prevBoiler = boiler;
					continue;
				}
				if( boiler.SteamPressure <= prevBoiler.SteamPressure ) {
					continue;
				}
				if( (prevBoiler.Water - boiler.Water) < xferWaterRate ) {
					continue;
				}

				//

				float xferredWater = prevBoiler.DrainWater( xferWaterRate, out _ );

				boiler.AddWater( xferredWater, prevBoiler.WaterHeat, out float xferBackwash );

				if( xferBackwash > 0f ) {
					prevBoiler.AddWater( xferBackwash, prevBoiler.WaterHeat, out _ );
				}
			}
		}
	}
}