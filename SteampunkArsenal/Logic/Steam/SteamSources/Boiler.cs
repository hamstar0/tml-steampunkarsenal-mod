using System;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources {
	public abstract class Boiler : SteamSource {
		public static Boiler GetBoilerForItem( Item item ) {
			SteamSource src = SteamSource.GetSteamSourceForItem( item );
			if( src == null || !(src is Boiler) ) {
				return null;
			}

			return src as Boiler;
		}



		////////////////

		public abstract float BoilerTemperature { get; }



		////////////////

		public abstract void SetBoilerHeat( float heatAmount );


		////////////////

		internal protected abstract void Update( Player owner );
	}
}