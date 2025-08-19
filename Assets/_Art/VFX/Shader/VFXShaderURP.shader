// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VFX/VFXShaderURP"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("CullMode", Float) = 0
		[Enum(AlphaBlend,10,Additive,1)]_Dst("Dst", Float) = 1
		[Enum(OFF,0,ON,1)]_ZWriteMode("ZWriteMode", Float) = 0
		[Enum(Less or Equal,4,Always,8)]_ZTestMode("ZTestMode", Float) = 4
		[HDR]_MainColor("MainColor", Color) = (1,1,1,1)
		[Header(Main)]_MainTex("MainTex", 2D) = "white" {}
		[Enum(Repeat,0,Clamp,1)]_MainWrapMode("MainWrapMode", Float) = 0
		[Enum(R,0,A,1)]_MainRA("MainRA", Float) = 0
		_MainUSpeed("MainUSpeed", Float) = 0
		_MainVSpeed("MainVSpeed", Float) = 0
		[Enum(UV,0,Polor,1)]_MainPolarMode("MainPolarMode", Float) = 0
		_MainPolarUSpeed("MainPolarUSpeed", Float) = 0
		_MainPolarVSpeed("MainPolarVSpeed", Float) = 0
		_MainScaleOffset("MainScaleOffset", Vector) = (1,1,0,0)
		_MainRotator("MainRotator", Float) = 0
		[Enum(OFF,0,ON,1)]_MainScaleMode("MainScaleMode", Float) = 0
		_MainScale("MainScale", Float) = 0
		[Header(Mask)]_MaskTex("MaskTex", 2D) = "white" {}
		[Toggle]_MaskCustom("MaskCustom", Float) = 0
		[Enum(Repeat,0,Clamp,1)]_MaskWrapMode("MaskWrapMode", Float) = 0
		_MaskUSpeed("MaskUSpeed", Float) = 0
		_MaskVSpeed("MaskVSpeed", Float) = 0
		[Enum(UV,0,Polor,1)]_MaskPolarMode("MaskPolarMode", Float) = 0
		_MaskPolarUSpeed("MaskPolarUSpeed", Float) = 0
		_MaskPolarVSpeed("MaskPolarVSpeed", Float) = 0
		_MaskScaleOffset("MaskScaleOffset", Vector) = (1,1,0,0)
		_MaskRotator("MaskRotator", Float) = 0
		[Enum(OFF,0,ON,1)]_MaskScaleMode("MaskScaleMode", Float) = 0
		_MaskScale("MaskScale", Float) = 0
		[Header(Noise)]_NoiseTex("NoiseTex", 2D) = "white" {}
		[Toggle]_NoiseCustom("NoiseCustom", Float) = 0
		_NoiseUSpeed("NoiseUSpeed", Float) = 0
		_NoiseVSpeed("NoiseVSpeed", Float) = 0
		[Enum(UV,0,Polor,1)]_NoisePolarMode("NoisePolarMode", Float) = 0
		_NoisePolarUSpeed("NoisePolarUSpeed", Float) = 0
		_NoisePolarVSpeed("NoisePolarVSpeed", Float) = 0
		_NoiseScaleOffset("NoiseScaleOffset", Vector) = (1,1,0,0)
		_NoiseRotator("NoiseRotator", Float) = 0
		[Enum(OFF,0,ON,1)]_NoiseScaleMode("NoiseScaleMode", Float) = 0
		_NoiseScale("NoiseScale", Float) = 0
		[Header(Dissolve)]_DissolveTex("DissolveTex", 2D) = "white" {}
		_DissolveRotator("DissolveRotator", Float) = 0
		_DissolveUSpeed("DissolveUSpeed", Float) = 0
		_DissolveVSpeed("DissolveVSpeed", Float) = 0
		_DissolveScale("DissolveScale", Float) = 0
		[Enum(OFF,0,ON,1)]_DissolveScaleMode("DissolveScaleMode", Float) = 0
		_DissolvePolarUSpeed("DissolvePolarUSpeed", Float) = 0
		_DissolvePolarVSpeed("DissolvePolarVSpeed", Float) = 0
		_DissolveScaleOffset("DissolveScaleOffset", Vector) = (1,1,0,0)
		_DissolveSmooth("DissolveSmooth", Range( 0 , 1)) = 0
		_DissolvePower("DissolvePower", Range( 0 , 1)) = 0
		[Enum(UV,0,Polor,1)]_DissolvePolarMode("DissolvePolarMode", Float) = 0
		[Enum(OFF,0,ON,1)]_DissolveSpeedCustom("DissolveSpeedCustom", Float) = 0
		[Enum(OFF,0,ON,1)]_DissolveCustom("DissolveCustom", Float) = 0
		_DissolveDivide("DissolveDivide", Range( 0 , 7)) = 1
		_NoisePower("NoisePower", Float) = 0
		[Enum(R,0,A,1)]_MaskRA("MaskRA", Float) = 0
		[Enum(R,0,A,1)]_NoiseRA("NoiseRA", Float) = 0
		[Enum(R,0,A,1)]_DissolveRA("DissolveRA", Float) = 0
		[Toggle(_DISSOLVESWITCH_ON)] _DissolveSwitch("DissolveSwitch", Float) = 0


		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25

		[HideInInspector] _QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector] _QueueControl("_QueueControl", Float) = -1

        [HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" "UniversalMaterialType"="Unlit" }

		Cull [_CullMode]
		AlphaToMask Off

		

		HLSLINCLUDE
		#pragma target 3.5
		#pragma prefer_hlslcc gles
		#pragma exclude_renderers xboxone xboxseries playstation ps4 ps5 switch // ensure rendering platforms toggle list is visible

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}

		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" "Queue"="Transparent+3000" }

			Blend SrcAlpha [_Dst], One OneMinusSrcAlpha
			ZWrite [_ZWriteMode]
			ZTest [_ZTestMode]
			Offset 0,0
			ColorMask RGBA

			

			HLSLPROGRAM

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 120108


			#pragma multi_compile _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3

			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ DEBUG_DISPLAY

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_UNLIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging3D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature _DISSOLVESWITCH_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
					float fogFactor : TEXCOORD2;
				#endif
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_color : COLOR;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MaskTex_ST;
			float4 _MainScaleOffset;
			float4 _DissolveTex_ST;
			float4 _MaskScaleOffset;
			float4 _NoiseTex_ST;
			float4 _MainTex_ST;
			float4 _NoiseScaleOffset;
			float4 _DissolveScaleOffset;
			float4 _MainColor;
			float _MaskWrapMode;
			float _MaskScaleMode;
			float _MaskScale;
			float _MaskPolarUSpeed;
			float _MaskPolarVSpeed;
			float _MaskPolarMode;
			float _MaskRotator;
			float _MaskRA;
			float _DissolveSmooth;
			float _MaskCustom;
			float _DissolveCustom;
			float _DissolveUSpeed;
			float _DissolveVSpeed;
			float _DissolveSpeedCustom;
			float _DissolveScale;
			float _DissolveScaleMode;
			float _DissolveRotator;
			float _DissolvePolarUSpeed;
			float _DissolvePolarVSpeed;
			float _DissolvePolarMode;
			float _DissolvePower;
			float _Dst;
			float _MainRA;
			float _MaskUSpeed;
			float _ZTestMode;
			float _CullMode;
			float _ZWriteMode;
			float _MainUSpeed;
			float _MainVSpeed;
			float _MainScale;
			float _MainScaleMode;
			float _MainRotator;
			float _MainWrapMode;
			float _MainPolarUSpeed;
			float _MainPolarVSpeed;
			float _MaskVSpeed;
			float _MainPolarMode;
			float _NoiseUSpeed;
			float _NoiseVSpeed;
			float _NoiseCustom;
			float _NoiseScale;
			float _NoiseScaleMode;
			float _NoiseRotator;
			float _NoisePolarUSpeed;
			float _NoisePolarVSpeed;
			float _NoisePolarMode;
			float _NoiseRA;
			float _DissolveRA;
			float _NoisePower;
			float _DissolveDivide;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			sampler2D _MaskTex;
			sampler2D _DissolveTex;


			
			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				o.ase_texcoord4 = v.ase_texcoord1;
				o.ase_color = v.ase_color;
				o.ase_texcoord5 = v.ase_texcoord2;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				#ifdef ASE_FOG
					o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif

				o.clipPos = positionCS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_color = v.ase_color;
				o.ase_texcoord2 = v.ase_texcoord2;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.worldPos;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 appendResult81_g17 = (float2(( _MainUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MainVSpeed )));
				float2 uv_MainTex = IN.ase_texcoord3.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 temp_output_83_0_g17 = uv_MainTex;
				float2 lerpResult87_g17 = lerp( ( appendResult81_g17 + temp_output_83_0_g17 ) , ( temp_output_83_0_g17 + float2( 0,0 ) ) , (float)0);
				float2 temp_cast_1 = (0.5).xx;
				float2 lerpResult93_g17 = lerp( lerpResult87_g17 , ( ( ( lerpResult87_g17 - temp_cast_1 ) * _MainScale ) + 0.5 ) , (float)(int)_MainScaleMode);
				float cos95_g17 = cos( ( ( _MainRotator * PI ) / 180.0 ) );
				float sin95_g17 = sin( ( ( _MainRotator * PI ) / 180.0 ) );
				float2 rotator95_g17 = mul( lerpResult93_g17 - float2( 0.5,0.5 ) , float2x2( cos95_g17 , -sin95_g17 , sin95_g17 , cos95_g17 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g17 = lerp( rotator95_g17 , saturate( rotator95_g17 ) , (float)(int)_MainWrapMode);
				float2 temp_output_12_0_g18 = (uv_MainTex*2.0 + -1.0);
				float2 break2_g18 = temp_output_12_0_g18;
				float2 appendResult9_g18 = (float2(pow( length( temp_output_12_0_g18 ) , 1.0 ) , ( ( atan2( break2_g18.y , break2_g18.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g18 = _MainScaleOffset;
				float2 appendResult24_g18 = (float2(break19_g18.x , break19_g18.y));
				float2 appendResult7_g18 = (float2(break19_g18.z , break19_g18.w));
				float2 appendResult6_g18 = (float2(( _MainPolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MainPolarVSpeed )));
				float2 lerpResult211 = lerp( lerpResult100_g17 , ( (appendResult9_g18*appendResult24_g18 + appendResult7_g18) + appendResult6_g18 ) , _MainPolarMode);
				float2 appendResult81_g9 = (float2(( _NoiseUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _NoiseVSpeed )));
				float2 uv_NoiseTex = IN.ase_texcoord3.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float2 temp_output_83_0_g9 = uv_NoiseTex;
				float4 texCoord259 = IN.ase_texcoord4;
				texCoord259.xy = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult258 = (float2(texCoord259.x , texCoord259.y));
				float2 lerpResult87_g9 = lerp( ( appendResult81_g9 + temp_output_83_0_g9 ) , ( temp_output_83_0_g9 + appendResult258 ) , (float)(int)_NoiseCustom);
				float2 temp_cast_8 = (0.5).xx;
				float2 lerpResult93_g9 = lerp( lerpResult87_g9 , ( ( ( lerpResult87_g9 - temp_cast_8 ) * _NoiseScale ) + 0.5 ) , (float)(int)_NoiseScaleMode);
				float cos95_g9 = cos( ( ( _NoiseRotator * PI ) / 180.0 ) );
				float sin95_g9 = sin( ( ( _NoiseRotator * PI ) / 180.0 ) );
				float2 rotator95_g9 = mul( lerpResult93_g9 - float2( 0.5,0.5 ) , float2x2( cos95_g9 , -sin95_g9 , sin95_g9 , cos95_g9 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g9 = lerp( rotator95_g9 , saturate( rotator95_g9 ) , (float)0);
				float2 temp_output_12_0_g16 = (uv_NoiseTex*2.0 + -1.0);
				float2 break2_g16 = temp_output_12_0_g16;
				float2 appendResult9_g16 = (float2(pow( length( temp_output_12_0_g16 ) , 1.0 ) , ( ( atan2( break2_g16.y , break2_g16.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g16 = _NoiseScaleOffset;
				float2 appendResult24_g16 = (float2(break19_g16.x , break19_g16.y));
				float2 appendResult7_g16 = (float2(break19_g16.z , break19_g16.w));
				float2 appendResult6_g16 = (float2(( _NoisePolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _NoisePolarVSpeed )));
				float2 lerpResult243 = lerp( lerpResult100_g9 , ( (appendResult9_g16*appendResult24_g16 + appendResult7_g16) + appendResult6_g16 ) , _NoisePolarMode);
				float4 tex2DNode254 = tex2D( _NoiseTex, lerpResult243 );
				float lerpResult330 = lerp( tex2DNode254.r , tex2DNode254.a , _NoiseRA);
				float Noise256 = ( _NoisePower * (-0.5 + (lerpResult330 - 0.0) * (0.5 - -0.5) / (1.0 - 0.0)) );
				float4 tex2DNode182 = tex2D( _MainTex, ( lerpResult211 + Noise256 ) );
				
				float lerpResult185 = lerp( tex2DNode182.r , tex2DNode182.a , _MainRA);
				float2 appendResult81_g22 = (float2(( _MaskUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MaskVSpeed )));
				float2 uv_MaskTex = IN.ase_texcoord3.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
				float2 temp_output_83_0_g22 = uv_MaskTex;
				float4 texCoord237 = IN.ase_texcoord4;
				texCoord237.xy = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult332 = (float2(texCoord237.z , texCoord237.w));
				float2 lerpResult87_g22 = lerp( ( appendResult81_g22 + temp_output_83_0_g22 ) , ( temp_output_83_0_g22 + appendResult332 ) , (float)(int)_MaskCustom);
				float2 temp_cast_15 = (0.5).xx;
				float2 lerpResult93_g22 = lerp( lerpResult87_g22 , ( ( ( lerpResult87_g22 - temp_cast_15 ) * _MaskScale ) + 0.5 ) , (float)(int)_MaskScaleMode);
				float cos95_g22 = cos( ( ( _MaskRotator * PI ) / 180.0 ) );
				float sin95_g22 = sin( ( ( _MaskRotator * PI ) / 180.0 ) );
				float2 rotator95_g22 = mul( lerpResult93_g22 - float2( 0.5,0.5 ) , float2x2( cos95_g22 , -sin95_g22 , sin95_g22 , cos95_g22 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g22 = lerp( rotator95_g22 , saturate( rotator95_g22 ) , (float)(int)_MaskWrapMode);
				float2 temp_output_12_0_g20 = (uv_MaskTex*2.0 + -1.0);
				float2 break2_g20 = temp_output_12_0_g20;
				float2 appendResult9_g20 = (float2(pow( length( temp_output_12_0_g20 ) , 1.0 ) , ( ( atan2( break2_g20.y , break2_g20.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g20 = _MaskScaleOffset;
				float2 appendResult24_g20 = (float2(break19_g20.x , break19_g20.y));
				float2 appendResult7_g20 = (float2(break19_g20.z , break19_g20.w));
				float2 appendResult6_g20 = (float2(( _MaskPolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MaskPolarVSpeed )));
				float2 lerpResult233 = lerp( lerpResult100_g22 , ( (appendResult9_g20*appendResult24_g20 + appendResult7_g20) + appendResult6_g20 ) , _MaskPolarMode);
				float4 tex2DNode234 = tex2D( _MaskTex, lerpResult233 );
				float lerpResult322 = lerp( tex2DNode234.r , tex2DNode234.a , _MaskRA);
				float Mask179 = lerpResult322;
				float2 appendResult314 = (float2(_DissolvePower , _DissolveSmooth));
				float4 texCoord285 = IN.ase_texcoord5;
				texCoord285.xy = IN.ase_texcoord5.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult315 = (float2(texCoord285.x , texCoord285.y));
				float2 lerpResult296 = lerp( appendResult314 , appendResult315 , _DissolveCustom);
				float2 break316 = lerpResult296;
				float2 appendResult81_g21 = (float2(( _DissolveUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _DissolveVSpeed )));
				float2 uv_DissolveTex = IN.ase_texcoord3.xy * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
				float2 temp_output_83_0_g21 = uv_DissolveTex;
				float2 appendResult284 = (float2(texCoord285.z , texCoord285.w));
				float2 lerpResult87_g21 = lerp( ( appendResult81_g21 + temp_output_83_0_g21 ) , ( temp_output_83_0_g21 + appendResult284 ) , (float)(int)_DissolveSpeedCustom);
				float2 temp_cast_23 = (0.5).xx;
				float2 lerpResult93_g21 = lerp( lerpResult87_g21 , ( ( ( lerpResult87_g21 - temp_cast_23 ) * _DissolveScale ) + 0.5 ) , (float)(int)_DissolveScaleMode);
				float cos95_g21 = cos( ( ( _DissolveRotator * PI ) / 180.0 ) );
				float sin95_g21 = sin( ( ( _DissolveRotator * PI ) / 180.0 ) );
				float2 rotator95_g21 = mul( lerpResult93_g21 - float2( 0.5,0.5 ) , float2x2( cos95_g21 , -sin95_g21 , sin95_g21 , cos95_g21 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g21 = lerp( rotator95_g21 , saturate( rotator95_g21 ) , (float)0);
				float2 temp_output_12_0_g15 = (uv_DissolveTex*2.0 + -1.0);
				float2 break2_g15 = temp_output_12_0_g15;
				float2 appendResult9_g15 = (float2(pow( length( temp_output_12_0_g15 ) , 1.0 ) , ( ( atan2( break2_g15.y , break2_g15.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g15 = _DissolveScaleOffset;
				float2 appendResult24_g15 = (float2(break19_g15.x , break19_g15.y));
				float2 appendResult7_g15 = (float2(break19_g15.z , break19_g15.w));
				float2 appendResult6_g15 = (float2(( _DissolvePolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _DissolvePolarVSpeed )));
				float2 lerpResult270 = lerp( lerpResult100_g21 , ( (appendResult9_g15*appendResult24_g15 + appendResult7_g15) + appendResult6_g15 ) , _DissolvePolarMode);
				float4 tex2DNode281 = tex2D( _DissolveTex, ( Noise256 + lerpResult270 ) );
				float lerpResult324 = lerp( tex2DNode281.r , tex2DNode281.a , _DissolveRA);
				float smoothstepResult309 = smoothstep( ( break316.x - break316.y ) , lerpResult296.x , saturate( ( ( lerpResult324 + ( lerpResult324 / _DissolveDivide ) ) / 2.0 ) ));
				#ifdef _DISSOLVESWITCH_ON
				float staticSwitch326 = smoothstepResult309;
				#else
				float staticSwitch326 = 1.0;
				#endif
				float Dissolve283 = staticSwitch326;
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = ( tex2DNode182 * _MainColor * IN.ase_color ).rgb;
				float Alpha = ( IN.ase_color.a * _MainColor.a * lerpResult185 * Mask179 * Dissolve283 );
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#if defined(_DBUFFER)
					ApplyDecalToBaseColor(IN.clipPos, Color);
				#endif

				#if defined(_ALPHAPREMULTIPLY_ON)
				Color *= Alpha;
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return half4( Color, Alpha );
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 120108


			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#pragma shader_feature _DISSOLVESWITCH_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MaskTex_ST;
			float4 _MainScaleOffset;
			float4 _DissolveTex_ST;
			float4 _MaskScaleOffset;
			float4 _NoiseTex_ST;
			float4 _MainTex_ST;
			float4 _NoiseScaleOffset;
			float4 _DissolveScaleOffset;
			float4 _MainColor;
			float _MaskWrapMode;
			float _MaskScaleMode;
			float _MaskScale;
			float _MaskPolarUSpeed;
			float _MaskPolarVSpeed;
			float _MaskPolarMode;
			float _MaskRotator;
			float _MaskRA;
			float _DissolveSmooth;
			float _MaskCustom;
			float _DissolveCustom;
			float _DissolveUSpeed;
			float _DissolveVSpeed;
			float _DissolveSpeedCustom;
			float _DissolveScale;
			float _DissolveScaleMode;
			float _DissolveRotator;
			float _DissolvePolarUSpeed;
			float _DissolvePolarVSpeed;
			float _DissolvePolarMode;
			float _DissolvePower;
			float _Dst;
			float _MainRA;
			float _MaskUSpeed;
			float _ZTestMode;
			float _CullMode;
			float _ZWriteMode;
			float _MainUSpeed;
			float _MainVSpeed;
			float _MainScale;
			float _MainScaleMode;
			float _MainRotator;
			float _MainWrapMode;
			float _MainPolarUSpeed;
			float _MainPolarVSpeed;
			float _MaskVSpeed;
			float _MainPolarMode;
			float _NoiseUSpeed;
			float _NoiseVSpeed;
			float _NoiseCustom;
			float _NoiseScale;
			float _NoiseScaleMode;
			float _NoiseRotator;
			float _NoisePolarUSpeed;
			float _NoisePolarVSpeed;
			float _NoisePolarMode;
			float _NoiseRA;
			float _DissolveRA;
			float _NoisePower;
			float _DissolveDivide;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			sampler2D _MaskTex;
			sampler2D _DissolveTex;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_color = v.ase_color;
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_texcoord3 = v.ase_texcoord1;
				o.ase_texcoord4 = v.ase_texcoord2;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.worldPos = positionWS;
				#endif

				o.clipPos = TransformWorldToHClip( positionWS );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord2 = v.ase_texcoord2;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.worldPos;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 appendResult81_g17 = (float2(( _MainUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MainVSpeed )));
				float2 uv_MainTex = IN.ase_texcoord2.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 temp_output_83_0_g17 = uv_MainTex;
				float2 lerpResult87_g17 = lerp( ( appendResult81_g17 + temp_output_83_0_g17 ) , ( temp_output_83_0_g17 + float2( 0,0 ) ) , (float)0);
				float2 temp_cast_1 = (0.5).xx;
				float2 lerpResult93_g17 = lerp( lerpResult87_g17 , ( ( ( lerpResult87_g17 - temp_cast_1 ) * _MainScale ) + 0.5 ) , (float)(int)_MainScaleMode);
				float cos95_g17 = cos( ( ( _MainRotator * PI ) / 180.0 ) );
				float sin95_g17 = sin( ( ( _MainRotator * PI ) / 180.0 ) );
				float2 rotator95_g17 = mul( lerpResult93_g17 - float2( 0.5,0.5 ) , float2x2( cos95_g17 , -sin95_g17 , sin95_g17 , cos95_g17 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g17 = lerp( rotator95_g17 , saturate( rotator95_g17 ) , (float)(int)_MainWrapMode);
				float2 temp_output_12_0_g18 = (uv_MainTex*2.0 + -1.0);
				float2 break2_g18 = temp_output_12_0_g18;
				float2 appendResult9_g18 = (float2(pow( length( temp_output_12_0_g18 ) , 1.0 ) , ( ( atan2( break2_g18.y , break2_g18.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g18 = _MainScaleOffset;
				float2 appendResult24_g18 = (float2(break19_g18.x , break19_g18.y));
				float2 appendResult7_g18 = (float2(break19_g18.z , break19_g18.w));
				float2 appendResult6_g18 = (float2(( _MainPolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MainPolarVSpeed )));
				float2 lerpResult211 = lerp( lerpResult100_g17 , ( (appendResult9_g18*appendResult24_g18 + appendResult7_g18) + appendResult6_g18 ) , _MainPolarMode);
				float2 appendResult81_g9 = (float2(( _NoiseUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _NoiseVSpeed )));
				float2 uv_NoiseTex = IN.ase_texcoord2.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float2 temp_output_83_0_g9 = uv_NoiseTex;
				float4 texCoord259 = IN.ase_texcoord3;
				texCoord259.xy = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult258 = (float2(texCoord259.x , texCoord259.y));
				float2 lerpResult87_g9 = lerp( ( appendResult81_g9 + temp_output_83_0_g9 ) , ( temp_output_83_0_g9 + appendResult258 ) , (float)(int)_NoiseCustom);
				float2 temp_cast_8 = (0.5).xx;
				float2 lerpResult93_g9 = lerp( lerpResult87_g9 , ( ( ( lerpResult87_g9 - temp_cast_8 ) * _NoiseScale ) + 0.5 ) , (float)(int)_NoiseScaleMode);
				float cos95_g9 = cos( ( ( _NoiseRotator * PI ) / 180.0 ) );
				float sin95_g9 = sin( ( ( _NoiseRotator * PI ) / 180.0 ) );
				float2 rotator95_g9 = mul( lerpResult93_g9 - float2( 0.5,0.5 ) , float2x2( cos95_g9 , -sin95_g9 , sin95_g9 , cos95_g9 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g9 = lerp( rotator95_g9 , saturate( rotator95_g9 ) , (float)0);
				float2 temp_output_12_0_g16 = (uv_NoiseTex*2.0 + -1.0);
				float2 break2_g16 = temp_output_12_0_g16;
				float2 appendResult9_g16 = (float2(pow( length( temp_output_12_0_g16 ) , 1.0 ) , ( ( atan2( break2_g16.y , break2_g16.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g16 = _NoiseScaleOffset;
				float2 appendResult24_g16 = (float2(break19_g16.x , break19_g16.y));
				float2 appendResult7_g16 = (float2(break19_g16.z , break19_g16.w));
				float2 appendResult6_g16 = (float2(( _NoisePolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _NoisePolarVSpeed )));
				float2 lerpResult243 = lerp( lerpResult100_g9 , ( (appendResult9_g16*appendResult24_g16 + appendResult7_g16) + appendResult6_g16 ) , _NoisePolarMode);
				float4 tex2DNode254 = tex2D( _NoiseTex, lerpResult243 );
				float lerpResult330 = lerp( tex2DNode254.r , tex2DNode254.a , _NoiseRA);
				float Noise256 = ( _NoisePower * (-0.5 + (lerpResult330 - 0.0) * (0.5 - -0.5) / (1.0 - 0.0)) );
				float4 tex2DNode182 = tex2D( _MainTex, ( lerpResult211 + Noise256 ) );
				float lerpResult185 = lerp( tex2DNode182.r , tex2DNode182.a , _MainRA);
				float2 appendResult81_g22 = (float2(( _MaskUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MaskVSpeed )));
				float2 uv_MaskTex = IN.ase_texcoord2.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
				float2 temp_output_83_0_g22 = uv_MaskTex;
				float4 texCoord237 = IN.ase_texcoord3;
				texCoord237.xy = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult332 = (float2(texCoord237.z , texCoord237.w));
				float2 lerpResult87_g22 = lerp( ( appendResult81_g22 + temp_output_83_0_g22 ) , ( temp_output_83_0_g22 + appendResult332 ) , (float)(int)_MaskCustom);
				float2 temp_cast_14 = (0.5).xx;
				float2 lerpResult93_g22 = lerp( lerpResult87_g22 , ( ( ( lerpResult87_g22 - temp_cast_14 ) * _MaskScale ) + 0.5 ) , (float)(int)_MaskScaleMode);
				float cos95_g22 = cos( ( ( _MaskRotator * PI ) / 180.0 ) );
				float sin95_g22 = sin( ( ( _MaskRotator * PI ) / 180.0 ) );
				float2 rotator95_g22 = mul( lerpResult93_g22 - float2( 0.5,0.5 ) , float2x2( cos95_g22 , -sin95_g22 , sin95_g22 , cos95_g22 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g22 = lerp( rotator95_g22 , saturate( rotator95_g22 ) , (float)(int)_MaskWrapMode);
				float2 temp_output_12_0_g20 = (uv_MaskTex*2.0 + -1.0);
				float2 break2_g20 = temp_output_12_0_g20;
				float2 appendResult9_g20 = (float2(pow( length( temp_output_12_0_g20 ) , 1.0 ) , ( ( atan2( break2_g20.y , break2_g20.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g20 = _MaskScaleOffset;
				float2 appendResult24_g20 = (float2(break19_g20.x , break19_g20.y));
				float2 appendResult7_g20 = (float2(break19_g20.z , break19_g20.w));
				float2 appendResult6_g20 = (float2(( _MaskPolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MaskPolarVSpeed )));
				float2 lerpResult233 = lerp( lerpResult100_g22 , ( (appendResult9_g20*appendResult24_g20 + appendResult7_g20) + appendResult6_g20 ) , _MaskPolarMode);
				float4 tex2DNode234 = tex2D( _MaskTex, lerpResult233 );
				float lerpResult322 = lerp( tex2DNode234.r , tex2DNode234.a , _MaskRA);
				float Mask179 = lerpResult322;
				float2 appendResult314 = (float2(_DissolvePower , _DissolveSmooth));
				float4 texCoord285 = IN.ase_texcoord4;
				texCoord285.xy = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult315 = (float2(texCoord285.x , texCoord285.y));
				float2 lerpResult296 = lerp( appendResult314 , appendResult315 , _DissolveCustom);
				float2 break316 = lerpResult296;
				float2 appendResult81_g21 = (float2(( _DissolveUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _DissolveVSpeed )));
				float2 uv_DissolveTex = IN.ase_texcoord2.xy * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
				float2 temp_output_83_0_g21 = uv_DissolveTex;
				float2 appendResult284 = (float2(texCoord285.z , texCoord285.w));
				float2 lerpResult87_g21 = lerp( ( appendResult81_g21 + temp_output_83_0_g21 ) , ( temp_output_83_0_g21 + appendResult284 ) , (float)(int)_DissolveSpeedCustom);
				float2 temp_cast_22 = (0.5).xx;
				float2 lerpResult93_g21 = lerp( lerpResult87_g21 , ( ( ( lerpResult87_g21 - temp_cast_22 ) * _DissolveScale ) + 0.5 ) , (float)(int)_DissolveScaleMode);
				float cos95_g21 = cos( ( ( _DissolveRotator * PI ) / 180.0 ) );
				float sin95_g21 = sin( ( ( _DissolveRotator * PI ) / 180.0 ) );
				float2 rotator95_g21 = mul( lerpResult93_g21 - float2( 0.5,0.5 ) , float2x2( cos95_g21 , -sin95_g21 , sin95_g21 , cos95_g21 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g21 = lerp( rotator95_g21 , saturate( rotator95_g21 ) , (float)0);
				float2 temp_output_12_0_g15 = (uv_DissolveTex*2.0 + -1.0);
				float2 break2_g15 = temp_output_12_0_g15;
				float2 appendResult9_g15 = (float2(pow( length( temp_output_12_0_g15 ) , 1.0 ) , ( ( atan2( break2_g15.y , break2_g15.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g15 = _DissolveScaleOffset;
				float2 appendResult24_g15 = (float2(break19_g15.x , break19_g15.y));
				float2 appendResult7_g15 = (float2(break19_g15.z , break19_g15.w));
				float2 appendResult6_g15 = (float2(( _DissolvePolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _DissolvePolarVSpeed )));
				float2 lerpResult270 = lerp( lerpResult100_g21 , ( (appendResult9_g15*appendResult24_g15 + appendResult7_g15) + appendResult6_g15 ) , _DissolvePolarMode);
				float4 tex2DNode281 = tex2D( _DissolveTex, ( Noise256 + lerpResult270 ) );
				float lerpResult324 = lerp( tex2DNode281.r , tex2DNode281.a , _DissolveRA);
				float smoothstepResult309 = smoothstep( ( break316.x - break316.y ) , lerpResult296.x , saturate( ( ( lerpResult324 + ( lerpResult324 / _DissolveDivide ) ) / 2.0 ) ));
				#ifdef _DISSOLVESWITCH_ON
				float staticSwitch326 = smoothstepResult309;
				#else
				float staticSwitch326 = 1.0;
				#endif
				float Dissolve283 = staticSwitch326;
				

				float Alpha = ( IN.ase_color.a * _MainColor.a * lerpResult185 * Mask179 * Dissolve283 );
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
            Name "SceneSelectionPass"
            Tags { "LightMode"="SceneSelectionPass" }

			Cull Off

			HLSLPROGRAM

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 120108


			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#pragma shader_feature _DISSOLVESWITCH_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MaskTex_ST;
			float4 _MainScaleOffset;
			float4 _DissolveTex_ST;
			float4 _MaskScaleOffset;
			float4 _NoiseTex_ST;
			float4 _MainTex_ST;
			float4 _NoiseScaleOffset;
			float4 _DissolveScaleOffset;
			float4 _MainColor;
			float _MaskWrapMode;
			float _MaskScaleMode;
			float _MaskScale;
			float _MaskPolarUSpeed;
			float _MaskPolarVSpeed;
			float _MaskPolarMode;
			float _MaskRotator;
			float _MaskRA;
			float _DissolveSmooth;
			float _MaskCustom;
			float _DissolveCustom;
			float _DissolveUSpeed;
			float _DissolveVSpeed;
			float _DissolveSpeedCustom;
			float _DissolveScale;
			float _DissolveScaleMode;
			float _DissolveRotator;
			float _DissolvePolarUSpeed;
			float _DissolvePolarVSpeed;
			float _DissolvePolarMode;
			float _DissolvePower;
			float _Dst;
			float _MainRA;
			float _MaskUSpeed;
			float _ZTestMode;
			float _CullMode;
			float _ZWriteMode;
			float _MainUSpeed;
			float _MainVSpeed;
			float _MainScale;
			float _MainScaleMode;
			float _MainRotator;
			float _MainWrapMode;
			float _MainPolarUSpeed;
			float _MainPolarVSpeed;
			float _MaskVSpeed;
			float _MainPolarMode;
			float _NoiseUSpeed;
			float _NoiseVSpeed;
			float _NoiseCustom;
			float _NoiseScale;
			float _NoiseScaleMode;
			float _NoiseRotator;
			float _NoisePolarUSpeed;
			float _NoisePolarVSpeed;
			float _NoisePolarMode;
			float _NoiseRA;
			float _DissolveRA;
			float _NoisePower;
			float _DissolveDivide;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			sampler2D _MaskTex;
			sampler2D _DissolveTex;


			
			int _ObjectId;
			int _PassValue;

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_color = v.ase_color;
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord2 = v.ase_texcoord2;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				o.clipPos = TransformWorldToHClip(positionWS);

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord2 = v.ase_texcoord2;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float2 appendResult81_g17 = (float2(( _MainUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MainVSpeed )));
				float2 uv_MainTex = IN.ase_texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 temp_output_83_0_g17 = uv_MainTex;
				float2 lerpResult87_g17 = lerp( ( appendResult81_g17 + temp_output_83_0_g17 ) , ( temp_output_83_0_g17 + float2( 0,0 ) ) , (float)0);
				float2 temp_cast_1 = (0.5).xx;
				float2 lerpResult93_g17 = lerp( lerpResult87_g17 , ( ( ( lerpResult87_g17 - temp_cast_1 ) * _MainScale ) + 0.5 ) , (float)(int)_MainScaleMode);
				float cos95_g17 = cos( ( ( _MainRotator * PI ) / 180.0 ) );
				float sin95_g17 = sin( ( ( _MainRotator * PI ) / 180.0 ) );
				float2 rotator95_g17 = mul( lerpResult93_g17 - float2( 0.5,0.5 ) , float2x2( cos95_g17 , -sin95_g17 , sin95_g17 , cos95_g17 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g17 = lerp( rotator95_g17 , saturate( rotator95_g17 ) , (float)(int)_MainWrapMode);
				float2 temp_output_12_0_g18 = (uv_MainTex*2.0 + -1.0);
				float2 break2_g18 = temp_output_12_0_g18;
				float2 appendResult9_g18 = (float2(pow( length( temp_output_12_0_g18 ) , 1.0 ) , ( ( atan2( break2_g18.y , break2_g18.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g18 = _MainScaleOffset;
				float2 appendResult24_g18 = (float2(break19_g18.x , break19_g18.y));
				float2 appendResult7_g18 = (float2(break19_g18.z , break19_g18.w));
				float2 appendResult6_g18 = (float2(( _MainPolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MainPolarVSpeed )));
				float2 lerpResult211 = lerp( lerpResult100_g17 , ( (appendResult9_g18*appendResult24_g18 + appendResult7_g18) + appendResult6_g18 ) , _MainPolarMode);
				float2 appendResult81_g9 = (float2(( _NoiseUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _NoiseVSpeed )));
				float2 uv_NoiseTex = IN.ase_texcoord.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float2 temp_output_83_0_g9 = uv_NoiseTex;
				float4 texCoord259 = IN.ase_texcoord1;
				texCoord259.xy = IN.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult258 = (float2(texCoord259.x , texCoord259.y));
				float2 lerpResult87_g9 = lerp( ( appendResult81_g9 + temp_output_83_0_g9 ) , ( temp_output_83_0_g9 + appendResult258 ) , (float)(int)_NoiseCustom);
				float2 temp_cast_8 = (0.5).xx;
				float2 lerpResult93_g9 = lerp( lerpResult87_g9 , ( ( ( lerpResult87_g9 - temp_cast_8 ) * _NoiseScale ) + 0.5 ) , (float)(int)_NoiseScaleMode);
				float cos95_g9 = cos( ( ( _NoiseRotator * PI ) / 180.0 ) );
				float sin95_g9 = sin( ( ( _NoiseRotator * PI ) / 180.0 ) );
				float2 rotator95_g9 = mul( lerpResult93_g9 - float2( 0.5,0.5 ) , float2x2( cos95_g9 , -sin95_g9 , sin95_g9 , cos95_g9 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g9 = lerp( rotator95_g9 , saturate( rotator95_g9 ) , (float)0);
				float2 temp_output_12_0_g16 = (uv_NoiseTex*2.0 + -1.0);
				float2 break2_g16 = temp_output_12_0_g16;
				float2 appendResult9_g16 = (float2(pow( length( temp_output_12_0_g16 ) , 1.0 ) , ( ( atan2( break2_g16.y , break2_g16.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g16 = _NoiseScaleOffset;
				float2 appendResult24_g16 = (float2(break19_g16.x , break19_g16.y));
				float2 appendResult7_g16 = (float2(break19_g16.z , break19_g16.w));
				float2 appendResult6_g16 = (float2(( _NoisePolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _NoisePolarVSpeed )));
				float2 lerpResult243 = lerp( lerpResult100_g9 , ( (appendResult9_g16*appendResult24_g16 + appendResult7_g16) + appendResult6_g16 ) , _NoisePolarMode);
				float4 tex2DNode254 = tex2D( _NoiseTex, lerpResult243 );
				float lerpResult330 = lerp( tex2DNode254.r , tex2DNode254.a , _NoiseRA);
				float Noise256 = ( _NoisePower * (-0.5 + (lerpResult330 - 0.0) * (0.5 - -0.5) / (1.0 - 0.0)) );
				float4 tex2DNode182 = tex2D( _MainTex, ( lerpResult211 + Noise256 ) );
				float lerpResult185 = lerp( tex2DNode182.r , tex2DNode182.a , _MainRA);
				float2 appendResult81_g22 = (float2(( _MaskUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MaskVSpeed )));
				float2 uv_MaskTex = IN.ase_texcoord.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
				float2 temp_output_83_0_g22 = uv_MaskTex;
				float4 texCoord237 = IN.ase_texcoord1;
				texCoord237.xy = IN.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult332 = (float2(texCoord237.z , texCoord237.w));
				float2 lerpResult87_g22 = lerp( ( appendResult81_g22 + temp_output_83_0_g22 ) , ( temp_output_83_0_g22 + appendResult332 ) , (float)(int)_MaskCustom);
				float2 temp_cast_14 = (0.5).xx;
				float2 lerpResult93_g22 = lerp( lerpResult87_g22 , ( ( ( lerpResult87_g22 - temp_cast_14 ) * _MaskScale ) + 0.5 ) , (float)(int)_MaskScaleMode);
				float cos95_g22 = cos( ( ( _MaskRotator * PI ) / 180.0 ) );
				float sin95_g22 = sin( ( ( _MaskRotator * PI ) / 180.0 ) );
				float2 rotator95_g22 = mul( lerpResult93_g22 - float2( 0.5,0.5 ) , float2x2( cos95_g22 , -sin95_g22 , sin95_g22 , cos95_g22 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g22 = lerp( rotator95_g22 , saturate( rotator95_g22 ) , (float)(int)_MaskWrapMode);
				float2 temp_output_12_0_g20 = (uv_MaskTex*2.0 + -1.0);
				float2 break2_g20 = temp_output_12_0_g20;
				float2 appendResult9_g20 = (float2(pow( length( temp_output_12_0_g20 ) , 1.0 ) , ( ( atan2( break2_g20.y , break2_g20.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g20 = _MaskScaleOffset;
				float2 appendResult24_g20 = (float2(break19_g20.x , break19_g20.y));
				float2 appendResult7_g20 = (float2(break19_g20.z , break19_g20.w));
				float2 appendResult6_g20 = (float2(( _MaskPolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MaskPolarVSpeed )));
				float2 lerpResult233 = lerp( lerpResult100_g22 , ( (appendResult9_g20*appendResult24_g20 + appendResult7_g20) + appendResult6_g20 ) , _MaskPolarMode);
				float4 tex2DNode234 = tex2D( _MaskTex, lerpResult233 );
				float lerpResult322 = lerp( tex2DNode234.r , tex2DNode234.a , _MaskRA);
				float Mask179 = lerpResult322;
				float2 appendResult314 = (float2(_DissolvePower , _DissolveSmooth));
				float4 texCoord285 = IN.ase_texcoord2;
				texCoord285.xy = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult315 = (float2(texCoord285.x , texCoord285.y));
				float2 lerpResult296 = lerp( appendResult314 , appendResult315 , _DissolveCustom);
				float2 break316 = lerpResult296;
				float2 appendResult81_g21 = (float2(( _DissolveUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _DissolveVSpeed )));
				float2 uv_DissolveTex = IN.ase_texcoord.xy * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
				float2 temp_output_83_0_g21 = uv_DissolveTex;
				float2 appendResult284 = (float2(texCoord285.z , texCoord285.w));
				float2 lerpResult87_g21 = lerp( ( appendResult81_g21 + temp_output_83_0_g21 ) , ( temp_output_83_0_g21 + appendResult284 ) , (float)(int)_DissolveSpeedCustom);
				float2 temp_cast_22 = (0.5).xx;
				float2 lerpResult93_g21 = lerp( lerpResult87_g21 , ( ( ( lerpResult87_g21 - temp_cast_22 ) * _DissolveScale ) + 0.5 ) , (float)(int)_DissolveScaleMode);
				float cos95_g21 = cos( ( ( _DissolveRotator * PI ) / 180.0 ) );
				float sin95_g21 = sin( ( ( _DissolveRotator * PI ) / 180.0 ) );
				float2 rotator95_g21 = mul( lerpResult93_g21 - float2( 0.5,0.5 ) , float2x2( cos95_g21 , -sin95_g21 , sin95_g21 , cos95_g21 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g21 = lerp( rotator95_g21 , saturate( rotator95_g21 ) , (float)0);
				float2 temp_output_12_0_g15 = (uv_DissolveTex*2.0 + -1.0);
				float2 break2_g15 = temp_output_12_0_g15;
				float2 appendResult9_g15 = (float2(pow( length( temp_output_12_0_g15 ) , 1.0 ) , ( ( atan2( break2_g15.y , break2_g15.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g15 = _DissolveScaleOffset;
				float2 appendResult24_g15 = (float2(break19_g15.x , break19_g15.y));
				float2 appendResult7_g15 = (float2(break19_g15.z , break19_g15.w));
				float2 appendResult6_g15 = (float2(( _DissolvePolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _DissolvePolarVSpeed )));
				float2 lerpResult270 = lerp( lerpResult100_g21 , ( (appendResult9_g15*appendResult24_g15 + appendResult7_g15) + appendResult6_g15 ) , _DissolvePolarMode);
				float4 tex2DNode281 = tex2D( _DissolveTex, ( Noise256 + lerpResult270 ) );
				float lerpResult324 = lerp( tex2DNode281.r , tex2DNode281.a , _DissolveRA);
				float smoothstepResult309 = smoothstep( ( break316.x - break316.y ) , lerpResult296.x , saturate( ( ( lerpResult324 + ( lerpResult324 / _DissolveDivide ) ) / 2.0 ) ));
				#ifdef _DISSOLVESWITCH_ON
				float staticSwitch326 = smoothstepResult309;
				#else
				float staticSwitch326 = 1.0;
				#endif
				float Dissolve283 = staticSwitch326;
				

				surfaceDescription.Alpha = ( IN.ase_color.a * _MainColor.a * lerpResult185 * Mask179 * Dissolve283 );
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				return outColor;
			}
			ENDHLSL
		}

		
		Pass
		{
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }

			HLSLPROGRAM

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 120108


			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#pragma shader_feature _DISSOLVESWITCH_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MaskTex_ST;
			float4 _MainScaleOffset;
			float4 _DissolveTex_ST;
			float4 _MaskScaleOffset;
			float4 _NoiseTex_ST;
			float4 _MainTex_ST;
			float4 _NoiseScaleOffset;
			float4 _DissolveScaleOffset;
			float4 _MainColor;
			float _MaskWrapMode;
			float _MaskScaleMode;
			float _MaskScale;
			float _MaskPolarUSpeed;
			float _MaskPolarVSpeed;
			float _MaskPolarMode;
			float _MaskRotator;
			float _MaskRA;
			float _DissolveSmooth;
			float _MaskCustom;
			float _DissolveCustom;
			float _DissolveUSpeed;
			float _DissolveVSpeed;
			float _DissolveSpeedCustom;
			float _DissolveScale;
			float _DissolveScaleMode;
			float _DissolveRotator;
			float _DissolvePolarUSpeed;
			float _DissolvePolarVSpeed;
			float _DissolvePolarMode;
			float _DissolvePower;
			float _Dst;
			float _MainRA;
			float _MaskUSpeed;
			float _ZTestMode;
			float _CullMode;
			float _ZWriteMode;
			float _MainUSpeed;
			float _MainVSpeed;
			float _MainScale;
			float _MainScaleMode;
			float _MainRotator;
			float _MainWrapMode;
			float _MainPolarUSpeed;
			float _MainPolarVSpeed;
			float _MaskVSpeed;
			float _MainPolarMode;
			float _NoiseUSpeed;
			float _NoiseVSpeed;
			float _NoiseCustom;
			float _NoiseScale;
			float _NoiseScaleMode;
			float _NoiseRotator;
			float _NoisePolarUSpeed;
			float _NoisePolarVSpeed;
			float _NoisePolarMode;
			float _NoiseRA;
			float _DissolveRA;
			float _NoisePower;
			float _DissolveDivide;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			sampler2D _MaskTex;
			sampler2D _DissolveTex;


			
			float4 _SelectionID;


			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_color = v.ase_color;
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord2 = v.ase_texcoord2;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				o.clipPos = TransformWorldToHClip(positionWS);
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord2 = v.ase_texcoord2;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float2 appendResult81_g17 = (float2(( _MainUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MainVSpeed )));
				float2 uv_MainTex = IN.ase_texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 temp_output_83_0_g17 = uv_MainTex;
				float2 lerpResult87_g17 = lerp( ( appendResult81_g17 + temp_output_83_0_g17 ) , ( temp_output_83_0_g17 + float2( 0,0 ) ) , (float)0);
				float2 temp_cast_1 = (0.5).xx;
				float2 lerpResult93_g17 = lerp( lerpResult87_g17 , ( ( ( lerpResult87_g17 - temp_cast_1 ) * _MainScale ) + 0.5 ) , (float)(int)_MainScaleMode);
				float cos95_g17 = cos( ( ( _MainRotator * PI ) / 180.0 ) );
				float sin95_g17 = sin( ( ( _MainRotator * PI ) / 180.0 ) );
				float2 rotator95_g17 = mul( lerpResult93_g17 - float2( 0.5,0.5 ) , float2x2( cos95_g17 , -sin95_g17 , sin95_g17 , cos95_g17 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g17 = lerp( rotator95_g17 , saturate( rotator95_g17 ) , (float)(int)_MainWrapMode);
				float2 temp_output_12_0_g18 = (uv_MainTex*2.0 + -1.0);
				float2 break2_g18 = temp_output_12_0_g18;
				float2 appendResult9_g18 = (float2(pow( length( temp_output_12_0_g18 ) , 1.0 ) , ( ( atan2( break2_g18.y , break2_g18.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g18 = _MainScaleOffset;
				float2 appendResult24_g18 = (float2(break19_g18.x , break19_g18.y));
				float2 appendResult7_g18 = (float2(break19_g18.z , break19_g18.w));
				float2 appendResult6_g18 = (float2(( _MainPolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MainPolarVSpeed )));
				float2 lerpResult211 = lerp( lerpResult100_g17 , ( (appendResult9_g18*appendResult24_g18 + appendResult7_g18) + appendResult6_g18 ) , _MainPolarMode);
				float2 appendResult81_g9 = (float2(( _NoiseUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _NoiseVSpeed )));
				float2 uv_NoiseTex = IN.ase_texcoord.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float2 temp_output_83_0_g9 = uv_NoiseTex;
				float4 texCoord259 = IN.ase_texcoord1;
				texCoord259.xy = IN.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult258 = (float2(texCoord259.x , texCoord259.y));
				float2 lerpResult87_g9 = lerp( ( appendResult81_g9 + temp_output_83_0_g9 ) , ( temp_output_83_0_g9 + appendResult258 ) , (float)(int)_NoiseCustom);
				float2 temp_cast_8 = (0.5).xx;
				float2 lerpResult93_g9 = lerp( lerpResult87_g9 , ( ( ( lerpResult87_g9 - temp_cast_8 ) * _NoiseScale ) + 0.5 ) , (float)(int)_NoiseScaleMode);
				float cos95_g9 = cos( ( ( _NoiseRotator * PI ) / 180.0 ) );
				float sin95_g9 = sin( ( ( _NoiseRotator * PI ) / 180.0 ) );
				float2 rotator95_g9 = mul( lerpResult93_g9 - float2( 0.5,0.5 ) , float2x2( cos95_g9 , -sin95_g9 , sin95_g9 , cos95_g9 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g9 = lerp( rotator95_g9 , saturate( rotator95_g9 ) , (float)0);
				float2 temp_output_12_0_g16 = (uv_NoiseTex*2.0 + -1.0);
				float2 break2_g16 = temp_output_12_0_g16;
				float2 appendResult9_g16 = (float2(pow( length( temp_output_12_0_g16 ) , 1.0 ) , ( ( atan2( break2_g16.y , break2_g16.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g16 = _NoiseScaleOffset;
				float2 appendResult24_g16 = (float2(break19_g16.x , break19_g16.y));
				float2 appendResult7_g16 = (float2(break19_g16.z , break19_g16.w));
				float2 appendResult6_g16 = (float2(( _NoisePolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _NoisePolarVSpeed )));
				float2 lerpResult243 = lerp( lerpResult100_g9 , ( (appendResult9_g16*appendResult24_g16 + appendResult7_g16) + appendResult6_g16 ) , _NoisePolarMode);
				float4 tex2DNode254 = tex2D( _NoiseTex, lerpResult243 );
				float lerpResult330 = lerp( tex2DNode254.r , tex2DNode254.a , _NoiseRA);
				float Noise256 = ( _NoisePower * (-0.5 + (lerpResult330 - 0.0) * (0.5 - -0.5) / (1.0 - 0.0)) );
				float4 tex2DNode182 = tex2D( _MainTex, ( lerpResult211 + Noise256 ) );
				float lerpResult185 = lerp( tex2DNode182.r , tex2DNode182.a , _MainRA);
				float2 appendResult81_g22 = (float2(( _MaskUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MaskVSpeed )));
				float2 uv_MaskTex = IN.ase_texcoord.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
				float2 temp_output_83_0_g22 = uv_MaskTex;
				float4 texCoord237 = IN.ase_texcoord1;
				texCoord237.xy = IN.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult332 = (float2(texCoord237.z , texCoord237.w));
				float2 lerpResult87_g22 = lerp( ( appendResult81_g22 + temp_output_83_0_g22 ) , ( temp_output_83_0_g22 + appendResult332 ) , (float)(int)_MaskCustom);
				float2 temp_cast_14 = (0.5).xx;
				float2 lerpResult93_g22 = lerp( lerpResult87_g22 , ( ( ( lerpResult87_g22 - temp_cast_14 ) * _MaskScale ) + 0.5 ) , (float)(int)_MaskScaleMode);
				float cos95_g22 = cos( ( ( _MaskRotator * PI ) / 180.0 ) );
				float sin95_g22 = sin( ( ( _MaskRotator * PI ) / 180.0 ) );
				float2 rotator95_g22 = mul( lerpResult93_g22 - float2( 0.5,0.5 ) , float2x2( cos95_g22 , -sin95_g22 , sin95_g22 , cos95_g22 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g22 = lerp( rotator95_g22 , saturate( rotator95_g22 ) , (float)(int)_MaskWrapMode);
				float2 temp_output_12_0_g20 = (uv_MaskTex*2.0 + -1.0);
				float2 break2_g20 = temp_output_12_0_g20;
				float2 appendResult9_g20 = (float2(pow( length( temp_output_12_0_g20 ) , 1.0 ) , ( ( atan2( break2_g20.y , break2_g20.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g20 = _MaskScaleOffset;
				float2 appendResult24_g20 = (float2(break19_g20.x , break19_g20.y));
				float2 appendResult7_g20 = (float2(break19_g20.z , break19_g20.w));
				float2 appendResult6_g20 = (float2(( _MaskPolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MaskPolarVSpeed )));
				float2 lerpResult233 = lerp( lerpResult100_g22 , ( (appendResult9_g20*appendResult24_g20 + appendResult7_g20) + appendResult6_g20 ) , _MaskPolarMode);
				float4 tex2DNode234 = tex2D( _MaskTex, lerpResult233 );
				float lerpResult322 = lerp( tex2DNode234.r , tex2DNode234.a , _MaskRA);
				float Mask179 = lerpResult322;
				float2 appendResult314 = (float2(_DissolvePower , _DissolveSmooth));
				float4 texCoord285 = IN.ase_texcoord2;
				texCoord285.xy = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult315 = (float2(texCoord285.x , texCoord285.y));
				float2 lerpResult296 = lerp( appendResult314 , appendResult315 , _DissolveCustom);
				float2 break316 = lerpResult296;
				float2 appendResult81_g21 = (float2(( _DissolveUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _DissolveVSpeed )));
				float2 uv_DissolveTex = IN.ase_texcoord.xy * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
				float2 temp_output_83_0_g21 = uv_DissolveTex;
				float2 appendResult284 = (float2(texCoord285.z , texCoord285.w));
				float2 lerpResult87_g21 = lerp( ( appendResult81_g21 + temp_output_83_0_g21 ) , ( temp_output_83_0_g21 + appendResult284 ) , (float)(int)_DissolveSpeedCustom);
				float2 temp_cast_22 = (0.5).xx;
				float2 lerpResult93_g21 = lerp( lerpResult87_g21 , ( ( ( lerpResult87_g21 - temp_cast_22 ) * _DissolveScale ) + 0.5 ) , (float)(int)_DissolveScaleMode);
				float cos95_g21 = cos( ( ( _DissolveRotator * PI ) / 180.0 ) );
				float sin95_g21 = sin( ( ( _DissolveRotator * PI ) / 180.0 ) );
				float2 rotator95_g21 = mul( lerpResult93_g21 - float2( 0.5,0.5 ) , float2x2( cos95_g21 , -sin95_g21 , sin95_g21 , cos95_g21 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g21 = lerp( rotator95_g21 , saturate( rotator95_g21 ) , (float)0);
				float2 temp_output_12_0_g15 = (uv_DissolveTex*2.0 + -1.0);
				float2 break2_g15 = temp_output_12_0_g15;
				float2 appendResult9_g15 = (float2(pow( length( temp_output_12_0_g15 ) , 1.0 ) , ( ( atan2( break2_g15.y , break2_g15.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g15 = _DissolveScaleOffset;
				float2 appendResult24_g15 = (float2(break19_g15.x , break19_g15.y));
				float2 appendResult7_g15 = (float2(break19_g15.z , break19_g15.w));
				float2 appendResult6_g15 = (float2(( _DissolvePolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _DissolvePolarVSpeed )));
				float2 lerpResult270 = lerp( lerpResult100_g21 , ( (appendResult9_g15*appendResult24_g15 + appendResult7_g15) + appendResult6_g15 ) , _DissolvePolarMode);
				float4 tex2DNode281 = tex2D( _DissolveTex, ( Noise256 + lerpResult270 ) );
				float lerpResult324 = lerp( tex2DNode281.r , tex2DNode281.a , _DissolveRA);
				float smoothstepResult309 = smoothstep( ( break316.x - break316.y ) , lerpResult296.x , saturate( ( ( lerpResult324 + ( lerpResult324 / _DissolveDivide ) ) / 2.0 ) ));
				#ifdef _DISSOLVESWITCH_ON
				float staticSwitch326 = smoothstepResult309;
				#else
				float staticSwitch326 = 1.0;
				#endif
				float Dissolve283 = staticSwitch326;
				

				surfaceDescription.Alpha = ( IN.ase_color.a * _MainColor.a * lerpResult185 * Mask179 * Dissolve283 );
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = 0;
				outColor = _SelectionID;

				return outColor;
			}

			ENDHLSL
		}

		
		Pass
		{
			
            Name "DepthNormals"
            Tags { "LightMode"="DepthNormalsOnly" }

			ZTest LEqual
			ZWrite On


			HLSLPROGRAM

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 120108


			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define VARYINGS_NEED_NORMAL_WS

			#define SHADERPASS SHADERPASS_DEPTHNORMALSONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#pragma shader_feature _DISSOLVESWITCH_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MaskTex_ST;
			float4 _MainScaleOffset;
			float4 _DissolveTex_ST;
			float4 _MaskScaleOffset;
			float4 _NoiseTex_ST;
			float4 _MainTex_ST;
			float4 _NoiseScaleOffset;
			float4 _DissolveScaleOffset;
			float4 _MainColor;
			float _MaskWrapMode;
			float _MaskScaleMode;
			float _MaskScale;
			float _MaskPolarUSpeed;
			float _MaskPolarVSpeed;
			float _MaskPolarMode;
			float _MaskRotator;
			float _MaskRA;
			float _DissolveSmooth;
			float _MaskCustom;
			float _DissolveCustom;
			float _DissolveUSpeed;
			float _DissolveVSpeed;
			float _DissolveSpeedCustom;
			float _DissolveScale;
			float _DissolveScaleMode;
			float _DissolveRotator;
			float _DissolvePolarUSpeed;
			float _DissolvePolarVSpeed;
			float _DissolvePolarMode;
			float _DissolvePower;
			float _Dst;
			float _MainRA;
			float _MaskUSpeed;
			float _ZTestMode;
			float _CullMode;
			float _ZWriteMode;
			float _MainUSpeed;
			float _MainVSpeed;
			float _MainScale;
			float _MainScaleMode;
			float _MainRotator;
			float _MainWrapMode;
			float _MainPolarUSpeed;
			float _MainPolarVSpeed;
			float _MaskVSpeed;
			float _MainPolarMode;
			float _NoiseUSpeed;
			float _NoiseVSpeed;
			float _NoiseCustom;
			float _NoiseScale;
			float _NoiseScaleMode;
			float _NoiseRotator;
			float _NoisePolarUSpeed;
			float _NoisePolarVSpeed;
			float _NoisePolarMode;
			float _NoiseRA;
			float _DissolveRA;
			float _NoisePower;
			float _DissolveDivide;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			sampler2D _MaskTex;
			sampler2D _DissolveTex;


			
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_color = v.ase_color;
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_texcoord2 = v.ase_texcoord1;
				o.ase_texcoord3 = v.ase_texcoord2;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 normalWS = TransformObjectToWorldNormal(v.ase_normal);

				o.clipPos = TransformWorldToHClip(positionWS);
				o.normalWS.xyz =  normalWS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord2 = v.ase_texcoord2;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float2 appendResult81_g17 = (float2(( _MainUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MainVSpeed )));
				float2 uv_MainTex = IN.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 temp_output_83_0_g17 = uv_MainTex;
				float2 lerpResult87_g17 = lerp( ( appendResult81_g17 + temp_output_83_0_g17 ) , ( temp_output_83_0_g17 + float2( 0,0 ) ) , (float)0);
				float2 temp_cast_1 = (0.5).xx;
				float2 lerpResult93_g17 = lerp( lerpResult87_g17 , ( ( ( lerpResult87_g17 - temp_cast_1 ) * _MainScale ) + 0.5 ) , (float)(int)_MainScaleMode);
				float cos95_g17 = cos( ( ( _MainRotator * PI ) / 180.0 ) );
				float sin95_g17 = sin( ( ( _MainRotator * PI ) / 180.0 ) );
				float2 rotator95_g17 = mul( lerpResult93_g17 - float2( 0.5,0.5 ) , float2x2( cos95_g17 , -sin95_g17 , sin95_g17 , cos95_g17 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g17 = lerp( rotator95_g17 , saturate( rotator95_g17 ) , (float)(int)_MainWrapMode);
				float2 temp_output_12_0_g18 = (uv_MainTex*2.0 + -1.0);
				float2 break2_g18 = temp_output_12_0_g18;
				float2 appendResult9_g18 = (float2(pow( length( temp_output_12_0_g18 ) , 1.0 ) , ( ( atan2( break2_g18.y , break2_g18.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g18 = _MainScaleOffset;
				float2 appendResult24_g18 = (float2(break19_g18.x , break19_g18.y));
				float2 appendResult7_g18 = (float2(break19_g18.z , break19_g18.w));
				float2 appendResult6_g18 = (float2(( _MainPolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MainPolarVSpeed )));
				float2 lerpResult211 = lerp( lerpResult100_g17 , ( (appendResult9_g18*appendResult24_g18 + appendResult7_g18) + appendResult6_g18 ) , _MainPolarMode);
				float2 appendResult81_g9 = (float2(( _NoiseUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _NoiseVSpeed )));
				float2 uv_NoiseTex = IN.ase_texcoord1.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float2 temp_output_83_0_g9 = uv_NoiseTex;
				float4 texCoord259 = IN.ase_texcoord2;
				texCoord259.xy = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult258 = (float2(texCoord259.x , texCoord259.y));
				float2 lerpResult87_g9 = lerp( ( appendResult81_g9 + temp_output_83_0_g9 ) , ( temp_output_83_0_g9 + appendResult258 ) , (float)(int)_NoiseCustom);
				float2 temp_cast_8 = (0.5).xx;
				float2 lerpResult93_g9 = lerp( lerpResult87_g9 , ( ( ( lerpResult87_g9 - temp_cast_8 ) * _NoiseScale ) + 0.5 ) , (float)(int)_NoiseScaleMode);
				float cos95_g9 = cos( ( ( _NoiseRotator * PI ) / 180.0 ) );
				float sin95_g9 = sin( ( ( _NoiseRotator * PI ) / 180.0 ) );
				float2 rotator95_g9 = mul( lerpResult93_g9 - float2( 0.5,0.5 ) , float2x2( cos95_g9 , -sin95_g9 , sin95_g9 , cos95_g9 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g9 = lerp( rotator95_g9 , saturate( rotator95_g9 ) , (float)0);
				float2 temp_output_12_0_g16 = (uv_NoiseTex*2.0 + -1.0);
				float2 break2_g16 = temp_output_12_0_g16;
				float2 appendResult9_g16 = (float2(pow( length( temp_output_12_0_g16 ) , 1.0 ) , ( ( atan2( break2_g16.y , break2_g16.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g16 = _NoiseScaleOffset;
				float2 appendResult24_g16 = (float2(break19_g16.x , break19_g16.y));
				float2 appendResult7_g16 = (float2(break19_g16.z , break19_g16.w));
				float2 appendResult6_g16 = (float2(( _NoisePolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _NoisePolarVSpeed )));
				float2 lerpResult243 = lerp( lerpResult100_g9 , ( (appendResult9_g16*appendResult24_g16 + appendResult7_g16) + appendResult6_g16 ) , _NoisePolarMode);
				float4 tex2DNode254 = tex2D( _NoiseTex, lerpResult243 );
				float lerpResult330 = lerp( tex2DNode254.r , tex2DNode254.a , _NoiseRA);
				float Noise256 = ( _NoisePower * (-0.5 + (lerpResult330 - 0.0) * (0.5 - -0.5) / (1.0 - 0.0)) );
				float4 tex2DNode182 = tex2D( _MainTex, ( lerpResult211 + Noise256 ) );
				float lerpResult185 = lerp( tex2DNode182.r , tex2DNode182.a , _MainRA);
				float2 appendResult81_g22 = (float2(( _MaskUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MaskVSpeed )));
				float2 uv_MaskTex = IN.ase_texcoord1.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
				float2 temp_output_83_0_g22 = uv_MaskTex;
				float4 texCoord237 = IN.ase_texcoord2;
				texCoord237.xy = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult332 = (float2(texCoord237.z , texCoord237.w));
				float2 lerpResult87_g22 = lerp( ( appendResult81_g22 + temp_output_83_0_g22 ) , ( temp_output_83_0_g22 + appendResult332 ) , (float)(int)_MaskCustom);
				float2 temp_cast_14 = (0.5).xx;
				float2 lerpResult93_g22 = lerp( lerpResult87_g22 , ( ( ( lerpResult87_g22 - temp_cast_14 ) * _MaskScale ) + 0.5 ) , (float)(int)_MaskScaleMode);
				float cos95_g22 = cos( ( ( _MaskRotator * PI ) / 180.0 ) );
				float sin95_g22 = sin( ( ( _MaskRotator * PI ) / 180.0 ) );
				float2 rotator95_g22 = mul( lerpResult93_g22 - float2( 0.5,0.5 ) , float2x2( cos95_g22 , -sin95_g22 , sin95_g22 , cos95_g22 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g22 = lerp( rotator95_g22 , saturate( rotator95_g22 ) , (float)(int)_MaskWrapMode);
				float2 temp_output_12_0_g20 = (uv_MaskTex*2.0 + -1.0);
				float2 break2_g20 = temp_output_12_0_g20;
				float2 appendResult9_g20 = (float2(pow( length( temp_output_12_0_g20 ) , 1.0 ) , ( ( atan2( break2_g20.y , break2_g20.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g20 = _MaskScaleOffset;
				float2 appendResult24_g20 = (float2(break19_g20.x , break19_g20.y));
				float2 appendResult7_g20 = (float2(break19_g20.z , break19_g20.w));
				float2 appendResult6_g20 = (float2(( _MaskPolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MaskPolarVSpeed )));
				float2 lerpResult233 = lerp( lerpResult100_g22 , ( (appendResult9_g20*appendResult24_g20 + appendResult7_g20) + appendResult6_g20 ) , _MaskPolarMode);
				float4 tex2DNode234 = tex2D( _MaskTex, lerpResult233 );
				float lerpResult322 = lerp( tex2DNode234.r , tex2DNode234.a , _MaskRA);
				float Mask179 = lerpResult322;
				float2 appendResult314 = (float2(_DissolvePower , _DissolveSmooth));
				float4 texCoord285 = IN.ase_texcoord3;
				texCoord285.xy = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult315 = (float2(texCoord285.x , texCoord285.y));
				float2 lerpResult296 = lerp( appendResult314 , appendResult315 , _DissolveCustom);
				float2 break316 = lerpResult296;
				float2 appendResult81_g21 = (float2(( _DissolveUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _DissolveVSpeed )));
				float2 uv_DissolveTex = IN.ase_texcoord1.xy * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
				float2 temp_output_83_0_g21 = uv_DissolveTex;
				float2 appendResult284 = (float2(texCoord285.z , texCoord285.w));
				float2 lerpResult87_g21 = lerp( ( appendResult81_g21 + temp_output_83_0_g21 ) , ( temp_output_83_0_g21 + appendResult284 ) , (float)(int)_DissolveSpeedCustom);
				float2 temp_cast_22 = (0.5).xx;
				float2 lerpResult93_g21 = lerp( lerpResult87_g21 , ( ( ( lerpResult87_g21 - temp_cast_22 ) * _DissolveScale ) + 0.5 ) , (float)(int)_DissolveScaleMode);
				float cos95_g21 = cos( ( ( _DissolveRotator * PI ) / 180.0 ) );
				float sin95_g21 = sin( ( ( _DissolveRotator * PI ) / 180.0 ) );
				float2 rotator95_g21 = mul( lerpResult93_g21 - float2( 0.5,0.5 ) , float2x2( cos95_g21 , -sin95_g21 , sin95_g21 , cos95_g21 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g21 = lerp( rotator95_g21 , saturate( rotator95_g21 ) , (float)0);
				float2 temp_output_12_0_g15 = (uv_DissolveTex*2.0 + -1.0);
				float2 break2_g15 = temp_output_12_0_g15;
				float2 appendResult9_g15 = (float2(pow( length( temp_output_12_0_g15 ) , 1.0 ) , ( ( atan2( break2_g15.y , break2_g15.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g15 = _DissolveScaleOffset;
				float2 appendResult24_g15 = (float2(break19_g15.x , break19_g15.y));
				float2 appendResult7_g15 = (float2(break19_g15.z , break19_g15.w));
				float2 appendResult6_g15 = (float2(( _DissolvePolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _DissolvePolarVSpeed )));
				float2 lerpResult270 = lerp( lerpResult100_g21 , ( (appendResult9_g15*appendResult24_g15 + appendResult7_g15) + appendResult6_g15 ) , _DissolvePolarMode);
				float4 tex2DNode281 = tex2D( _DissolveTex, ( Noise256 + lerpResult270 ) );
				float lerpResult324 = lerp( tex2DNode281.r , tex2DNode281.a , _DissolveRA);
				float smoothstepResult309 = smoothstep( ( break316.x - break316.y ) , lerpResult296.x , saturate( ( ( lerpResult324 + ( lerpResult324 / _DissolveDivide ) ) / 2.0 ) ));
				#ifdef _DISSOLVESWITCH_ON
				float staticSwitch326 = smoothstepResult309;
				#else
				float staticSwitch326 = 1.0;
				#endif
				float Dissolve283 = staticSwitch326;
				

				surfaceDescription.Alpha = ( IN.ase_color.a * _MainColor.a * lerpResult185 * Mask179 * Dissolve283 );
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					clip(surfaceDescription.Alpha - surfaceDescription.AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				float3 normalWS = IN.normalWS;

				return half4(NormalizeNormalPerPixel(normalWS), 0.0);
			}

			ENDHLSL
		}

		
		Pass
		{
			
            Name "DepthNormalsOnly"
            Tags { "LightMode"="DepthNormalsOnly" }

			ZTest LEqual
			ZWrite On

			HLSLPROGRAM

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 120108


			#pragma exclude_renderers glcore gles gles3 
			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define ATTRIBUTES_NEED_TEXCOORD1
			#define VARYINGS_NEED_NORMAL_WS
			#define VARYINGS_NEED_TANGENT_WS

			#define SHADERPASS SHADERPASS_DEPTHNORMALSONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#pragma shader_feature _DISSOLVESWITCH_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MaskTex_ST;
			float4 _MainScaleOffset;
			float4 _DissolveTex_ST;
			float4 _MaskScaleOffset;
			float4 _NoiseTex_ST;
			float4 _MainTex_ST;
			float4 _NoiseScaleOffset;
			float4 _DissolveScaleOffset;
			float4 _MainColor;
			float _MaskWrapMode;
			float _MaskScaleMode;
			float _MaskScale;
			float _MaskPolarUSpeed;
			float _MaskPolarVSpeed;
			float _MaskPolarMode;
			float _MaskRotator;
			float _MaskRA;
			float _DissolveSmooth;
			float _MaskCustom;
			float _DissolveCustom;
			float _DissolveUSpeed;
			float _DissolveVSpeed;
			float _DissolveSpeedCustom;
			float _DissolveScale;
			float _DissolveScaleMode;
			float _DissolveRotator;
			float _DissolvePolarUSpeed;
			float _DissolvePolarVSpeed;
			float _DissolvePolarMode;
			float _DissolvePower;
			float _Dst;
			float _MainRA;
			float _MaskUSpeed;
			float _ZTestMode;
			float _CullMode;
			float _ZWriteMode;
			float _MainUSpeed;
			float _MainVSpeed;
			float _MainScale;
			float _MainScaleMode;
			float _MainRotator;
			float _MainWrapMode;
			float _MainPolarUSpeed;
			float _MainPolarVSpeed;
			float _MaskVSpeed;
			float _MainPolarMode;
			float _NoiseUSpeed;
			float _NoiseVSpeed;
			float _NoiseCustom;
			float _NoiseScale;
			float _NoiseScaleMode;
			float _NoiseRotator;
			float _NoisePolarUSpeed;
			float _NoisePolarVSpeed;
			float _NoisePolarMode;
			float _NoiseRA;
			float _DissolveRA;
			float _NoisePower;
			float _DissolveDivide;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _MainTex;
			sampler2D _NoiseTex;
			sampler2D _MaskTex;
			sampler2D _DissolveTex;


			
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_color = v.ase_color;
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_texcoord2 = v.ase_texcoord1;
				o.ase_texcoord3 = v.ase_texcoord2;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 normalWS = TransformObjectToWorldNormal(v.ase_normal);

				o.clipPos = TransformWorldToHClip(positionWS);
				o.normalWS.xyz =  normalWS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord2 = v.ase_texcoord2;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float2 appendResult81_g17 = (float2(( _MainUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MainVSpeed )));
				float2 uv_MainTex = IN.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 temp_output_83_0_g17 = uv_MainTex;
				float2 lerpResult87_g17 = lerp( ( appendResult81_g17 + temp_output_83_0_g17 ) , ( temp_output_83_0_g17 + float2( 0,0 ) ) , (float)0);
				float2 temp_cast_1 = (0.5).xx;
				float2 lerpResult93_g17 = lerp( lerpResult87_g17 , ( ( ( lerpResult87_g17 - temp_cast_1 ) * _MainScale ) + 0.5 ) , (float)(int)_MainScaleMode);
				float cos95_g17 = cos( ( ( _MainRotator * PI ) / 180.0 ) );
				float sin95_g17 = sin( ( ( _MainRotator * PI ) / 180.0 ) );
				float2 rotator95_g17 = mul( lerpResult93_g17 - float2( 0.5,0.5 ) , float2x2( cos95_g17 , -sin95_g17 , sin95_g17 , cos95_g17 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g17 = lerp( rotator95_g17 , saturate( rotator95_g17 ) , (float)(int)_MainWrapMode);
				float2 temp_output_12_0_g18 = (uv_MainTex*2.0 + -1.0);
				float2 break2_g18 = temp_output_12_0_g18;
				float2 appendResult9_g18 = (float2(pow( length( temp_output_12_0_g18 ) , 1.0 ) , ( ( atan2( break2_g18.y , break2_g18.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g18 = _MainScaleOffset;
				float2 appendResult24_g18 = (float2(break19_g18.x , break19_g18.y));
				float2 appendResult7_g18 = (float2(break19_g18.z , break19_g18.w));
				float2 appendResult6_g18 = (float2(( _MainPolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MainPolarVSpeed )));
				float2 lerpResult211 = lerp( lerpResult100_g17 , ( (appendResult9_g18*appendResult24_g18 + appendResult7_g18) + appendResult6_g18 ) , _MainPolarMode);
				float2 appendResult81_g9 = (float2(( _NoiseUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _NoiseVSpeed )));
				float2 uv_NoiseTex = IN.ase_texcoord1.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float2 temp_output_83_0_g9 = uv_NoiseTex;
				float4 texCoord259 = IN.ase_texcoord2;
				texCoord259.xy = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult258 = (float2(texCoord259.x , texCoord259.y));
				float2 lerpResult87_g9 = lerp( ( appendResult81_g9 + temp_output_83_0_g9 ) , ( temp_output_83_0_g9 + appendResult258 ) , (float)(int)_NoiseCustom);
				float2 temp_cast_8 = (0.5).xx;
				float2 lerpResult93_g9 = lerp( lerpResult87_g9 , ( ( ( lerpResult87_g9 - temp_cast_8 ) * _NoiseScale ) + 0.5 ) , (float)(int)_NoiseScaleMode);
				float cos95_g9 = cos( ( ( _NoiseRotator * PI ) / 180.0 ) );
				float sin95_g9 = sin( ( ( _NoiseRotator * PI ) / 180.0 ) );
				float2 rotator95_g9 = mul( lerpResult93_g9 - float2( 0.5,0.5 ) , float2x2( cos95_g9 , -sin95_g9 , sin95_g9 , cos95_g9 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g9 = lerp( rotator95_g9 , saturate( rotator95_g9 ) , (float)0);
				float2 temp_output_12_0_g16 = (uv_NoiseTex*2.0 + -1.0);
				float2 break2_g16 = temp_output_12_0_g16;
				float2 appendResult9_g16 = (float2(pow( length( temp_output_12_0_g16 ) , 1.0 ) , ( ( atan2( break2_g16.y , break2_g16.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g16 = _NoiseScaleOffset;
				float2 appendResult24_g16 = (float2(break19_g16.x , break19_g16.y));
				float2 appendResult7_g16 = (float2(break19_g16.z , break19_g16.w));
				float2 appendResult6_g16 = (float2(( _NoisePolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _NoisePolarVSpeed )));
				float2 lerpResult243 = lerp( lerpResult100_g9 , ( (appendResult9_g16*appendResult24_g16 + appendResult7_g16) + appendResult6_g16 ) , _NoisePolarMode);
				float4 tex2DNode254 = tex2D( _NoiseTex, lerpResult243 );
				float lerpResult330 = lerp( tex2DNode254.r , tex2DNode254.a , _NoiseRA);
				float Noise256 = ( _NoisePower * (-0.5 + (lerpResult330 - 0.0) * (0.5 - -0.5) / (1.0 - 0.0)) );
				float4 tex2DNode182 = tex2D( _MainTex, ( lerpResult211 + Noise256 ) );
				float lerpResult185 = lerp( tex2DNode182.r , tex2DNode182.a , _MainRA);
				float2 appendResult81_g22 = (float2(( _MaskUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MaskVSpeed )));
				float2 uv_MaskTex = IN.ase_texcoord1.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
				float2 temp_output_83_0_g22 = uv_MaskTex;
				float4 texCoord237 = IN.ase_texcoord2;
				texCoord237.xy = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult332 = (float2(texCoord237.z , texCoord237.w));
				float2 lerpResult87_g22 = lerp( ( appendResult81_g22 + temp_output_83_0_g22 ) , ( temp_output_83_0_g22 + appendResult332 ) , (float)(int)_MaskCustom);
				float2 temp_cast_14 = (0.5).xx;
				float2 lerpResult93_g22 = lerp( lerpResult87_g22 , ( ( ( lerpResult87_g22 - temp_cast_14 ) * _MaskScale ) + 0.5 ) , (float)(int)_MaskScaleMode);
				float cos95_g22 = cos( ( ( _MaskRotator * PI ) / 180.0 ) );
				float sin95_g22 = sin( ( ( _MaskRotator * PI ) / 180.0 ) );
				float2 rotator95_g22 = mul( lerpResult93_g22 - float2( 0.5,0.5 ) , float2x2( cos95_g22 , -sin95_g22 , sin95_g22 , cos95_g22 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g22 = lerp( rotator95_g22 , saturate( rotator95_g22 ) , (float)(int)_MaskWrapMode);
				float2 temp_output_12_0_g20 = (uv_MaskTex*2.0 + -1.0);
				float2 break2_g20 = temp_output_12_0_g20;
				float2 appendResult9_g20 = (float2(pow( length( temp_output_12_0_g20 ) , 1.0 ) , ( ( atan2( break2_g20.y , break2_g20.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g20 = _MaskScaleOffset;
				float2 appendResult24_g20 = (float2(break19_g20.x , break19_g20.y));
				float2 appendResult7_g20 = (float2(break19_g20.z , break19_g20.w));
				float2 appendResult6_g20 = (float2(( _MaskPolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _MaskPolarVSpeed )));
				float2 lerpResult233 = lerp( lerpResult100_g22 , ( (appendResult9_g20*appendResult24_g20 + appendResult7_g20) + appendResult6_g20 ) , _MaskPolarMode);
				float4 tex2DNode234 = tex2D( _MaskTex, lerpResult233 );
				float lerpResult322 = lerp( tex2DNode234.r , tex2DNode234.a , _MaskRA);
				float Mask179 = lerpResult322;
				float2 appendResult314 = (float2(_DissolvePower , _DissolveSmooth));
				float4 texCoord285 = IN.ase_texcoord3;
				texCoord285.xy = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult315 = (float2(texCoord285.x , texCoord285.y));
				float2 lerpResult296 = lerp( appendResult314 , appendResult315 , _DissolveCustom);
				float2 break316 = lerpResult296;
				float2 appendResult81_g21 = (float2(( _DissolveUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _DissolveVSpeed )));
				float2 uv_DissolveTex = IN.ase_texcoord1.xy * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
				float2 temp_output_83_0_g21 = uv_DissolveTex;
				float2 appendResult284 = (float2(texCoord285.z , texCoord285.w));
				float2 lerpResult87_g21 = lerp( ( appendResult81_g21 + temp_output_83_0_g21 ) , ( temp_output_83_0_g21 + appendResult284 ) , (float)(int)_DissolveSpeedCustom);
				float2 temp_cast_22 = (0.5).xx;
				float2 lerpResult93_g21 = lerp( lerpResult87_g21 , ( ( ( lerpResult87_g21 - temp_cast_22 ) * _DissolveScale ) + 0.5 ) , (float)(int)_DissolveScaleMode);
				float cos95_g21 = cos( ( ( _DissolveRotator * PI ) / 180.0 ) );
				float sin95_g21 = sin( ( ( _DissolveRotator * PI ) / 180.0 ) );
				float2 rotator95_g21 = mul( lerpResult93_g21 - float2( 0.5,0.5 ) , float2x2( cos95_g21 , -sin95_g21 , sin95_g21 , cos95_g21 )) + float2( 0.5,0.5 );
				float2 lerpResult100_g21 = lerp( rotator95_g21 , saturate( rotator95_g21 ) , (float)0);
				float2 temp_output_12_0_g15 = (uv_DissolveTex*2.0 + -1.0);
				float2 break2_g15 = temp_output_12_0_g15;
				float2 appendResult9_g15 = (float2(pow( length( temp_output_12_0_g15 ) , 1.0 ) , ( ( atan2( break2_g15.y , break2_g15.x ) / ( 2.0 * PI ) ) + 0.5 )));
				float4 break19_g15 = _DissolveScaleOffset;
				float2 appendResult24_g15 = (float2(break19_g15.x , break19_g15.y));
				float2 appendResult7_g15 = (float2(break19_g15.z , break19_g15.w));
				float2 appendResult6_g15 = (float2(( _DissolvePolarUSpeed * _TimeParameters.x ) , ( _TimeParameters.x * _DissolvePolarVSpeed )));
				float2 lerpResult270 = lerp( lerpResult100_g21 , ( (appendResult9_g15*appendResult24_g15 + appendResult7_g15) + appendResult6_g15 ) , _DissolvePolarMode);
				float4 tex2DNode281 = tex2D( _DissolveTex, ( Noise256 + lerpResult270 ) );
				float lerpResult324 = lerp( tex2DNode281.r , tex2DNode281.a , _DissolveRA);
				float smoothstepResult309 = smoothstep( ( break316.x - break316.y ) , lerpResult296.x , saturate( ( ( lerpResult324 + ( lerpResult324 / _DissolveDivide ) ) / 2.0 ) ));
				#ifdef _DISSOLVESWITCH_ON
				float staticSwitch326 = smoothstepResult309;
				#else
				float staticSwitch326 = 1.0;
				#endif
				float Dissolve283 = staticSwitch326;
				

				surfaceDescription.Alpha = ( IN.ase_color.a * _MainColor.a * lerpResult185 * Mask179 * Dissolve283 );
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					clip(surfaceDescription.Alpha - surfaceDescription.AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				float3 normalWS = IN.normalWS;

				return half4(NormalizeNormalPerPixel(normalWS), 0.0);
			}

			ENDHLSL
		}
		
	}
	
	CustomEditor "CustomShaderGUI"
	FallBack "Hidden/Shader Graph/FallbackError"
	
	Fallback "Hidden/InternalErrorShader"
}
/*ASEBEGIN
Version=19200
Node;AmplifyShaderEditor.CommentaryNode;218;-2587.732,-721.2526;Inherit;False;1499.488;997.0276;MAIN;19;182;211;217;215;213;212;216;214;83;86;209;206;205;123;99;261;321;185;184;;1,0.02821757,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;175;-1002.334,-190.7559;Inherit;False;482.0878;389.3668;ALPHA;6;191;303;196;5;4;3;;0.3113208,0.3113208,0.3113208,1;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;4;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;5;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;197;321,-412;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;198;321,-412;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;SceneSelectionPass;0;6;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;199;321,-412;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ScenePickingPass;0;7;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;200;321,-412;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthNormals;0;8;DepthNormals;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=DepthNormalsOnly;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;201;321,-412;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthNormalsOnly;0;9;DepthNormalsOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=DepthNormalsOnly;False;True;9;d3d11;metal;vulkan;xboxone;xboxseries;playstation;ps4;ps5;switch;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.CommentaryNode;219;-2593.773,291.4716;Inherit;False;1504.836;965.9515;MASK;21;237;236;179;220;234;232;228;230;229;223;222;221;224;226;225;233;231;227;322;323;332;;0.9923129,1,0,1;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;-1145.227,3820.081;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.CommentaryNode;240;-1058.542,290.8154;Inherit;False;1986.175;964.1294;Noise;23;256;320;318;319;254;260;242;258;255;259;253;252;251;250;249;248;247;245;244;243;241;330;331;;0,1,0.07963061,1;0;0
Node;AmplifyShaderEditor.FunctionNode;241;-433.8744,557.5827;Inherit;False;UV_Util;-1;;9;3aa0bb821dd43dc4890c52b2de0d1c4b;0;9;83;FLOAT2;0,0;False;74;FLOAT;0;False;75;FLOAT;0;False;85;FLOAT2;0,0;False;86;INT;0;False;103;INT;0;False;99;FLOAT;0;False;91;FLOAT;0;False;94;INT;0;False;1;FLOAT2;47
Node;AmplifyShaderEditor.LerpOp;243;-67.87615,571.132;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;244;-719.4614,471.7268;Inherit;False;Property;_NoiseUSpeed;NoiseUSpeed;31;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;245;-714.3184,555.5959;Inherit;False;Property;_NoiseVSpeed;NoiseVSpeed;32;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;247;-715.6265,710.6815;Inherit;False;Property;_NoiseRotator;NoiseRotator;37;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;-710.3506,779.7397;Inherit;False;Property;_NoiseScale;NoiseScale;39;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;249;-724.3516,854.7397;Inherit;False;Property;_NoiseScaleMode;NoiseScaleMode;38;1;[Enum];Create;True;0;2;OFF;0;ON;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;250;-723.4524,935.8388;Inherit;False;Property;_NoisePolarUSpeed;NoisePolarUSpeed;34;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;251;-747.4524,1005.84;Inherit;False;Property;_NoisePolarVSpeed;NoisePolarVSpeed;35;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;252;-748.1255,1078.844;Inherit;False;Property;_NoiseScaleOffset;NoiseScaleOffset;36;0;Create;True;0;0;0;False;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;253;-381.3653,1015.267;Inherit;False;Property;_NoisePolarMode;NoisePolarMode;33;1;[Enum];Create;True;0;2;UV;0;Polor;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;255;-753.4194,340.8155;Inherit;False;0;254;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;258;-887.9425,564.9672;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;123;-2488.067,-293.4721;Inherit;False;Property;_MainRotator;MainRotator;14;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;205;-2464.792,-222.4138;Inherit;False;Property;_MainScale;MainScale;16;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;206;-2495.792,-149.4138;Inherit;False;Property;_MainScaleMode;MainScaleMode;15;1;[Enum];Create;True;0;2;OFF;0;ON;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-2486.759,-448.5576;Inherit;False;Property;_MainVSpeed;MainVSpeed;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;216;-2520.566,74.68956;Inherit;False;Property;_MainScaleOffset;MainScaleOffset;13;0;Create;True;0;0;0;False;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;212;-2491.894,-68.31474;Inherit;False;Property;_MainPolarUSpeed;MainPolarUSpeed;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;213;-2519.894,1.685262;Inherit;False;Property;_MainPolarVSpeed;MainPolarVSpeed;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;225;-2267.031,472.3828;Inherit;False;Property;_MaskUSpeed;MaskUSpeed;20;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;226;-2261.887,556.2519;Inherit;False;Property;_MaskVSpeed;MaskVSpeed;21;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;224;-2264.633,632.2425;Inherit;False;Property;_MaskWrapMode;MaskWrapMode;19;1;[Enum];Create;True;0;2;Repeat;0;Clamp;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;221;-2263.196,711.3375;Inherit;False;Property;_MaskRotator;MaskRotator;26;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-2239.92,782.3958;Inherit;False;Property;_MaskScale;MaskScale;28;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;223;-2270.92,855.3958;Inherit;False;Property;_MaskScaleMode;MaskScaleMode;27;1;[Enum];Create;True;0;2;OFF;0;ON;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;229;-2271.021,936.4949;Inherit;False;Property;_MaskPolarUSpeed;MaskPolarUSpeed;23;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;230;-2295.021,1006.496;Inherit;False;Property;_MaskPolarVSpeed;MaskPolarVSpeed;24;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;228;-2295.695,1079.5;Inherit;False;Property;_MaskScaleOffset;MaskScaleOffset;25;0;Create;True;0;0;0;False;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;236;-2494.56,657.325;Inherit;False;Property;_MaskCustom;MaskCustom;18;1;[Toggle];Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;267;-2590.376,1274.877;Inherit;False;2746.299;985.491;Dissolve;39;324;325;296;311;316;317;310;308;306;305;309;312;297;280;279;278;277;276;299;286;281;294;315;314;283;285;282;284;275;274;272;271;270;269;268;326;327;328;329;;0,0.5397825,1,1;0;0
Node;AmplifyShaderEditor.FunctionNode;269;-1972.584,1851.739;Inherit;False;UV_Polar;-1;;15;cb6b60ee34962504bb479ce245fbcabd;0;4;17;FLOAT2;0,0;False;18;FLOAT;0;False;4;FLOAT;0;False;22;FLOAT4;1,1,0,0;False;1;FLOAT2;23
Node;AmplifyShaderEditor.RangedFloatNode;271;-2263.635,1455.788;Inherit;False;Property;_DissolveUSpeed;DissolveUSpeed;42;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;272;-2258.49,1539.657;Inherit;False;Property;_DissolveVSpeed;DissolveVSpeed;43;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;274;-2259.8,1694.743;Inherit;False;Property;_DissolveRotator;DissolveRotator;41;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;275;-2236.523,1765.801;Inherit;False;Property;_DissolveScale;DissolveScale;44;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;284;-2486.115,1502.029;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;282;-2297.593,1324.877;Inherit;False;0;281;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;-685.9902,-130.055;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;303;-979.4674,15.38322;Inherit;False;283;Dissolve;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;191;-977.1169,-54.35198;Inherit;False;179;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;242;-428.4112,867.6778;Inherit;False;UV_Polar;-1;;16;cb6b60ee34962504bb479ce245fbcabd;0;4;17;FLOAT2;0,0;False;18;FLOAT;0;False;4;FLOAT;0;False;22;FLOAT4;1,1,0,0;False;1;FLOAT2;23
Node;AmplifyShaderEditor.DynamicAppendNode;314;-1476.19,1800.149;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;315;-1523.925,1986.523;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;294;-1590.059,1906.616;Inherit;False;Property;_DissolveSmooth;DissolveSmooth;49;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;86;-2491.902,-532.4268;Inherit;False;Property;_MainUSpeed;MainUSpeed;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;281;-1397.537,1529.064;Inherit;True;Property;_DissolveTex;DissolveTex;40;1;[Header];Create;True;1;Dissolve;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;299;-1510.954,2105.396;Inherit;False;Property;_DissolveCustom;DissolveCustom;53;1;[Enum];Create;True;0;2;OFF;0;ON;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;260;-922.9695,659.7548;Inherit;False;Property;_NoiseCustom;NoiseCustom;30;1;[Toggle];Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;276;-2267.523,1838.801;Inherit;False;Property;_DissolveScaleMode;DissolveScaleMode;45;1;[Enum];Create;True;0;2;OFF;0;ON;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;277;-2267.625,1919.9;Inherit;False;Property;_DissolvePolarUSpeed;DissolvePolarUSpeed;46;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;278;-2291.625,1989.902;Inherit;False;Property;_DissolvePolarVSpeed;DissolvePolarVSpeed;47;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;279;-2292.299,2062.906;Inherit;False;Property;_DissolveScaleOffset;DissolveScaleOffset;48;0;Create;True;0;0;0;False;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;280;-1925.539,1999.328;Inherit;False;Property;_DissolvePolarMode;DissolvePolarMode;51;1;[Enum];Create;True;0;2;UV;0;Polor;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;297;-1679.058,1707.842;Inherit;False;Property;_DissolvePower;DissolvePower;50;0;Create;True;0;0;0;False;0;False;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;254;123.1718,554.6638;Inherit;True;Property;_NoiseTex;NoiseTex;29;1;[Header];Create;True;1;Noise;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;214;-2254.317,-470.5708;Inherit;False;UV_Util;-1;;17;3aa0bb821dd43dc4890c52b2de0d1c4b;0;9;83;FLOAT2;0,0;False;74;FLOAT;0;False;75;FLOAT;0;False;85;FLOAT2;0,0;False;86;INT;0;False;103;INT;0;False;99;FLOAT;0;False;91;FLOAT;0;False;94;INT;0;False;1;FLOAT2;47
Node;AmplifyShaderEditor.FunctionNode;215;-2266.854,-161.4758;Inherit;False;UV_Polar;-1;;18;cb6b60ee34962504bb479ce245fbcabd;0;4;17;FLOAT2;0,0;False;18;FLOAT;0;False;4;FLOAT;0;False;22;FLOAT4;1,1,0,0;False;1;FLOAT2;23
Node;AmplifyShaderEditor.RangedFloatNode;217;-2204.092,2.598339;Inherit;False;Property;_MainPolarMode;MainPolarMode;10;1;[Enum];Create;True;0;2;UV;0;Polor;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;321;-1762.403,-440.9243;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;211;-1948.854,-371.5305;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;261;-1961.703,-202.9694;Inherit;False;256;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;182;-1612.254,-467.8347;Inherit;True;Property;_MainTex;MainTex;5;1;[Header];Create;True;1;Main;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;188;-596.32,-519.7495;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;180;-839.5998,-438.7585;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;181;-1038.172,-440.42;Inherit;False;Property;_MainColor;MainColor;4;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1.843137,1.843137,1.843137,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;185;-1252.672,-365.0738;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;231;-2011.42,861.771;Inherit;False;UV_Polar;-1;;20;cb6b60ee34962504bb479ce245fbcabd;0;4;17;FLOAT2;0,0;False;18;FLOAT;0;False;4;FLOAT;0;False;22;FLOAT4;1,1,0,0;False;1;FLOAT2;23
Node;AmplifyShaderEditor.SamplerNode;234;-1558.442,721.5413;Inherit;True;Property;_MaskTex;MaskTex;17;1;[Header];Create;True;1;Mask;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;179;-1434.753,434.7867;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;309;-432.0062,1660.641;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;305;-877.0975,1573.383;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;306;-724.997,1570.783;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;308;-581.9972,1573.382;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;310;-977.1979,1692.982;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;316;-768.5244,1701.082;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleSubtractOpNode;311;-603.4901,1687.807;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;296;-1242.402,1887.682;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;325;-1268.682,1716.063;Inherit;False;Property;_DissolveRA;DissolveRA;58;1;[Enum];Create;True;0;2;R;0;A;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;324;-1112.254,1577.305;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;327;-384.0166,1565.595;Inherit;False;Constant;_Float0;Float 0;58;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;283;-213.6151,1502.749;Inherit;False;Dissolve;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;317;-614.1156,1935.636;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;312;-1291.591,1789.748;Inherit;False;Property;_DissolveDivide;DissolveDivide;54;0;Create;True;0;0;0;False;0;False;1;0;0;7;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;270;-1681.707,1555.194;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;328;-1527.194,1480.471;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;329;-1762.367,1422.349;Inherit;False;256;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;184;-1485.256,-275.3445;Inherit;False;Property;_MainRA;MainRA;7;1;[Enum];Create;True;0;2;R;0;A;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;232;-1971.528,1036.924;Inherit;False;Property;_MaskPolarMode;MaskPolarMode;22;1;[Enum];Create;True;0;2;UV;0;Polor;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;233;-1723.077,743.7336;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;322;-1419.342,547.4844;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;323;-1603.663,600.0886;Inherit;False;Property;_MaskRA;MaskRA;56;1;[Enum];Create;True;0;2;R;0;A;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;319;678.8247,622.4698;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;318;482.0079,711.9449;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.5;False;4;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;256;737.6685,513.1075;Inherit;False;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;330;274.1343,782.9626;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;331;108.8135,818.5669;Inherit;False;Property;_NoiseRA;NoiseRA;57;1;[Enum];Create;True;0;2;R;0;A;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;209;-2489.504,-372.5671;Inherit;False;Property;_MainWrapMode;MainWrapMode;6;1;[Enum];Create;True;0;2;Repeat;0;Clamp;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;268;-1978.047,1541.644;Inherit;False;UV_Util;-1;;21;3aa0bb821dd43dc4890c52b2de0d1c4b;0;9;83;FLOAT2;0,0;False;74;FLOAT;0;False;75;FLOAT;0;False;85;FLOAT2;0,0;False;86;INT;0;False;103;INT;0;False;99;FLOAT;0;False;91;FLOAT;0;False;94;INT;0;False;1;FLOAT2;47
Node;AmplifyShaderEditor.RangedFloatNode;286;-2492.164,1640.73;Inherit;False;Property;_DissolveSpeedCustom;DissolveSpeedCustom;52;1;[Enum];Create;True;0;2;OFF;0;ON;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;326;-217.3258,1597.872;Inherit;False;Property;_DissolveSwitch;DissolveSwitch;59;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;320;513.5418,531.6664;Inherit;False;Property;_NoisePower;NoisePower;55;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;220;-2300.989,341.4716;Inherit;False;0;234;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;227;-2016.883,551.6759;Inherit;False;UV_Util;-1;;22;3aa0bb821dd43dc4890c52b2de0d1c4b;0;9;83;FLOAT2;0,0;False;74;FLOAT;0;False;75;FLOAT;0;False;85;FLOAT2;0,0;False;86;INT;0;False;103;INT;0;False;99;FLOAT;0;False;91;FLOAT;0;False;94;INT;0;False;1;FLOAT2;47
Node;AmplifyShaderEditor.DynamicAppendNode;332;-2423.905,538.5273;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;99;-2525.86,-663.3381;Inherit;False;0;182;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;237;-2554.299,350.5767;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;259;-1021.73,337.5608;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;285;-2584.32,1320.455;Inherit;False;2;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;194;-122.5327,-304.2461;Inherit;False;Property;_CullMode;CullMode;0;1;[Enum];Create;False;0;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;195;-123.0474,-382.3976;Inherit;False;Property;_Dst;Dst;1;1;[Enum];Create;False;0;2;AlphaBlend;10;Additive;1;0;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;192;-126.6323,-226.297;Inherit;False;Property;_ZTestMode;ZTestMode;3;1;[Enum];Create;False;0;2;Less or Equal;4;Always;8;0;True;0;False;4;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;-386.2654,-369.9803;Float;False;True;-1;2;CustomShaderGUI;0;13;VFX/VFXShaderURP;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;True;True;2;True;_CullMode;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;UniversalMaterialType=Unlit;True;3;True;6;d3d11;glcore;gles;gles3;metal;vulkan;0;True;True;2;5;False;;10;True;_Dst;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;2;True;_ZWriteMode;True;3;True;_ZTestMode;True;False;0;False;;0;False;;True;2;LightMode=UniversalForward;Queue=Transparent=Queue=3000;False;False;0;Hidden/InternalErrorShader;0;0;Standard;23;Surface;1;638294159058852208;  Blend;0;0;Two Sided;0;638301679033111844;Forward Only;0;0;Cast Shadows;0;638301656613121842;  Use Shadow Threshold;0;0;Receive Shadows;0;638301656627020394;GPU Instancing;0;638301674581675192;LOD CrossFade;0;0;Built-in Fog;0;0;DOTS Instancing;0;0;Meta Pass;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,;0;  Type;0;0;  Tess;16,False,;0;  Min;10,False,;0;  Max;25,False,;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Vertex Position,InvertActionOnDeselection;1;0;0;10;False;True;False;True;False;False;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.RangedFloatNode;333;-126.0838,-147.0999;Inherit;False;Property;_ZWriteMode;ZWriteMode;2;1;[Enum];Create;False;0;2;OFF;0;ON;1;0;True;0;False;0;2;0;0;0;1;FLOAT;0
WireConnection;241;83;255;0
WireConnection;241;74;244;0
WireConnection;241;75;245;0
WireConnection;241;85;258;0
WireConnection;241;86;260;0
WireConnection;241;99;247;0
WireConnection;241;91;248;0
WireConnection;241;94;249;0
WireConnection;243;0;241;47
WireConnection;243;1;242;23
WireConnection;243;2;253;0
WireConnection;258;0;259;1
WireConnection;258;1;259;2
WireConnection;269;17;282;0
WireConnection;269;18;277;0
WireConnection;269;4;278;0
WireConnection;269;22;279;0
WireConnection;284;0;285;3
WireConnection;284;1;285;4
WireConnection;196;0;180;4
WireConnection;196;1;181;4
WireConnection;196;2;185;0
WireConnection;196;3;191;0
WireConnection;196;4;303;0
WireConnection;242;17;255;0
WireConnection;242;18;250;0
WireConnection;242;4;251;0
WireConnection;242;22;252;0
WireConnection;314;0;297;0
WireConnection;314;1;294;0
WireConnection;315;0;285;1
WireConnection;315;1;285;2
WireConnection;281;1;328;0
WireConnection;254;1;243;0
WireConnection;214;83;99;0
WireConnection;214;74;86;0
WireConnection;214;75;83;0
WireConnection;214;103;209;0
WireConnection;214;99;123;0
WireConnection;214;91;205;0
WireConnection;214;94;206;0
WireConnection;215;17;99;0
WireConnection;215;18;212;0
WireConnection;215;4;213;0
WireConnection;215;22;216;0
WireConnection;321;0;211;0
WireConnection;321;1;261;0
WireConnection;211;0;214;47
WireConnection;211;1;215;23
WireConnection;211;2;217;0
WireConnection;182;1;321;0
WireConnection;188;0;182;0
WireConnection;188;1;181;0
WireConnection;188;2;180;0
WireConnection;185;0;182;1
WireConnection;185;1;182;4
WireConnection;185;2;184;0
WireConnection;231;17;220;0
WireConnection;231;18;229;0
WireConnection;231;4;230;0
WireConnection;231;22;228;0
WireConnection;234;1;233;0
WireConnection;179;0;322;0
WireConnection;309;0;308;0
WireConnection;309;1;311;0
WireConnection;309;2;317;0
WireConnection;305;0;324;0
WireConnection;305;1;310;0
WireConnection;306;0;305;0
WireConnection;308;0;306;0
WireConnection;310;0;324;0
WireConnection;310;1;312;0
WireConnection;316;0;296;0
WireConnection;311;0;316;0
WireConnection;311;1;316;1
WireConnection;296;0;314;0
WireConnection;296;1;315;0
WireConnection;296;2;299;0
WireConnection;324;0;281;1
WireConnection;324;1;281;4
WireConnection;324;2;325;0
WireConnection;283;0;326;0
WireConnection;317;0;296;0
WireConnection;270;0;268;47
WireConnection;270;1;269;23
WireConnection;270;2;280;0
WireConnection;328;0;329;0
WireConnection;328;1;270;0
WireConnection;233;0;227;47
WireConnection;233;1;231;23
WireConnection;233;2;232;0
WireConnection;322;0;234;1
WireConnection;322;1;234;4
WireConnection;322;2;323;0
WireConnection;319;0;320;0
WireConnection;319;1;318;0
WireConnection;318;0;330;0
WireConnection;256;0;319;0
WireConnection;330;0;254;1
WireConnection;330;1;254;4
WireConnection;330;2;331;0
WireConnection;268;83;282;0
WireConnection;268;74;271;0
WireConnection;268;75;272;0
WireConnection;268;85;284;0
WireConnection;268;86;286;0
WireConnection;268;99;274;0
WireConnection;268;91;275;0
WireConnection;268;94;276;0
WireConnection;326;1;327;0
WireConnection;326;0;309;0
WireConnection;227;83;220;0
WireConnection;227;74;225;0
WireConnection;227;75;226;0
WireConnection;227;85;332;0
WireConnection;227;86;236;0
WireConnection;227;103;224;0
WireConnection;227;99;221;0
WireConnection;227;91;222;0
WireConnection;227;94;223;0
WireConnection;332;0;237;3
WireConnection;332;1;237;4
WireConnection;2;2;188;0
WireConnection;2;3;196;0
ASEEND*/
//CHKSM=4289921C56790769C5B08DFE20FCE77469D4851A