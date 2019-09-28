// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New AmplifyShader"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Grass("Grass", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		_Float2("Float 2", Float) = 0
		_Float1("Float 1", Float) = 0
		_Float3("Float 3", Float) = 0
		_Color0("Color 0", Color) = (0.07983268,0.08536252,0.245283,0)
		_Float4("Float 4", Range( 0 , 1)) = 0
		_Rock("Rock", 2D) = "white" {}
		_rockForestAlpha("rockForestAlpha", 2D) = "white" {}
		_Grassheight("Grassheight", 2D) = "white" {}
		_rockHeight("rockHeight", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float4 _Color0;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Float4;
		uniform float _Float0;
		uniform float _Float2;
		uniform sampler2D _Rock;
		uniform float4 _Rock_ST;
		uniform sampler2D _Grass;
		uniform float4 _Grass_ST;
		uniform sampler2D _Grassheight;
		uniform float4 _Grassheight_ST;
		uniform sampler2D _rockForestAlpha;
		uniform float4 _rockForestAlpha_ST;
		uniform sampler2D _rockHeight;
		uniform float4 _rockHeight_ST;
		uniform float _Float1;
		uniform float _Float3;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float lerpResult28 = lerp( ase_vertex3Pos.z , ase_vertex3Pos.y , _Float4);
			float lerpResult4 = lerp( ( lerpResult28 + _Float0 ) , 0 , 0);
			float clampResult10 = clamp( ( lerpResult4 * _Float2 ) , 0 , 1 );
			float4 lerpResult7 = lerp( _Color0 , tex2D( _TextureSample0, uv_TextureSample0 ) , clampResult10);
			float2 uv_Rock = i.uv_texcoord * _Rock_ST.xy + _Rock_ST.zw;
			float2 uv_Grass = i.uv_texcoord * _Grass_ST.xy + _Grass_ST.zw;
			float2 uv_Grassheight = i.uv_texcoord * _Grassheight_ST.xy + _Grassheight_ST.zw;
			float2 uv_rockForestAlpha = i.uv_texcoord * _rockForestAlpha_ST.xy + _rockForestAlpha_ST.zw;
			float4 blendOpSrc51 = tex2D( _Grassheight, uv_Grassheight );
			float4 blendOpDest51 = tex2D( _rockForestAlpha, uv_rockForestAlpha );
			float4 clampResult60 = clamp( CalculateContrast(2,( saturate( (( blendOpDest51 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest51 - 0.5 ) ) * ( 1.0 - blendOpSrc51 ) ) : ( 2.0 * blendOpDest51 * blendOpSrc51 ) ) ))) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 lerpResult31 = lerp( tex2D( _Rock, uv_Rock ) , tex2D( _Grass, uv_Grass ) , clampResult60);
			float2 uv_rockHeight = i.uv_texcoord * _rockHeight_ST.xy + _rockHeight_ST.zw;
			float lerpResult21 = lerp( ( lerpResult28 + _Float1 ) , 0 , 0);
			float clampResult24 = clamp( ( lerpResult21 * _Float3 ) , 0 , 1 );
			float4 temp_cast_0 = (clampResult24).xxxx;
			float4 blendOpSrc55 = tex2D( _rockHeight, uv_rockHeight );
			float4 blendOpDest55 = temp_cast_0;
			float4 clampResult58 = clamp( CalculateContrast(2,( saturate( (( blendOpDest55 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest55 - 0.5 ) ) * ( 1.0 - blendOpSrc55 ) ) : ( 2.0 * blendOpDest55 * blendOpSrc55 ) ) ))) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 lerpResult16 = lerp( lerpResult7 , lerpResult31 , clampResult58);
			o.Albedo = lerpResult16.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15001
1934;414;1143;564;1502.054;-265.5656;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;29;-1631.014,141.207;Float;False;Property;_Float4;Float 4;7;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;2;-1575.476,-38.26493;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;18;-1354.7,29.17316;Float;False;763;341;Comment;6;3;11;12;4;8;28;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;28;-1257.014,61.20697;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1122.261,596.0606;Float;False;Property;_Float1;Float 1;4;0;Create;True;0;0;False;0;0;-0.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-1023.17,439.0099;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1319.657,264.9163;Float;False;Property;_Float0;Float 0;2;0;Create;True;0;0;False;0;0;2.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-787.5936,605.4092;Float;False;Property;_Float3;Float 3;5;0;Create;True;0;0;False;0;0;3.37;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;21;-866.1185,439.0099;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-1064.7,79.17316;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-658.5876,446.4886;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;32;-1260.978,1467;Float;True;Property;_rockForestAlpha;rockForestAlpha;9;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;33;-1326.447,1259.413;Float;True;Property;_Grassheight;Grassheight;10;0;Create;True;0;0;False;0;None;7339ac558c573a6408856d331b2a51a3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-952.7003,255.1731;Float;False;Property;_Float2;Float 2;3;0;Create;True;0;0;False;0;0;0.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;56;-580.0909,176.1936;Float;True;Property;_rockHeight;rockHeight;11;0;Create;True;0;0;False;0;None;52d319e16df91ce41bd135724c647e12;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;4;-936.7003,79.17316;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;24;-324.5942,423.5989;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;51;-599.0135,1104.586;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-760.7003,79.17316;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;59;-370.505,889.4003;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;55;-278.3725,162.7455;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;60;-317.52,761.8441;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;5;-795.937,-390.5231;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;740e96e5b465a5649bc3c3bd258fa736;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;25;-462.9398,-535.5616;Float;False;Property;_Color0;Color 0;6;0;Create;True;0;0;False;0;0.07983268,0.08536252,0.245283,0;0.1665628,0.1672303,0.1792453,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;57;-209.9409,-17.72919;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;30;-598.6876,553.4343;Float;True;Property;_Rock;Rock;8;0;Create;True;0;0;False;0;None;f51f14956aa15ea41b62a6555ea0654b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;10;-518.0562,32.00009;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-706.2963,783.3438;Float;True;Property;_Grass;Grass;1;0;Create;True;0;0;False;0;None;8104d96a9e066d84ab9983bd97e11abd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;7;-210.1639,-220.1761;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;58;-56.19817,-22.59218;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;31;-212.8976,623.4468;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;16;117.3204,-57.80881;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;335.2847,-40.08657;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;New AmplifyShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;2;3
WireConnection;28;1;2;2
WireConnection;28;2;29;0
WireConnection;19;0;28;0
WireConnection;19;1;20;0
WireConnection;21;0;19;0
WireConnection;3;0;28;0
WireConnection;3;1;8;0
WireConnection;22;0;21;0
WireConnection;22;1;23;0
WireConnection;4;0;3;0
WireConnection;24;0;22;0
WireConnection;51;0;33;0
WireConnection;51;1;32;0
WireConnection;11;0;4;0
WireConnection;11;1;12;0
WireConnection;59;1;51;0
WireConnection;55;0;56;0
WireConnection;55;1;24;0
WireConnection;60;0;59;0
WireConnection;57;1;55;0
WireConnection;10;0;11;0
WireConnection;7;0;25;0
WireConnection;7;1;5;0
WireConnection;7;2;10;0
WireConnection;58;0;57;0
WireConnection;31;0;30;0
WireConnection;31;1;6;0
WireConnection;31;2;60;0
WireConnection;16;0;7;0
WireConnection;16;1;31;0
WireConnection;16;2;58;0
WireConnection;0;0;16;0
ASEEND*/
//CHKSM=0585A558AC81D309679B84BFD70298D96ED1B56A