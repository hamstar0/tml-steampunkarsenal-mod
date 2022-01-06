using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using ModLibsGeneral.Libraries.Players;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class ConvergentBoiler : Boiler {
		public override float Water => this.ConnectedSteamSources
			.Sum( b => b.Water );

		public override float WaterTemperature  => this.ConnectedSteamSources.Count > 0
			? this.ConnectedSteamSources.Average( b => b.WaterTemperature )
			: 1f;

		public override float BoilerTemperature => this.ConnectedSteamSources
			.Any( b => b is Boiler )
				? this.ConnectedSteamSources
					.Where( b => b is Boiler )
					.Average( b => ((Boiler)b).BoilerTemperature )
				: 0f;

		public override float Capacity => this.ConnectedSteamSources
			.Sum( b => b.Capacity );

		////

		public override float SteamPressure => this.ConnectedSteamSources
			.Sum( b => b.SteamPressure );



		////////////////

		protected ISet<SteamSource> ConnectedSteamSources = new HashSet<SteamSource>();



		////////////////

		public ConvergentBoiler() : base() { }

		public ConvergentBoiler( IList<SteamSource> boilers ) : base() {
			this.ConnectedSteamSources = new HashSet<SteamSource>( boilers );
		}


		////////////////

		public override float AddWater( float waterAmount, float heatAmount, out float waterOverflow ) {
			float overflow = waterAmount;
			var availableSteamSources = new HashSet<SteamSource>( this.ConnectedSteamSources );

			do {
				float divWaterAmt = overflow / (float)availableSteamSources.Count;

				//

				overflow = 0f;

				foreach( SteamSource steamSrc in availableSteamSources.ToArray() ) {
					steamSrc.AddWater( divWaterAmt, heatAmount, out float myOverflow );

					//

					if( myOverflow > 0f ) {
						overflow += myOverflow;

						availableSteamSources.Remove( steamSrc );
					}
				}
			} while( overflow > 0f && availableSteamSources.Count > 0 );

			//

			waterOverflow = overflow;
			return waterAmount - waterOverflow;
		}


		////

		public override void SetBoilerHeat( float heatAmount ) {
			List<Boiler> boilers = this.ConnectedSteamSources
				.Where( ss => ss is Boiler )
				.Select( ss => ss as Boiler )
				.ToList();

			float mean = boilers.Average( ss => ss.WaterTemperature );
			float addAmt = heatAmount - mean;

			boilers.ForEach(
				ss => ss.SetBoilerHeat( ss.WaterTemperature + addAmt )
			);
		}


		////////////////

		private void NormalizeSteamPressure() {
			f
		}
	}
}