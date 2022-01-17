using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace SteampunkArsenal {
	class OffsetWrapperPlayerLayer : PlayerLayer {
		public static (OffsetWrapperPlayerLayer wrapBegin, OffsetWrapperPlayerLayer wrapClose) CreateLayers(
					PlayerLayer baseLayer,
					Func<float> shakeAmountGetter ) {
			var wrap1 = new OffsetWrapperPlayerLayer( baseLayer.Name+"1", shakeAmountGetter );
			var wrap2 = new OffsetWrapperPlayerLayer( baseLayer.Name+"2", shakeAmountGetter, wrap1 );
			return (wrap1, wrap2);
		}



		////////////////

		private OffsetWrapperPlayerLayer InitialWrappingLayer;

		private Vector2 OldBodyOrigin;

		private Func<float> ShakeAmountGetter;



		////////////////

		private OffsetWrapperPlayerLayer(
						string name,
						Func<float> shakeAmountGetter,
						OffsetWrapperPlayerLayer initialWrappingLayer = null
					) : base( SteamArseMod.Instance.Name, name+"Wrapper", layer => { } ) {
			this.ShakeAmountGetter = shakeAmountGetter;
			this.InitialWrappingLayer = initialWrappingLayer;
		}

		////

		public override void Draw( ref PlayerDrawInfo drawInfo ) {
			if( this.InitialWrappingLayer == null ) {
				this.PreProcessDrawInfo( ref drawInfo );
			} else {
				this.PostProcessDrawInfo( ref drawInfo );
			}
		}


		////////////////

		private void PreProcessDrawInfo( ref PlayerDrawInfo drawInfo ) {
			this.OldBodyOrigin = drawInfo.bodyOrigin;

			//

			float shake = this.ShakeAmountGetter();

			drawInfo.bodyOrigin += new Vector2(
				(Main.rand.NextFloat() * shake * 2f) - shake,
				(Main.rand.NextFloat() * shake * 2f) - shake
			);
			//drawInfo.position += new Vector2(
			//drawInfo.itemLocation += new Vector2(
		}

		private void PostProcessDrawInfo( ref PlayerDrawInfo drawInfo ) {
			drawInfo.bodyOrigin = this.InitialWrappingLayer.OldBodyOrigin;
		}
	}
}
