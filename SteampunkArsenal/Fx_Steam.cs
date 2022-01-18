using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Dusts;


namespace SteampunkArsenal {
	public partial class Fx {
		private static void RollPosVel( float dispersalScale, out Vector2 randPosOffset, out Vector2 randVelOffset ) {
			float posOffset = dispersalScale * 12f;
			float velOffset = dispersalScale * 3f;

			randPosOffset = new Vector2(
				(Main.rand.NextFloat() * posOffset * 2f) - posOffset,
				(Main.rand.NextFloat() * posOffset * 2f) - posOffset
			);
			randVelOffset = new Vector2(
				(Main.rand.NextFloat() * velOffset * 2f) - velOffset,
				(Main.rand.NextFloat() * velOffset * 2f) - velOffset
			);
		}


		////////////////

		public static void CreateSmallSteamFx(
					Vector2 basePosition,
					Vector2 baseVelocity,
					int puffs,
					float scale,
					float dispersalScale = 1f ) {
			Vector2 randPosOffset, randVelOffset;
			int wid = (int)(dispersalScale * 12f);
			int hei = (int)(dispersalScale * 12f);
			int steamDustType = ModContent.DustType<SmallSteamDust>();

			for( int i = 0; i < puffs; i++ ) {
				Fx.RollPosVel( dispersalScale, out randPosOffset, out randVelOffset );
				Vector2 pos = basePosition + randPosOffset;
				Vector2 vel = baseVelocity + randVelOffset;
				pos.X -= 16f;

				Dust.NewDust(
					Position: pos,
					Width: wid,
					Height: hei,
					Type: steamDustType,
					SpeedX: vel.X,
					SpeedY: vel.Y,
					Scale: scale
				);
				//int goreIdx = Gore.NewGore( pos, vel, 11, scale );
				//Main.gore[goreIdx].alpha = alpha;
			}
		}

		public static void CreateLargeSteamFx(
					Vector2 basePosition,
					Vector2 baseVelocity,
					int puffs,
					float scale,
					float dispersalScale = 1f ) {
					//int alpha = 128
			Vector2 randPosOffset, randVelOffset;

			for( int i = 0; i < puffs; i++ ) {
				Fx.RollPosVel( dispersalScale, out randPosOffset, out randVelOffset );
				Vector2 pos = basePosition + randPosOffset;
				Vector2 vel = baseVelocity + randVelOffset;
				pos.X -= 16f;

				int goreIdx = Gore.NewGore( pos, vel, 862, scale );
				Main.gore[goreIdx].alpha = 128;
			}
		}


		////

		public static void CreateSteamEruptionFx( Vector2 pos, float steamAmount ) {
			Fx.CreateSmallSteamFx( pos, default, (int)steamAmount / 5, 1f );
			Fx.CreateSmallSteamFx( pos, default, (int)steamAmount / 20, 2f );
			Fx.CreateLargeSteamFx( pos, default, (int)steamAmount / 35, 1f );
		}
	}
}