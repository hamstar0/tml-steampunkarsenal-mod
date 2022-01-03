using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using ModLibsGeneral.Libraries.Players;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public class ConvergentBoiler : Boiler {
		public override float Water => this.ConnectedBoilers.Sum( b => b.Water );

		public override float WaterTemperature  => this.ConnectedBoilers.Average( b => b.WaterTemperature );

		public override float BoilerTemperature  => this.ConnectedBoilers.Average( b => b.BoilerTemperature );

		public override float Capacity => this.ConnectedBoilers.Sum( b => b.Capacity );



		////////////////

		protected ISet<Boiler> ConnectedBoilers = new HashSet<Boiler>();



		////////////////

		public ConvergentBoiler() : base() { }

		public ConvergentBoiler( IList<Boiler> boilers ) : base() {
			this.ConnectedBoilers = new HashSet<Boiler>( boilers );
		}


		////////////////

		public override float AddWater( float waterAmount, float heatAmount, out float waterOverflow ) {
			float overflow = waterAmount;
			var availableBoilers = new HashSet<Boiler>( this.ConnectedBoilers );

			do {
				float divWaterAmt = overflow / (float)availableBoilers.Count;

				//

				overflow = 0f;

				foreach( Boiler boiler in availableBoilers.ToArray() ) {
					boiler.AddWater( divWaterAmt, heatAmount, out float myOverflow );

					//

					if( myOverflow > 0f ) {
						overflow += myOverflow;

						availableBoilers.Remove( boiler );
					}
				}
			} while( overflow > 0f && availableBoilers.Count > 0 );

			//

			waterOverflow = overflow;
			return waterAmount - waterOverflow;
		}


		////

		public override void SetBoilerHeat( float heatAmount ) {
			foreach( Boiler boiler in this.ConnectedBoilers ) {
				float currHeat = boiler.WaterTemperature;
				float addedHeat = heatAmount - currHeat;

				boiler.SetBoilerHeat( currHeat + addedHeat );
			}
		}


		////////////////

		protected internal override void Update( Player owner ) {
			this.RefreshConnectedBoilers( owner );

			foreach( Boiler boiler in this.ConnectedBoilers ) {
				boiler.Update( owner );
			}
		}


		////////////////

		private void RefreshConnectedBoilers( Player player ) {
			this.ConnectedBoilers.Clear();

			//

			Boiler boiler = Boiler.GetBoilerForItem( player.armor[1] );
			if( boiler == null ) {
				this.ConnectedBoilers.Add( boiler );
			}

			int minAcc = PlayerItemLibraries.VanillaAccessorySlotFirst;
			int maxAcc = minAcc + PlayerItemLibraries.GetCurrentVanillaMaxAccessories( player );
			for( int i=minAcc; i<maxAcc; i++ ) {
				boiler = Boiler.GetBoilerForItem( player.armor[i] );
				if( boiler == null ) {
					this.ConnectedBoilers.Add( boiler );
				}
			}

			//

			/*int beg = PlayerItemLibraries.VanillaAccessorySlotFirst;
			int max = PlayerItemLibraries.GetCurrentVanillaMaxAccessories( player );
			int end = beg + max;

			for( int i = beg; i < end; i++ ) {
				Item item = player.armor[i];
				var myitem = item?.modItem as PortABoilerItem;
				if( myitem == null ) {
					continue;
				}

				this.ConnectedBoilers.Add( myitem.Boiler );
			}*/
		}
	}
}