#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
float2 ViewportDimensions;
float2 Origin;
float2 Length;
float Radius;
float4 DrawColor;
float Opacity;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
	float2 pixel = floor(input.TextureCoordinates * ViewportDimensions);

	bool change = false;

	float2 normal = normalize(Length);
	// Length normal.

	float dot1 = dot(pixel - Origin, normal);
	float2 center = normal * dot1 + Origin;
	// Finds closest point on line to pixel position.

	float2 lower = float2(min(Origin.x, Origin.x + Length.x), min(Origin.y, Origin.y + Length.y));
	float2 upper = float2(max(Origin.x, Origin.x + Length.x), max(Origin.y, Origin.y + Length.y));
	center = clamp(center, lower, upper);
	// Clamps centerpoint to dimensions of capsule.

	if (distance(pixel, center) < Radius)
	{
		color *= 1 - Opacity;
		color += DrawColor * Opacity;
	}
	return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};