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
				spriteBatch.DrawString(Game1.font, (parent.damages[i] / 10d).ToString("f1") + "%", new Vector2(100 + i * 100, 300), Color.White);
				spriteBatch.DrawString(Game1.font, "P" + (i + 1), TransformVector(Game1.gameState.fighters[i].position - new Vector2(0, 40), transformation), Color.White);
			}
            base.CustomDraw(spriteBatch, transformation);
        }
    }
}
