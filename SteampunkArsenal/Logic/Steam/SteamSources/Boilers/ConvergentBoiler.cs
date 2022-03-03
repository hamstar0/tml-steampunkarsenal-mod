using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class ConvergentBoiler : Boiler {
		public override float Water => this.ConnectedSteamSources
			.Sum( b => b.Water );

		public override float WaterHeat  => this.ConnectedSteamSources.Count > 0
			? this.ConnectedSteamSources
				.Average( b => b.WaterHeat )
			: 1f;

		public override float BoilerHeat => this.ConnectedSteamSources
			.Any( b => b is Boiler )
				? this.ConnectedSteamSources
					.Where( b => b is Boiler )
					.Average( b => ((Boiler)b).BoilerHeat )
				: 0f;

		public override float WaterCapacity => this.ConnectedSteamSources
			.Sum( b => b.WaterCapacity );

		////
		
		public override float TotalPressure => this.ConnectedSteamSources
			.Sum( b => b.TotalPressure );

		////

		public override float PressurePercentLeakPerTick => this._PressurePercentLeakPerTick;



		////////////////

		protected ISet<SteamSource> ConnectedSteamSources = new HashSet<SteamSource>();

		private float _PressurePercentLeakPerTick = 0f;



		////////////////

		public ConvergentBoiler( PlumbingType plumbingType ) : base( plumbingType ) { }

		public ConvergentBoiler(
					PlumbingType plumbingType,
					IList<SteamSource> boilers ) : base( plumbingType ) {
			this.ConnectedSteamSources = new HashSet<SteamSource>( boilers );
		}


		////////////////

		public float AddWaterBoilersOnly(
					float waterAmount,
					float heatAmount,
					out float waterOverflow ) {
			var steamSources = new HashSet<SteamSource>(
				this.ConnectedSteamSources.Where( ss => ss is Boiler )
			);

			return this.AddWater( steamSources, waterAmount, heatAmount, out waterOverflow );
		}

		protected override float AddWater(
					float waterAmount,
					float heatAmount,
					out float waterOverflow ) {
			var steamSources = new HashSet<SteamSource>( this.ConnectedSteamSources );

			return this.AddWater( steamSources, waterAmount, heatAmount, out waterOverflow );
		}

		private float AddWater(
					ISet<SteamSource> steamSources,
					float waterAmount,
					float heatAmount,
					out float waterOverflow ) {
			float overflow = waterAmount;

			while( overflow > 0f && steamSources.Count > 0 ) {
				float divWaterAmt = overflow / (float)steamSources.Count;

				//

				overflow = 0f;
				
				foreach( SteamSource steamSrc in steamSources.ToArray() ) {
					steamSrc.AddWater_If( divWaterAmt, heatAmount, out float latestOverflow );

					//

					if( latestOverflow > 0f ) {
						overflow += latestOverflow;

						steamSources.Remove( steamSrc );
					}
				}
			}

			//
			
			waterOverflow = overflow;
//DebugLibraries.Print( "addwater", "sources: "+steamSources.Count
//	+", waterAmount: "+waterAmount
//	+", waterOverflow: "+waterOverflow );
			return waterAmount - waterOverflow;
		}


		////

		public float DrainBoilersOnlyWater_If( float waterAmount, out float waterUnderflow ) {
			var steamSources = new HashSet<SteamSource>(
				this.ConnectedSteamSources.Where( ss => ss is Boiler )
			);

			return this.DrainWater( steamSources, waterAmount, out waterUnderflow );
		}

		protected override float DrainWater( float waterAmount, out float waterUnderflow ) {
			var nonBoilerSteamSources = this.ConnectedSteamSources
				.Where( ss => !(ss is Boiler) );
			var steamSources = new HashSet<SteamSource>( nonBoilerSteamSources );

			float drawnWater = this.DrainWater( steamSources, waterAmount, out waterUnderflow );

			if( drawnWater == waterAmount || waterUnderflow > 0f ) {
				return drawnWater;
			}

			//

			var boilerSteamSources = steamSources
				.Where( ss => ss is Boiler );
			steamSources = new HashSet<SteamSource>( boilerSteamSources );

			return this.DrainWater( steamSources, waterAmount, out waterUnderflow );
		}

		private float DrainWater( ISet<SteamSource> steamSources, float waterAmount, out float waterUnderflow ) {
			float underflow = waterAmount;

			//

			do {
				float divWaterAmt = underflow / (float)steamSources.Count;

				//

				underflow = 0f;

				foreach( SteamSource steamSrc in steamSources.ToArray() ) {
					steamSrc.DrainWater_If( divWaterAmt, out float latestUnderflow );

					//

					if( latestUnderflow > 0f ) {
						underflow += latestUnderflow;

						steamSources.Remove( steamSrc );
					}
				}
			} while( underflow > 0f && steamSources.Count > 0 );

			//

			waterUnderflow = underflow;
			return waterAmount - waterUnderflow;
		}


		////////////////

		protected override void SetBoilerHeat( float heatAmount ) {
			List<Boiler> boilers = this.ConnectedSteamSources
				.Where( ss => ss is Boiler && ss.IsActive )
				.Select( ss => ss as Boiler )
				.ToList();
			
			float mean = boilers.Average( ss => ss.BoilerHeat );
			float addAmt = heatAmount - mean;

			boilers.ForEach(
				ss => ss.SetBoilerHeat_If( ss.BoilerHeat + addAmt )
			);
		}


		////////////////

		internal protected override bool CanSave() {
			return false;
		}

		internal protected override void Load( TagCompound tag ) {
			throw new NotImplementedException( "Cannot load ConvernentBoiler" );
		}

		internal protected override TagCompound Save() {
			throw new NotImplementedException( "Cannot save ConvernentBoiler" );
		}
	}
}