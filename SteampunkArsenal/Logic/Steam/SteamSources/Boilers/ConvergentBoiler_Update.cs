using System;
using System.Collections.Generic;
using Terraria;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Players;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class ConvergentBoiler : Boiler {
		protected internal override void PreUpdate( Player owner ) {
			this.RefreshConnectedBoilers( owner );

			//
			
			var boilers = new List<Boiler>();
			var nonBoilers = new List<SteamContainer>();

			foreach( SteamSource steamSrc in this.ConnectedSteamSources ) {
				if( steamSrc is Boiler ) {
					boilers.Add( (Boiler)steamSrc );
				} else if( steamSrc is SteamContainer ) {
					nonBoilers.Add( steamSrc as SteamContainer );
				}
			}

			//

			foreach( Boiler boiler in boilers ) {
				boiler.PreUpdate( owner );
			}

			//
			
			foreach( SteamContainer container in nonBoilers ) {
				container.TransferPressureToMeFromSourcesUntilFull( boilers );
			}

			//

			this.NormalizeSteamPressureIncrementally();
		}


		internal protected override void PostUpdate( Player owner ) {
			if( this.SteamPressure > this.Capacity ) {
				this.EmitSteam( owner.MountedCenter, this.SteamPressure );
			}

			//

			foreach( SteamSource steamSrc in this.ConnectedSteamSources ) {
				if( steamSrc is Boiler ) {
					((Boiler)steamSrc).PostUpdate( owner );
				}
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