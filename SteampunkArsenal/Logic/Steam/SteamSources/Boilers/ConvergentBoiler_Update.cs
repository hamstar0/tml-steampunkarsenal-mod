using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Players;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class ConvergentBoiler : Boiler {
		protected internal override void PreUpdate( Player owner, bool isChild ) {
			this.RefreshConnectedBoilers( owner );

			//
			
			var boilers = new List<Boiler>();
			var nonBoilers = new List<SteamContainer>();

			foreach( SteamSource steamSrc in this.ConnectedSteamSources ) {
				if( !steamSrc.IsActive ) {
					continue;
				}

				if( steamSrc is Boiler ) {
					boilers.Add( steamSrc as Boiler );
				} else if( steamSrc is SteamContainer ) {
					nonBoilers.Add( steamSrc as SteamContainer );
				}
			}

			//

			foreach( Boiler boiler in boilers ) {
				boiler.PreUpdate( owner, true );
			}

			//
			
			if( this.NormalizePressureDistributionIncrementally_If(boilers, nonBoilers) ) {
				if( SteamArseMod.Instance.SteamHissLoop.State != SoundState.Playing ) {
					SteamArseMod.Instance.SteamHissLoop.Volume = 0.35f;
					SteamArseMod.Instance.SteamHissLoop.Play();
				} else {
					SteamArseMod.Instance.SteamHissLoop.Stop();
				}
			}
		}


		internal protected override void PostUpdate( Player owner, bool isChild ) {
			if( !isChild ) {
				if( this.TotalPressure > this.TotalCapacity ) {
					this.EmitSteam( owner.MountedCenter, this.TotalPressure - this.TotalCapacity );
				}
			}

			//

			foreach( SteamSource steamSrc in this.ConnectedSteamSources ) {
				if( steamSrc is Boiler ) {
					((Boiler)steamSrc).PostUpdate( owner, true );
				}
			}
		}


		////////////////

		private void RefreshConnectedBoilers( Player player ) {
			this.ConnectedSteamSources.Clear();

			//
			
			int minAcc = PlayerItemLibraries.VanillaAccessorySlotFirst;
			int maxAcc = minAcc + PlayerItemLibraries.GetCurrentVanillaMaxAccessories( player );

			for( int i=0; i<maxAcc; i++ ) {
				SteamSource steamSrc = SteamSource.GetSteamSourceForItem( player.armor[i] );

				if( steamSrc != null && steamSrc.PlumbingType == PlumbingType.Worn ) {
					this.ConnectedSteamSources.Add( steamSrc );
				}
			}

			for( int i=0; i<player.inventory.Length; i++ ) {
				SteamSource steamSrc = SteamSource.GetSteamSourceForItem( player.inventory[i] );

				if( steamSrc != null && steamSrc.PlumbingType == PlumbingType.Inventory ) {
					this.ConnectedSteamSources.Add( steamSrc );
				}
			}
		}
	}
}