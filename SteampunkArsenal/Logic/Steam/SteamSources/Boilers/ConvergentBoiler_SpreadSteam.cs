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

			this.NormalizeBoilerPressureDistributionIncrementally(
				boilers: boilers,
				ratePerBoiler: 10f / 60f
			);

			this.NormalizeSteamContainerDistributionIncrementally(
				boilers: boilers,
				containers: containers,
				maxRatePerBoiler: 10f / 60f,
				minBoilerPressure: b => b.TotalCapacity * 0.9f
			);
		}


		////////////////

		private void NormalizeBoilerPressureDistributionIncrementally( IList<Boiler> boilers, float ratePerBoiler ) {
			Boiler prevBoiler = null;

			foreach( Boiler boiler in boilers ) {
				if( prevBoiler == null ) {
					prevBoiler = boiler;

					continue;
				}

				//

				if( Math.Abs(prevBoiler.TotalPressure - boiler.TotalPressure) >= ratePerBoiler ) {
					if( boiler.TotalPressure < prevBoiler.TotalPressure ) {
						boiler.TransferWaterToMeFromSource_If( prevBoiler, ratePerBoiler, out _, out _ );
					} else {
						prevBoiler.TransferWaterToMeFromSource_If( boiler, ratePerBoiler, out _, out _ );
					}
				}

				prevBoiler = boiler;
			}
		}

		////

		private void NormalizeSteamContainerDistributionIncrementally(
					IList<Boiler> boilers,
					IList<SteamContainer> containers,
					float maxRatePerBoiler,
					Func<Boiler, float> minBoilerPressure ) {
			foreach( SteamContainer container in containers.ToArray() ) {
				if( container.TotalPressure > (container.TotalCapacity - maxRatePerBoiler) ) {
					containers.Remove( container );

					continue;
				}

				//

				IEnumerable<Boiler> hotBoilers = boilers
					//.Where( b => b.SteamPressure >= maxRatePerBoiler )
					.Where( b => b.TotalPressure >= minBoilerPressure(b) )
					.OrderByDescending( b => b.SteamPressure );
				if( !hotBoilers.Any() ) {
					break;
				}

				//

				float highestSteamPerc = hotBoilers.First().SteamPressure;

				//

				foreach( Boiler boiler in hotBoilers ) {
					float xferPerc = boiler.SteamPressure / highestSteamPerc;
					float xferAmt = xferPerc * maxRatePerBoiler;

					//

					container.TransferSteamToMeFromSource_If(
						source: boiler,
						intendedSteamXferAmt: xferAmt,
						waterUnderflow: out _,
						waterOverflow: out float waterOverflow
					);

					if( waterOverflow > 0f ) {
						boiler.AddWater_If( waterOverflow, boiler.WaterHeat, out _ );
					}

					//

					if( container.TotalPressure > (container.TotalCapacity - xferAmt) ) {
						break;
					}
				}
			}
		}
	}
}