using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace SteampunkArsenal.Dusts {
	public class SmallSteamDust : ModDust {
		public override bool Autoload( ref string name, ref string texture ) {
			Main.instance.LoadGore( 11 );

			texture = "Terraria/Gore_11";
			return mod.Properties.Autoload;
		}


		////

		public override void OnSpawn( Dust dust ) {
			dust.noGravity = true;
			dust.frame = Main.goreTexture[ 11 ].Bounds;
			
			if( dust.color == default ) {
				dust.color = Color.White * 0.75f;
			}

			dust.velocity *= 0.1f;
			dust.velocity.Y -= (Main.rand.NextFloat() * 0.5f) + 0.5f;
			dust.rotation = (Main.rand.NextFloat() * MathHelper.Pi) - (MathHelper.Pi * 0.5f);
		}


		public override bool Update( Dust dust ) {
			int colorStep = 6;

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