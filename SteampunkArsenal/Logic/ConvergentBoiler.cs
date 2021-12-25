using Terraria;


namespace SteampunkArsenal {
	public class ConvergentBoiler : Boiler {
		public override float Heat {
			get => this.ConnectedBoilers.Sum( b => b.Heat );
			protected set {
				foreach( Boiler boiler in this.ConnectedBoilers ) {
					boiler.Heat = value / (float)this.ConnectedBoilers.Count;
				}
			}
		}

		public override float Water {
			get => this.ConnectedBoilers.Sum( b => b.Water );
			protected set {
				foreach( Boiler boiler in this.ConnectedBoilers ) {
					boiler.Water = value / (float)this.ConnectedBoilers.Count;
				}
			}
		}



		////////////////
	}
}