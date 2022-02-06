using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using SteampunkArsenal.Logic.Steam;


namespace SteampunkArsenal {
	partial class SteamArseItem : GlobalItem {
		public override bool NeedsSaving( Item item ) {
			SteamSource steamSrc = SteamSource.GetSteamSourceForItem( item );
			return steamSrc?.CanSave() ?? false;
		}

		public override void Load( Item item, TagCompound tag ) {
			SteamSource steamSrc = SteamSource.GetSteamSourceForItem( item );

			if( steamSrc != null ) {
				steamSrc.Load( tag );
			}
		}

		public override TagCompound Save( Item item ) {
			SteamSource steamSrc = SteamSource.GetSteamSourceForItem( item );

			return steamSrc?.Save() ?? new TagCompound();
		}


		////////////////

		public override void PostDrawInInventory(
					Item item,
					SpriteBatch spriteBatch,
					Vector2 position,
					Rectangle frame,
					Color drawColor,
					Color itemColor,
					Vector2 origin,
					float scale ) {
			SteamSource steamSrc = SteamSource.GetSteamSourceForItem( item );
			if( steamSrc == null ) {
				return;
			}

			float pressurePerc = steamSrc.TotalPressure / steamSrc.WaterCapacity;
			if( pressurePerc <= 0f ) {
				return;
			}

			//

			float offsetX = (float)frame.Width * scale * 0.5f;
			float offsetY = ((float)frame.Height - 8f) * scale;
			var offset = new Vector2( offsetX, offsetY );

			//

			Texture2D itemTex = Main.itemTexture[item.type];
			float itemScale = itemTex.Width > itemTex.Height
				? itemTex.Width
				: itemTex.Height;

			//

			this.DrawHeatBar( spriteBatch, position + offset, drawColor, scale, itemScale, pressurePerc );
		}


		public void DrawHeatBar(
					SpriteBatch spriteBatch,
					Vector2 position,
					Color color,
					float renderScale,
					float itemScale,
					float pressurePercent ) {
			Texture2D barBg = SteamArseMod.Instance.GetTexture( "Items/PressureBarOverlay_BG" );

			//

			float newBarScale = itemScale / (float)barBg.Width;

			//

			spriteBatch.Draw(
				texture: barBg,
				position: position,
				sourceRectangle: null,
				color: color,
				rotation: 0f,
				origin: new Vector2( (float)barBg.Width * 0.5f, 0 ),
				scale: renderScale * newBarScale,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);

			if( pressurePercent > 0f ) {
				this.DrawHeatBarOverlay( spriteBatch, position, color, renderScale, itemScale, pressurePercent );
			}
		}

		public void DrawHeatBarOverlay(
					SpriteBatch spriteBatch,
					Vector2 position,
					Color color,
					float renderScale,
					float itemScale,
					float pressurePercent ) {
			Texture2D barFg = SteamArseMod.Instance.GetTexture( "Items/PressureBarOverlay_FG" );

			//

			float newBarScale = itemScale / (float)barFg.Width;

			//

			spriteBatch.Draw(
				texture: barFg,
				position: position,
				sourceRectangle: new Rectangle( 0, 0, (int)(pressurePercent * (float)barFg.Width), barFg.Height ),
				color: color,
				rotation: 0f,
				origin: new Vector2( (float)barFg.Width * 0.5f, 0 ),
				scale: renderScale * newBarScale,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}
	}
}