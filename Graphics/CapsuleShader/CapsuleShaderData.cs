using HololiveFightingGame.Collision;
using Microsoft.Xna.Framework;

namespace HololiveFightingGame.Graphics.CapsuleShader
{
    public class CapsuleShaderData
	{
		public CapsuleShaderData(Capsule capsule, Color color)
        {
			this.capsule = capsule;
			this.color = color;
        }

		public Capsule capsule;
		public Color color;
	}
}
