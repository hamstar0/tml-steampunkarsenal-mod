using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class ConvergentBoiler : Boiler {
		private void NormalizePressureDistributionIncrementally_If(
					IList<Boiler> boilers,
					IList<SteamContainer> containers ) {
			if( !this.IsActive ) {
				return;
			}

			//

			this.NormalizeBoilerWaterDistributionIncrementally( boilers, 1f / 60f );

			this.NormalizeSteamContainerDistributionIncrementally( boilers, containers, 1f / 60f );
		}


		////////////////

		private void NormalizeBoilerWaterDistributionIncrementally( IList<Boiler> boilers, float rate ) {
			Boiler prevBoiler = null;

			foreach( Boiler boiler in boilers ) {
				if( prevBoiler == null ) {
					prevBoiler = boiler;

					continue;
				}

				//

				if( Math.Abs(prevBoiler.TotalPressure - boiler.TotalPressure) >= rate ) {
					if( boiler.TotalPressure < prevBoiler.TotalPressure ) {
						boiler.TransferWaterToMeFromSource_If( prevBoiler, rate, out _, out _ );
					} else {
						prevBoiler.TransferWaterToMeFromSource_If( boiler, rate, out _, out _ );
					}
				}

				prevBoiler = boiler;
			}
		}

		////

		private void NormalizeSteamContainerDistributionIncrementally(
					IList<Boiler> boilers,
					IList<SteamContainer> containers,
					float rate ) {
			foreach( Boiler boiler in boilers ) {
				if( boiler.SteamPressure < rate ) {
					continue;
				}

				foreach( SteamContainer container in containers.ToArray() ) {
					if( container.TotalPressure > (container.TotalCapacity - rate) ) {
						containers.Remove( container );

						continue;
					}

					container.TransferSteamToMeFromSource_If( boiler, rate, out _, out float waterOverflow );

					if( waterOverflow > 0f ) {
						boiler.AddWater_If( waterOverflow, boiler.WaterHeat, out _ );
					}
				}
			}
		}
	}
}