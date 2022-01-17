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


		public override void OnSpawn( Dust dust ) {
			dust.noGravity = true;
			dust.frame = Main.goreTexture[ 11 ].Bounds;
			dust.color = new Color( 192, 192, 192, 192 );

			dust.velocity *= 0.1f;
			dust.velocity.Y -= (Main.rand.NextFloat() * 0.5f) + 0.5f;
			//dust.rotation = Main.rand.NextFloat() * MathHelper.Pi;
		}

		public override bool Update( Dust dust ) {
			int colorStep = 6;
			byte c = (byte)(dust.color.R - colorStep );

			if( c <= colorStep ) {
				dust.active = false;

				return true;
			}

			//

			dust.color = new Color( c, c, c, c );

			//

			dust.position += dust.velocity;
			dust.scale += 0.01f;

			//

			//float rotDeg = MathHelper.ToDegrees( dust.rotation );
			//float newRotDeg = ( (int)rotDeg % 2 ) == 0
			//	? rotDeg + 2f
			//	: rotDeg - 2f;

			//dust.rotation = MathHelper.ToRadians( newRotDeg );

			return false;
		}
	}
}