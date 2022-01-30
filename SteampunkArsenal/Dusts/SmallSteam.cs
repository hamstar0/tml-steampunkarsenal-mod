using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SteampunkArsenal.Dusts {
	public class SmallSteamDust : ModDust {
		public static void Create(
					Vector2 centerPosition,
					Vector2 velocity,
					float dispersalRadius,
					float velocityNoise,
					float scale ) {
			centerPosition -= new Vector2( dispersalRadius, dispersalRadius );
			velocityNoise *= 0.5f;

			Dust.NewDust(
				Position: centerPosition - new Vector2(dispersalRadius, dispersalRadius),
				Width: (int)(dispersalRadius * 2f),
				Height: (int)(dispersalRadius * 2f),
				Type: ModContent.DustType<SmallSteamDust>(),
				SpeedX: velocity.X + (Main.rand.NextFloat() * velocityNoise * 2f) - velocityNoise,
				SpeedY: velocity.Y + (Main.rand.NextFloat() * velocityNoise * 2f) - velocityNoise,
				Scale: scale
			);
		}



		////////////////

		public override bool Autoload( ref string name, ref string texture ) {
			if( Main.netMode != NetmodeID.Server && !Main.dedServ ) {
				Main.instance.LoadGore( 11 );
			}

			texture = "Terraria/Gore_11";
			return mod.Properties.Autoload;
		}


		////

		public override void OnSpawn( Dust dust ) {
			dust.noGravity = true;
			dust.frame = Main.goreTexture[ 11 ].Bounds;
			
			if( dust.color == default ) {
				dust.color = Color.White;
			}

			dust.alpha = 128;
			
			dust.velocity *= 0.1f;
			dust.velocity.Y -= (Main.rand.NextFloat() * 0.5f) + 0.5f;
			dust.rotation = (Main.rand.NextFloat() * MathHelper.Pi) - (MathHelper.Pi * 0.5f);
		}


		public override bool Update( Dust dust ) {
			int colorStep = 3;

			if( dust.alpha >= (255 - colorStep) ) {
				dust.active = false;

				return true;
			}

			//

			float perc = (float)dust.alpha / 255f;

			dust.color *= 1f - perc;

			//

			dust.alpha += colorStep;

			//

			dust.position += dust.velocity;
			dust.scale += 0.01f;

			//

			float rotDeg = MathHelper.ToDegrees( dust.rotation );
			rotDeg += dust.rotation / MathHelper.Pi;

			dust.rotation = MathHelper.ToRadians( rotDeg );

			//

			return false;
		}
	}
}