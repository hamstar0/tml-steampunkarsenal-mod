using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class ConvergentBoiler : Boiler {
		private void NormalizeSteamPressureIncrementally_If() {
			if( this.IsActive ) {
				this.NormalizeSteamPressureIncrementally();
			}
		}

		private void NormalizeSteamPressureIncrementally() {
			float xferWaterRate = 1f / 60f;

			//

			IEnumerable<Boiler> boilers = this.ConnectedSteamSources
				.Where( ss => ss is Boiler && ss.IsActive )
				.Select( ss => ss as Boiler );
			Boiler prevBoiler = null;

			foreach( Boiler boiler in boilers ) {
				if( prevBoiler == null ) {
					prevBoiler = boiler;
					continue;
				}
				if( boiler.TotalPressure <= prevBoiler.TotalPressure ) {
					continue;
				}
				if( (prevBoiler.Water - boiler.Water) < xferWaterRate ) {
					continue;
				}

				//

				float xferredWater = prevBoiler.DrainWater_If( xferWaterRate, out _ );

				if( xferredWater > 0f ) {
					boiler.AddWater_If( xferredWater, prevBoiler.WaterHeat, out float xferBackwash );

					if( xferBackwash > 0f ) {
						prevBoiler.AddWater_If( xferBackwash, prevBoiler.WaterHeat, out _ );
					}
				}
			}
		}
	}
}