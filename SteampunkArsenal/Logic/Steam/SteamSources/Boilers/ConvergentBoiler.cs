using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class ConvergentBoiler : Boiler {
		public override float Water => this.ConnectedSteamSources
			.Sum( b => b.Water );

		public override float WaterHeat  => this.ConnectedSteamSources.Count > 0
			? this.ConnectedSteamSources.Average( b => b.WaterHeat )
			: 1f;

		public override float BoilerHeat => this.ConnectedSteamSources
			.Any( b => b is Boiler )
				? this.ConnectedSteamSources
					.Where( b => b is Boiler )
					.Average( b => ((Boiler)b).BoilerHeat )
				: 0f;

		public override float SteamCapacity => this.ConnectedSteamSources
			.Sum( b => b.SteamCapacity );

		////

		public override float SteamPressure => this.ConnectedSteamSources
			.Sum( b => b.SteamPressure );



		////////////////

		protected ISet<SteamSource> ConnectedSteamSources = new HashSet<SteamSource>();



		////////////////

		public ConvergentBoiler( bool canConverge ) : base( canConverge ) { }

		public ConvergentBoiler( bool canConverge, IList<SteamSource> boilers ) : base( canConverge ) {
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
					steamSrc.AddWater( divWaterAmt, heatAmount, out float latestOverflow );

					//

					if( latestOverflow > 0f ) {
						overflow += latestOverflow;

						availableSteamSources.Remove( steamSrc );
					}
				}
			} while( overflow > 0f && availableSteamSources.Count > 0 );

			//

			waterOverflow = overflow;
			return waterAmount - waterOverflow;
		}


		public override float DrainWater( float waterAmount, out float waterUnderflow ) {
			float underflow = waterAmount;
			var availableSteamSources = new HashSet<SteamSource>( this.ConnectedSteamSources );

			do {
				float divWaterAmt = underflow / (float)availableSteamSources.Count;

				//

				underflow = 0f;

				foreach( SteamSource steamSrc in availableSteamSources.ToArray() ) {
					steamSrc.DrainWater( divWaterAmt, out float latestUnderflow );

					//

					if( latestUnderflow > 0f ) {
						underflow += latestUnderflow;

						availableSteamSources.Remove( steamSrc );
					}
				}
			} while( underflow > 0f && availableSteamSources.Count > 0 );

			//

			waterUnderflow = underflow;
			return waterAmount - waterUnderflow;
		}


		////////////////

		public override void SetBoilerHeat( float heatAmount ) {
			List<Boiler> boilers = this.ConnectedSteamSources
				.Where( ss => ss is Boiler )
				.Select( ss => ss as Boiler )
				.ToList();

			float mean = boilers.Average( ss => ss.WaterHeat );
			float addAmt = heatAmount - mean;

			boilers.ForEach(
				ss => ss.SetBoilerHeat( ss.WaterHeat + addAmt )
			);
		}
	}
}