using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;


namespace SteampunkArsenal {
	public partial class Fx {
		private static void RollPosVel( out Vector2 randPos, out Vector2 randVel ) {
			randPos = new Vector2( Main.rand.Next(-12, 12), Main.rand.Next(-18, 18) );
			randVel = new Vector2( Main.rand.Next(-3, 3), Main.rand.Next(-3, 3) );
		}


		////////////////

		public static void CreateSmallSteamFx( Vector2 pos, int puffs, float scale ) {
			Vector2 randPos, randVel;

			for( int i = 0; i < puffs; i++ ) {
				Fx.RollPosVel( out randPos, out randVel );
				randPos += pos;

				int goreIdx = Gore.NewGore( randPos, randVel, 11, scale );
				Main.gore[goreIdx].alpha = 128;
			}
		}

		public static void CreateLargeSteamFx( Vector2 pos, int puffs, float scale ) {
			Vector2 randPos, randVel;

			for( int i = 0; i < puffs; i++ ) {
				Fx.RollPosVel( out randPos, out randVel );
				randPos += pos;

				int goreIdx = Gore.NewGore( randPos, randVel, 862, scale );
				Main.gore[goreIdx].alpha = 128;
			}
		}


		////

		public static void CreateSteamEruptionFx( Vector2 pos, float steamAmount ) {
			Fx.CreateSmallSteamFx( pos, (int)steamAmount / 5, 1f );
			Fx.CreateSmallSteamFx( pos, (int)steamAmount / 20, 2f );
			Fx.CreateLargeSteamFx( pos, (int)steamAmount / 35, 1f );
		}
	}
}