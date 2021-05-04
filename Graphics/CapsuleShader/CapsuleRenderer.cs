using HololiveFightingGame.Collision;
using HololiveFightingGame.Input;
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

		public static void Draw(SpriteBatch spriteBatch, Transformation transformation)
		{
			foreach (CapsuleShaderData capsule in capsuleShaders)
			{
				Effect shader = Game1.capsuleRenderShader;

				shader.Parameters["Origin"].SetValue(DrawObject.TransformVector(capsule.capsule.origin, transformation));
				shader.Parameters["Length"].SetValue(capsule.capsule.length * -transformation.zoom);
				shader.Parameters["Radius"].SetValue(capsule.capsule.radius * transformation.zoom);
				shader.Parameters["DrawColor"].SetValue(capsule.color.ToVector3());
				shader.CurrentTechnique.Passes[0].Apply();
				// spriteBatch.Begin(effect: shader);
				// spriteBatch.End();
			}
			capsuleShaders.Clear();
		}
	}
}
