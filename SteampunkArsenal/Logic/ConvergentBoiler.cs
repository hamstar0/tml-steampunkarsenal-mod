using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace SteampunkArsenal {
	public class ConvergentBoiler : Boiler {
		public override float Heat {
			get => this.ConnectedBoilers.Sum( b => b.Heat );
			protected internal set {
				float divVal = value / (float)this.ConnectedBoilers.Count;

				foreach( Boiler boiler in this.ConnectedBoilers ) {
					boiler.Heat = divVal;
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

		public ISet<Boiler> ConnectedBoilers { get; private set; } = new HashSet<Boiler>();



		////////////////

		public ConvergentBoiler() : base() { }

		public ConvergentBoiler( IList<Boiler> boilers ) : base() {
			this.ConnectedBoilers = new HashSet<Boiler>( boilers );
		}
	}
}