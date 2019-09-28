// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CustomWaterFoam"
{
	Properties
	{
		_NormalMapWaves("Normal Map Waves", 2D) = "bump" {}
		_WaveScale("WaveScale", Float) = 0
		_Distortion("Distortion", Float) = 0.5
		_depthcolor("depth color", Color) = (0.1839857,0.3014706,0.2625791,1)
		_DepthDist("DepthDist", Range( -10 , 10)) = 1
		_DepthFoam("Depth Foam", Range( -10 , 10)) = 1
		_FoamColor("Foam Color", Color) = (1,1,1,0)
		_FoamPower("Foam Power", Float) = 1
		_FoamMultiply("Foam Multiply", Float) = 13
		_FoamContrast("Foam Contrast", Float) = 8
		_Foamopacity("Foam opacity", Range( 0 , 1)) = 0.8
		_SkyTexture("SkyTexture", 2D) = "white" {}
		_SkyColor("SkyColor", Color) = (1,1,1,1)
		_SkySpeed("SkySpeed", Vector) = (0.2,0,0,0)
		_Color0("Color 0", Color) = (0,0.1927017,0.7075472,0)
		_Float1("Float 1", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
		};

		uniform sampler2D _GrabTexture;
		uniform float _WaveScale;
		uniform sampler2D _NormalMapWaves;
		uniform float4 _NormalMapWaves_ST;
		uniform float _Distortion;
		uniform float4 _depthcolor;
		uniform float4 _Color0;
		uniform sampler2D _CameraDepthTexture;
		uniform float _DepthDist;
		uniform float _Float1;
		uniform float4 _SkyColor;
		uniform sampler2D _SkyTexture;
		uniform float2 _SkySpeed;
		uniform float4 _SkyTexture_ST;
		uniform float4 _FoamColor;
		uniform float _FoamMultiply;
		uniform float _FoamContrast;
		uniform float _DepthFoam;
		uniform float _FoamPower;
		uniform float _Foamopacity;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float2 uv_NormalMapWaves = i.uv_texcoord * _NormalMapWaves_ST.xy + _NormalMapWaves_ST.zw;
			float2 panner260 = ( uv_NormalMapWaves + 1 * _Time.y * float2( -0.03,0 ));
			float2 panner259 = ( uv_NormalMapWaves + 1 * _Time.y * float2( 0.04,0.04 ));
			float3 temp_output_264_0 = BlendNormals( UnpackScaleNormal( tex2D( _NormalMapWaves, panner260 ) ,_WaveScale ) , UnpackScaleNormal( tex2D( _NormalMapWaves, panner259 ) ,_WaveScale ) );
			float4 screenColor254 = tex2D( _GrabTexture, ( (ase_grabScreenPosNorm).xyw + ( temp_output_264_0 * _Distortion ) ).xy );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth164 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth164 = abs( ( screenDepth164 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDist ) );
			float clampResult285 = clamp( distanceDepth164 , 0 , 1 );
			float4 lerpResult347 = lerp( _depthcolor , _Color0 , clampResult285);
			float4 screenColor370 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( ase_grabScreenPos ) );
			float4 lerpResult371 = lerp( lerpResult347 , screenColor370 , _Float1);
			float2 uv_SkyTexture = i.uv_texcoord * _SkyTexture_ST.xy + _SkyTexture_ST.zw;
			float2 panner343 = ( uv_SkyTexture + 1 * _Time.y * _SkySpeed);
			float4 lerpResult297 = lerp( lerpResult371 , _SkyColor , tex2D( _SkyTexture, ( float3( panner343 ,  0.0 ) + temp_output_264_0 ).xy ));
			float4 lerpResult217 = lerp( screenColor254 , lerpResult297 , clampResult285);
			float4 _Color2 = float4(1,1,1,0);
			float grayscale300 = Luminance(temp_output_264_0);
			float clampResult307 = clamp( pow( ( grayscale300 * _FoamMultiply ) , _FoamContrast ) , 0 , 1 );
			float4 temp_cast_3 = (clampResult307).xxxx;
			float screenDepth224 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth224 = abs( ( screenDepth224 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFoam ) );
			float clampResult319 = clamp( distanceDepth224 , 0 , 1 );
			float4 lerpResult314 = lerp( _Color2 , temp_cast_3 , clampResult319);
			float clampResult238 = clamp( pow( distanceDepth224 , _FoamPower ) , 0 , 1 );
			float4 temp_cast_4 = (clampResult238).xxxx;
			float4 lerpResult322 = lerp( _Color2 , float4(0,0,0,0) , ( lerpResult314 - temp_cast_4 ).r);
			float4 clampResult324 = clamp( lerpResult322 , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 lerpResult225 = lerp( _FoamColor , lerpResult217 , clampResult324.r);
			float4 lerpResult248 = lerp( lerpResult217 , lerpResult225 , _Foamopacity);
			o.Albedo = lerpResult248.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15001
2001;280;1143;663;2763.356;1392.39;1.880846;True;False
Node;AmplifyShaderEditor.CommentaryNode;249;-4078.947,-726.301;Float;False;1281.603;457.1994;Blend panning normals to fake noving ripples;7;264;263;262;261;260;259;258;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;258;-4030.947,-662.301;Float;False;0;263;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;259;-3742.947,-566.301;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.04,0.04;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;261;-3694.947,-438.301;Float;False;Property;_WaveScale;WaveScale;1;0;Create;True;0;0;False;0;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;260;-3742.947,-678.301;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.03,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;263;-3406.947,-678.301;Float;True;Property;_NormalMapWaves;Normal Map Waves;0;0;Create;True;0;0;False;0;None;2881d71fa93e7104698854c6da477827;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;262;-3390.947,-470.301;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Instance;263;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;264;-2958.947,-534.301;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;348;-1650.569,-305.9502;Float;False;770.229;289.134;Comment;3;167;164;285;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;303;-2777.796,337.5435;Float;False;Property;_FoamMultiply;Foam Multiply;8;0;Create;True;0;0;False;0;13;12.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;300;-2770.364,213.3818;Float;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;223;-2658.413,464.3535;Float;False;Property;_DepthFoam;Depth Foam;5;0;Create;True;0;0;False;0;1;0.4;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;167;-1593.113,-136.2894;Float;False;Property;_DepthDist;DepthDist;4;0;Create;True;0;0;False;0;1;0.8;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;310;-2563.292,341.586;Float;False;Property;_FoamContrast;Foam Contrast;9;0;Create;True;0;0;False;0;8;22.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;302;-2548.242,228.8345;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;232;-2170.585,543.4949;Float;False;Property;_FoamPower;Foam Power;7;0;Create;True;0;0;False;0;1;1.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;164;-1270.28,-185.5454;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;224;-2378.601,464.7851;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;344;-2311.437,-140.7699;Float;False;Property;_SkySpeed;SkySpeed;13;0;Create;True;0;0;False;0;0.2,0;0.2,-0.01;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;250;-2712.109,-716.9537;Float;False;985.6011;418.6005;Get screen color for refraction and disturbe it with normals;8;257;256;255;254;253;252;251;325;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PowerNode;309;-2345.282,269.6226;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;342;-2420.258,-276.1172;Float;False;0;294;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;251;-2603.695,-491.6255;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;237;-1845.958,479.5902;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;216;-1642.949,-1044.418;Float;False;Property;_depthcolor;depth color;3;0;Create;True;0;0;False;0;0.1839857,0.3014706,0.2625791,1;0.04705883,0.6509804,0.574955,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;345;-1577.822,-1256.656;Float;False;Property;_Color0;Color 0;14;0;Create;True;0;0;False;0;0,0.1927017,0.7075472,0;0,0.1078617,0.3962264,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;285;-1055.34,-255.9502;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;319;-1984.031,384.7348;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;252;-2506.265,-418.3532;Float;False;Property;_Distortion;Distortion;2;0;Create;True;0;0;False;0;0.5;0.48;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;318;-2012.218,62.90353;Float;False;Constant;_Color2;Color 2;12;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;343;-2128.809,-256.8357;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.2,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;307;-2143.897,266.608;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;256;-2668.003,-657.5959;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;325;-1904.014,-374.3689;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;347;-1271.675,-973.0568;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;370;-1048.025,-1140.357;Float;False;Global;_GrabScreen0;Grab Screen 0;16;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;372;-1243.633,-828.1362;Float;False;Property;_Float1;Float 1;16;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;257;-2370.655,-657.4619;Float;False;True;True;False;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;253;-2305.107,-523.9534;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;238;-1668.616,454.9448;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;314;-1759.17,248.8374;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;371;-972.7914,-899.6083;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;323;-1504.898,22.75995;Float;False;Constant;_Color3;Color 3;12;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;298;-1704.871,-703.5693;Float;False;Property;_SkyColor;SkyColor;12;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;255;-2123.01,-564.8537;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;294;-1736.685,-391.6284;Float;True;Property;_SkyTexture;SkyTexture;11;0;Create;True;0;0;False;0;None;898ad92158b86c74bb26a630b2d4fbfa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;313;-1494.026,280.5769;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;254;-1968.83,-569.7656;Float;False;Global;_GrabScreen1;Grab Screen 1;-1;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;297;-1404.855,-467.2224;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;322;-1273.441,243.1596;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;226;-1202.323,23.05176;Float;False;Property;_FoamColor;Foam Color;6;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;217;-908.4481,-516.4762;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;324;-1088.556,247.0396;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;243;-870.9919,355.3689;Float;False;Property;_Foamopacity;Foam opacity;10;0;Create;True;0;0;False;0;0.8;0.879;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;225;-850.2144,198.7798;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;367;-1747.159,-913.8766;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;364;-1957.715,-862.6996;Float;False;Property;_Float0;Float 0;15;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;248;-497.8096,241.7325;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;366;-2054.22,-1177.072;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;369;-1594.729,-859.7575;Float;False;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;290;-257.04,247.0367;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;CustomWaterFoam;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;259;0;258;0
WireConnection;260;0;258;0
WireConnection;263;1;260;0
WireConnection;263;5;261;0
WireConnection;262;1;259;0
WireConnection;262;5;261;0
WireConnection;264;0;263;0
WireConnection;264;1;262;0
WireConnection;300;0;264;0
WireConnection;302;0;300;0
WireConnection;302;1;303;0
WireConnection;164;0;167;0
WireConnection;224;0;223;0
WireConnection;309;0;302;0
WireConnection;309;1;310;0
WireConnection;251;0;264;0
WireConnection;237;0;224;0
WireConnection;237;1;232;0
WireConnection;285;0;164;0
WireConnection;319;0;224;0
WireConnection;343;0;342;0
WireConnection;343;2;344;0
WireConnection;307;0;309;0
WireConnection;325;0;343;0
WireConnection;325;1;264;0
WireConnection;347;0;216;0
WireConnection;347;1;345;0
WireConnection;347;2;285;0
WireConnection;257;0;256;0
WireConnection;253;0;251;0
WireConnection;253;1;252;0
WireConnection;238;0;237;0
WireConnection;314;0;318;0
WireConnection;314;1;307;0
WireConnection;314;2;319;0
WireConnection;371;0;347;0
WireConnection;371;1;370;0
WireConnection;371;2;372;0
WireConnection;255;0;257;0
WireConnection;255;1;253;0
WireConnection;294;1;325;0
WireConnection;313;0;314;0
WireConnection;313;1;238;0
WireConnection;254;0;255;0
WireConnection;297;0;371;0
WireConnection;297;1;298;0
WireConnection;297;2;294;0
WireConnection;322;0;318;0
WireConnection;322;1;323;0
WireConnection;322;2;313;0
WireConnection;217;0;254;0
WireConnection;217;1;297;0
WireConnection;217;2;285;0
WireConnection;324;0;322;0
WireConnection;225;0;226;0
WireConnection;225;1;217;0
WireConnection;225;2;324;0
WireConnection;367;0;366;2
WireConnection;367;1;364;0
WireConnection;248;0;217;0
WireConnection;248;1;225;0
WireConnection;248;2;243;0
WireConnection;369;0;367;0
WireConnection;290;0;248;0
ASEEND*/
//CHKSM=649498BD7243A0A726771256ED574819B1298C6A