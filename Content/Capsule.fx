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
	float dot1 = dot(pixel - Origin, normal);
	float2 center = normal * dot1 + Origin;
	center = clamp(center, Origin, Origin + Length);

	if (distance(pixel, center) < Radius)
	{
		color *= 0.5;
		color += DrawColor * 0.5;
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