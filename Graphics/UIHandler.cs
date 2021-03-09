using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HololiveFightingGame.Graphics
{
	public class UIHandler
	{
		public int[] damages;

		public UIHandler()
		{
			uiDrawObject = new UIDrawObject();
			damages = new int[Game1.gameState.fighters.Length];
			uiDrawObject.parent = this;
			uiDrawObject.texture = new SlicedSprite(Game1.inGameUI);
			for (int i = 0; i < 10; i++)
			{
				int x = i % 5;
				int y = i / 5;
				uiDrawObject.texture.slices.Add("num_" + ((i + 1) % 10), new Rectangle(x * 10, y * 11, 10, 11));
				uiDrawObject.texture.slices.Add("numsmall_" + ((i + 1) % 10), new Rectangle(x * 6, 23 + y * 7, 6, 7));
			}
			uiDrawObject.texture.slices.Add("point", new Rectangle(31, 23, 4, 4));
			uiDrawObject.texture.slices.Add("percent", new Rectangle(37, 24, 11, 13));
			for (int i = 0; i < 4; i++)
			{
				uiDrawObject.texture.slices.Add("ind_" + i, new Rectangle(14 * i, 38, 14, 15));
			}
			GraphicsHandler.main.children["game"].children.Add("ui", uiDrawObject);
		}

		public void Update()
		{
			
		}

		public UIDrawObject uiDrawObject;
	}

	public class UIDrawObject : DrawObject
	{
		public UIHandler parent;

		public UIDrawObject(DrawObjectType type = DrawObjectType.Custom) : base(type)
		{
			this.type = DrawObjectType.Custom;
		}

		public override void CustomDraw(SpriteBatch spriteBatch, Transformation transformation)
		{
			spriteBatch.DrawString(Game1.font, Game1.language.GetLocalizedString("Test"), Vector2.Zero, Color.White);
			for (int i = 0; i < parent.damages.Length; i++)
			{
				spriteBatch.Draw(texture.texture, TransformVector(Game1.gameState.fighters[i].position - new Vector2(0, 40), transformation), texture.slices["ind_" + i], Color.White, 0, Vector2.Zero, transformation.zoom, SpriteEffects.None, 0);

				string damage = ((float)parent.damages[i] / 10).ToString("f1");
				Vector2 origin = new Vector2(200 + i * 200, 300);
				if (damage.Length == 2)
				{
					damage = "0.0";
				}
				for (int j = 0; j < damage.Length; j++)
				{
					if (damage[j] == '.')
						continue;
					float offset = origin.X - damage.Length * 16 + j * 16;
					if (j == damage.Length - 1)
					{
						spriteBatch.Draw(texture.texture, new Vector2(offset - 8, 308), texture.slices["numsmall_" + damage[j]], Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
					}
					else
					{
						spriteBatch.Draw(texture.texture, new Vector2(offset, 300), texture.slices["num_" + damage[j]], Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
					}
					spriteBatch.Draw(texture.texture, origin + new Vector2(-30, 14), texture.slices["point"], Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
					spriteBatch.Draw(texture.texture, origin + new Vector2(-14, -2), texture.slices["percent"], Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
				}

				//spriteBatch.DrawString(Game1.font, (parent.damages[i] / 10d).ToString("f1") + "%", new Vector2(100 + i * 100, 300), Color.White);
			}
			base.CustomDraw(spriteBatch, transformation);
		}
	}
}
