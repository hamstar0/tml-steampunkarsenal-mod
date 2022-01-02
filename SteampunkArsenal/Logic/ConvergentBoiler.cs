using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using SteampunkArsenal.Items.Armor;


namespace SteampunkArsenal {
	public class ConvergentBoiler : Boiler {
		public override bool IsActive => this.ConnectedBoilers.Any( b => b.IsActive );

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
				boiler.SetBoilerHeat( heatAmount );
			}
		}



		////////////////

		protected internal override void Update() {
			foreach( Boiler boiler in this.ConnectedBoilers ) {
				boiler.Update();
			}
		}


		////////////////

		public void RefreshConnectedBoilers( Player player ) {
			this.ConnectedBoilers.Clear();

			//

			Item bodyArmor = player.armor[1];

			if( bodyArmor?.active == true && bodyArmor.type == ModContent.ItemType<BoilerOBurdenItem>() ) {
				var myitem = bodyArmor.modItem as BoilerOBurdenItem;

				if( myitem != null ) {
					this.ConnectedBoilers.Add( myitem.MyBoiler );
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