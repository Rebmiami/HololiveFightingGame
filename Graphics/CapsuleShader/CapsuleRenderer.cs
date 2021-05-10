using HololiveFightingGame.Collision;
using HololiveFightingGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HololiveFightingGame.Graphics.CapsuleShader
{
	public static class CapsuleRenderer
	{
		public static List<CapsuleShaderData> capsuleShaders;

		static CapsuleRenderer()
		{
			capsuleShaders = new List<CapsuleShaderData>();
		}

		public static void Draw(SpriteBatch spriteBatch, Transformation transformation, GraphicsDevice graphicsDevice)
		{
			Effect shader = Game1.capsuleRenderShader;

			Texture2D pixel = new Texture2D(graphicsDevice, 1, 1);
			pixel.SetData(new Color[] { new Color(0, 0, 0, 0) });

			foreach (CapsuleShaderData capsule in capsuleShaders)
			{
				if (capsule.transform)
                {
					shader.Parameters["Origin"].SetValue(DrawObject.TransformVector(capsule.capsule.origin, transformation));
					shader.Parameters["Length"].SetValue(capsule.capsule.length * -transformation.zoom);
					shader.Parameters["Radius"].SetValue(capsule.capsule.radius * transformation.zoom);
					shader.Parameters["DrawColor"].SetValue(capsule.color.ToVector3() * (1 / (capsule.color.A / 256f)));
					shader.Parameters["Opacity"].SetValue(capsule.color.A / 256f);
				}
				else
                {
					shader.Parameters["Origin"].SetValue(capsule.capsule.origin + Program.WindowBounds().Size.ToVector2() / 2);
					shader.Parameters["Length"].SetValue(capsule.capsule.length);
					shader.Parameters["Radius"].SetValue(capsule.capsule.radius);
					shader.Parameters["DrawColor"].SetValue(capsule.color.ToVector3() * (1 / (capsule.color.A / 256f)));
					shader.Parameters["Opacity"].SetValue(capsule.color.A / 256f);
				}

				// shader.CurrentTechnique.Passes[0].Apply();
				spriteBatch.Begin(effect: shader);
				spriteBatch.Draw(pixel, Program.WindowBounds(), Color.White);
				spriteBatch.End();
			}
			capsuleShaders.Clear();
		}
	}
}
