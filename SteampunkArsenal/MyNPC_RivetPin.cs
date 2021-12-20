using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Net;


namespace SteampunkArsenal {
	partial class SteamArseNPC : GlobalNPC {
		public static void ApplyRivetToIf_SyncsFromServer( NPC npc, Projectile rivetProjectile ) {
			if( npc?.active != true || rivetProjectile?.active != true ) {
				return;
			}
			if( npc.GetGlobalNPC<SteamArseNPC>().RivetedTo != null ) {
				return;
			}

			Vector2 diff = npc.Center - rivetProjectile.Center;
			float distSqr = diff.LengthSquared();

			if( distSqr < (128f * 128f) ) {
				SteamArseNPC.ApplyRivetTo_SyncsFromServer( npc, rivetProjectile );
			}
		}

		private static void ApplyRivetTo_SyncsFromServer( NPC npc, Projectile rivetProjectile ) {
			Vector2 offset;
			float npcDim = (npc.width + npc.height) * 0.5f;

			var mynpc = npc.GetGlobalNPC<SteamArseNPC>();

			if( mynpc.RivetedTo.Count() == 0 ) {
				Vector2 openPos = SteamArseNPC.FindBestPinPosition( npcDim, npc.Center, rivetProjectile.Center );
				offset = openPos - rivetProjectile.Center;
			} else {
				offset = npc.Center - rivetProjectile.Center;
			}

			mynpc.RivetedTo[rivetProjectile] = offset;

			//

			if( Main.netMode == NetmodeID.Server ) {
				NPCPinProtocol.SendToClients(
					npcWho: npc.whoAmI,
					rivetProjOwner: rivetProjectile.owner,
					rivetProjId: rivetProjectile.identity,
					pinStrength: rivetProjectile.timeLeft,
					offset: offset
				);
			}
		}


		////////////////////
		
		public static Vector2 FindBestPinPosition( float targetSize, Vector2 targetPos, Vector2 pinPos ) {
			float maxDist = 4 * 16;
			float maxDistSqr = maxDist * maxDist;
			float wldMaxX = Main.maxTilesX * 16;
			float wldMaxY = Main.maxTilesY * 16;

			var invalidPositions = new HashSet<Vector2>();
			var scannedPositions = new HashSet<Vector2>();

			//

			Vector2 diff = targetPos - pinPos;
			Vector2 offset = Vector2.Normalize( diff ) * targetSize * 0.75f;
			Vector2 newTargetPos = offset + pinPos;

			scannedPositions.Add( newTargetPos );

			//

			bool ValidateCheckPoint( Vector2 worldPos ) {
				if( worldPos.X < 0 || worldPos.X > wldMaxX ) {
					return false;
				}
				if( worldPos.Y < 0 || worldPos.Y > wldMaxY ) {
					return false;
				}

				float distSqr = (pinPos - worldPos).LengthSquared();
				if( distSqr >= maxDistSqr ) {
					return false;
				}

				Tile tile = Main.tile[ (int)worldPos.X/16, (int)worldPos.Y/16 ];
				if( tile?.active() == true && Main.tileSolid[tile.type] ) {
					return false;
				}

				return !invalidPositions.Contains( worldPos );
			}

			void ExpandCheckingRange() {
				foreach( Vector2 scannedPos in scannedPositions.ToArray() ) {
					if( invalidPositions.Contains(scannedPos) ) {
						continue;
					}

					//

					var top = new Vector2( scannedPos.X, scannedPos.Y - 16 );
					if( !scannedPositions.Contains(top) ) {
						if( ValidateCheckPoint(top) ) {
							scannedPositions.Add( top );
						} else {
							invalidPositions.Add( top );
						}
					}

					var left = new Vector2( scannedPos.X - 16, scannedPos.Y );
					if( !scannedPositions.Contains(left) ) {
						if( ValidateCheckPoint(left) ) {
							scannedPositions.Add( left );
						} else {
							invalidPositions.Add( left );
						}
					}

					var right = new Vector2( scannedPos.X + 16, scannedPos.Y );
					if( !scannedPositions.Contains(right) ) {
						if( ValidateCheckPoint(right) ) {
							scannedPositions.Add( right );
						} else {
							invalidPositions.Add( right );
						}
					}

					var bottom = new Vector2( scannedPos.X, scannedPos.Y + 16 );
					if( !scannedPositions.Contains(bottom) ) {
						if( ValidateCheckPoint(bottom) ) {
							scannedPositions.Add( bottom );
						} else {
							invalidPositions.Add( bottom );
						}
					}
				}
			}

			//

			ExpandCheckingRange();
			ExpandCheckingRange();
			ExpandCheckingRange();
			ExpandCheckingRange();

			//

			float shortestDistSqr = float.MaxValue;
			Vector2 shortestDistPos = newTargetPos;

			foreach( Vector2 scannedPos in scannedPositions ) {
				if( invalidPositions.Contains(scannedPos) ) {
					continue;
				}

				float distSqr = (scannedPos - pinPos).LengthSquared();
				if( distSqr < shortestDistSqr ) {
					shortestDistSqr = distSqr;
					shortestDistPos = scannedPos;
				}
			}

//Timers.RunUntil( () => {
//	Dust.QuickDust( shortestDistPos, Color.Lime );
//	Dust.QuickDust( newTargetPos, Color.Red );
//	return true;
//}, 60, false );
			return shortestDistPos;
		}



		////////////////////

		internal void SyncRivetPinFor( int rivetProjWho, int pinStrength, Vector2 offset ) {
			Projectile proj = Main.projectile[ rivetProjWho ];
			if( proj?.active != true ) {
				LogLibraries.WarnOnce( "Could not sync rivet pin" );
				return;
			}

			this.RivetedTo[ proj ] = offset;

			proj.timeLeft = pinStrength;
		}
	}
}