using HololiveFightingGame.Collision;
using Microsoft.Xna.Framework;

namespace HololiveFightingGame.Graphics.CapsuleShader
{
    public class CapsuleShaderData
	{
		public CapsuleShaderData(Capsule capsule, Color color, bool transform = true)
        {
			this.capsule = capsule;
			this.color = color;
			this.transform = transform;
        }

		public Capsule capsule;
		public Color color;

		public bool transform;
	}
}
