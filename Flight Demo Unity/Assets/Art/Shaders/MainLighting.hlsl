
void MainLight_half(float3 WorldPos, out half3 Direction, out half3 Color, out half Attenuation)
{
	
#if SHADERGRAPH_PREVIEW
	//If we're previewing this code in shadergraph, we dont have access to any light source, so we just provide some fake values.
	Direction = half3(0.5, 0.5, 0);
	Color = 1;
	Attenuation = 1;

#else
	//if we're actually rendering, then we can get the actual light values:

	//First we check if main directional light is casting shadows
#if SHADOWS_SCREEN
	half4 clipPos = TransformWorldToHClip(WorldPos);
	half4 shadowCoord = ComputeScreenPos(clipPos);
#else
	half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
#endif //end SHADOW_SCREEN


	Light mainLight = GetMainLight(shadowCoord);
	Direction = mainLight.direction;
	Color = mainLight.color;
	Attenuation = mainLight.distanceAttenuation * mainLight.shadowAttenuation;

#endif //end SHADERGRAPH_PREVIEW

}