using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
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

			if( Main.netMode != NetmodeID.Server ) {
				SoundEffectInstance hissSnd = SteamArseMod.Instance.SteamHissLoop;

				if( this.NormalizePressureDistributionIncrementally_If(boilers, nonBoilers) ) {
					if( hissSnd.State == SoundState.Stopped ) {
						hissSnd.Play();
					} else if( hissSnd.State == SoundState.Paused ) {
						hissSnd.Resume();
					}
				} else {
					if( hissSnd.State == SoundState.Playing ) {
						hissSnd.Pause();
					}
				}
			}
		}


		internal protected override void PostUpdate( Player owner, bool isChild ) {
			if( !isChild ) {
				if( this.TotalPressure > this.WaterCapacity ) {
					this.EmitSteam( owner.MountedCenter, this.TotalPressure - this.WaterCapacity );
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
				if( PlayerItemLibraries.IsVanitySlot(player, i) ) {
					continue;
				}

				//

				SteamSource steamSrc = SteamSource.GetSteamSourceForItem( player.armor[i] );

				if( steamSrc != null && steamSrc.PlumbingType == PlumbingType.Worn && steamSrc.IsActive ) {
					this.ConnectedSteamSources.Add( steamSrc );
				}
			}

			for( int i=0; i<player.inventory.Length; i++ ) {
				SteamSource steamSrc = SteamSource.GetSteamSourceForItem( player.inventory[i] );

				if( steamSrc != null && steamSrc.PlumbingType == PlumbingType.Inventory && steamSrc.IsActive ) {
					this.ConnectedSteamSources.Add( steamSrc );
				}
			}
		}
	}
}