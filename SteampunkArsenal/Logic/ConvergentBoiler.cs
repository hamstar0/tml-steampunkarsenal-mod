using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using SteampunkArsenal.Items.Armor;


namespace SteampunkArsenal {
	public class ConvergentBoiler : Boiler {
		public override float WaterTemperature {
			get => this.ConnectedBoilers.Sum( b => b.WaterTemperature );
			protected internal set {
				float divVal = value / (float)this.ConnectedBoilers.Count;

				foreach( Boiler boiler in this.ConnectedBoilers ) {
					boiler.WaterTemperature = divVal;
				}
			}
		}

		public override float Water {
			get => this.ConnectedBoilers.Sum( b => b.Water );
			protected internal set {
				foreach( Boiler boiler in this.ConnectedBoilers ) {
					boiler.Water = value / (float)this.ConnectedBoilers.Count;
				}
			}
		}



		////////////////

		public override bool IsActive => this.ConnectedBoilers.Count > 0;

		////

		public ISet<Boiler> ConnectedBoilers { get; private set; } = new HashSet<Boiler>();



		////////////////

		public ConvergentBoiler() : base() { }

		public ConvergentBoiler( IList<Boiler> boilers ) : base() {
			this.ConnectedBoilers = new HashSet<Boiler>( boilers );
		}


		////////////////

		public override float AddWater( float waterAmount, float heatAmount, out float waterOverflow ) {
		}


		////

		public override void SetBoilerHeat( float heatAmount ) {
			f
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