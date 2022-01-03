using System;
using Terraria;
using SteampunkArsenal.Items;
using SteampunkArsenal.Items.Armor;


namespace SteampunkArsenal.Logic {
	public abstract class Boiler : SteamSource {
		public static Boiler GetBoilerForItem( Item item ) {
			if( item?.active != true || item.modItem == null ) {
				return null;
			}

			if( item.modItem is SteamPoweredRivetLauncherItem ) {
				return ((SteamPoweredRivetLauncherItem)item.modItem).MyBoiler;
			}

			if( item.modItem is BoilerOBurdenItem ) {
				return ((BoilerOBurdenItem)item.modItem).MyBoiler;
			}

			//if( !(item.modItem is PortABoilerItem) ) {
			//	return ((PortABoilerItem)item.modItem).MyBoiler;
			//}

			return null;
		}



		////////////////

		public abstract float BoilerTemperature { get; }



		////////////////

		public abstract void SetBoilerHeat( float heatAmount );


		////////////////

		internal protected abstract void Update( Player owner );
	}
}