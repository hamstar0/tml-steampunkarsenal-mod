using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SteampunkArsenal.Dusts;


namespace SteampunkArsenal {
	public partial class Fx {
		public static void CreateSmallSteamFx(
					Vector2 position,
					Vector2 velocity,
					float dispersalRadius,
					float velocityNoise,
					int puffs,
					float scale ) {
			position.X -= 4f;

			for( int i = 0; i < puffs; i++ ) {
				SmallSteamDust.Create(
					centerPosition: position,
					velocity: velocity,
					dispersalRadius: dispersalRadius,
					velocityNoise: velocityNoise,
					scale: scale
				);
				//int goreIdx = Gore.NewGore( pos, vel, 11, scale );
				//Main.gore[goreIdx].alpha = alpha;
			}
		}

		public static void CreateLargeSteamFx(
					Vector2 position,
					Vector2 velocity,
					float dispersalRadius,
					float velocityNoise,
					int puffs,
					float scale ) {
					//int alpha = 128
			for( int i = 0; i < puffs; i++ ) {
				Vector2 posOffset = new Vector2(
					(Main.rand.NextFloat() * dispersalRadius * 2f) - dispersalRadius,
					(Main.rand.NextFloat() * dispersalRadius * 2f) - dispersalRadius
				);
				Vector2 velOffset = new Vector2(
					(Main.rand.NextFloat() * velocityNoise * 2f) - velocityNoise,
					(Main.rand.NextFloat() * velocityNoise * 2f) - velocityNoise
				);
				velOffset *= 0.5f;

				Vector2 pos = position + posOffset;
				Vector2 vel = velocity + velOffset;
				pos.X -= 16f;

				//

				int goreIdx = Gore.NewGore( pos, vel, 862, scale );
				Main.gore[goreIdx].alpha = 96;	//128
			}
		}


		////

		public static void CreateSteamEruptionFx(
					Vector2 position,
					float dispersalRadius,
					float steamAmount ) {
			float smallAmtChance = steamAmount / 5f;
			float midAmtChance = steamAmount / 20f;
			float bigAmtChance = steamAmount / 35f;

			int smallAmt = (int)smallAmtChance;
			if( Main.rand.NextFloat() < (smallAmtChance - smallAmt) ) {
				smallAmt++;
			}

			int midAmt = (int)midAmtChance;
			if( Main.rand.NextFloat() < (midAmtChance - midAmt) ) {
				midAmt++;
			}

			int bigAmt = (int)bigAmtChance;
			if( Main.rand.NextFloat() < (bigAmtChance - bigAmt) ) {
				bigAmt++;
			}

			Fx.CreateSmallSteamFx( position, default, dispersalRadius, 0f, smallAmt, 1f );
			Fx.CreateSmallSteamFx( position, default, dispersalRadius, 0f, midAmt, 2f );
			Fx.CreateLargeSteamFx( position, default, dispersalRadius, 0f, bigAmt, 1f );
		}
	}
}