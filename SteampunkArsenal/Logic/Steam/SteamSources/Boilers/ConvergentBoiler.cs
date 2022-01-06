using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using ModLibsGeneral.Libraries.Players;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public class ConvergentBoiler : Boiler {
		public override float Water => this.ConnectedSteamSources.Sum( b => b.Water );

		public override float WaterTemperature  => this.ConnectedSteamSources.Average( b => b.WaterTemperature );

		public override float BoilerTemperature  => this.ConnectedSteamSources
			.Where( b => b is Boiler )
			.Average( b => ((Boiler)b).BoilerTemperature );

		public override float Capacity => this.ConnectedSteamSources.Sum( b => b.Capacity );



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

		protected internal override void Update( Player owner ) {
			this.RefreshConnectedBoilers( owner );

			//

			var boilers = new List<Boiler>();
			var containers = new List<SteamContainer>();

			foreach( SteamSource steamSrc in this.ConnectedSteamSources ) {
				if( steamSrc is Boiler ) {
					boilers.Add( (Boiler)steamSrc );

					((Boiler)steamSrc).Update( owner );
				} else if( steamSrc is SteamContainer ) {
					containers.Add( steamSrc as SteamContainer );
				}
			}

			//

			foreach( SteamContainer container in containers ) {
				container.TransferPressureToMeFromSourcesUntilFull( boilers );
			}
		}


		////////////////

		private void RefreshConnectedBoilers( Player player ) {
			this.ConnectedSteamSources.Clear();

			//

			SteamSource steamSrc = SteamSource.GetSteamSourceForItem( player.armor[1] );
			if( steamSrc != null ) {
				this.ConnectedSteamSources.Add( steamSrc );
			}

			int minAcc = PlayerItemLibraries.VanillaAccessorySlotFirst;
			int maxAcc = minAcc + PlayerItemLibraries.GetCurrentVanillaMaxAccessories( player );
			for( int i=minAcc; i<maxAcc; i++ ) {
				steamSrc = SteamSource.GetSteamSourceForItem( player.armor[i] );
				if( steamSrc != null ) {
					this.ConnectedSteamSources.Add( steamSrc );
				}
			}

			for( int i=0; i<player.inventory.Length; i++ ) {
				steamSrc = SteamSource.GetSteamSourceForItem( player.inventory[i] );
				if( steamSrc != null && steamSrc is SteamContainer ) {
					this.ConnectedSteamSources.Add( steamSrc );
				}
			}
		}
	}
}