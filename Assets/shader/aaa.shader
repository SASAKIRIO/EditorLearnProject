Shader "HIKALAB/Unlit/aaa"
{
    Properties
    {
        /*[Header(GraphicsSettings)]
        [KeywordEnum(Reality, Toon)]*/
       /* _Graphics ("Graphics Mode", Float) = 0*/
        [Header(MAIN)]
        [Space(10)]
        _MainTex ("Main Texture", 2D) = "white" {}
        [NoScaleOffset]
        [Normal]_NormalMap ("Normal Texture", 2D) = "bump" {}
        _NormalScale("Normal Scale", Range(0, 2)) = 0
        _RampTex ("Ramp Texture", 2D) = "white"{}
        [Space(15)]
        [Header(COVER)]
        [Toggle(COVER_ON_OFF)]
        _Check ("[Cover] ON / OFF", Float) = 0
        [Space(10)]
        _CoverTex ("Cover Texture", 2D) = "white"{}

        _CoverPower ("Cover power", Float) = 0
        _CoverDirectionX("Cover Direction X", Range(-1,1))=0
        _CoverDirectionY("Cover Direction Y", Range(-1,1))=0
        _CoverDirectionZ("Cover Direction Z", Range(-1,1))=0

        // [HideInInspector]
        // _CoverDirection("CD",Vector) = (_CoverDirectionX,_CoverDirectionY,_CoverDirectionZ,_CoverDirectionW);
        

        _CoverAmount ("Cover Amount", Range(0, 1)) = 1
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature _ COVER_ON_OFF
            
           /* #pragma shader_feature _ GRAPHICS_REALITY_GRAPHICS_TOON*/
           
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv_Main : TEXCOORD0;
                float2 uv_Cover : TEXCOORD1;
                float4 vertex : SV_POSITION;

                float3 normal : NORMAL;
                float2 uvNormal : TEXCOORD2;
                float4 tangent : TANGENT;
                float3 binormal : TEXCOORD3;
            };


            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NormalMap;
            float4 _NormalMap_ST;
            sampler2D _RampTex;
            sampler2D _CoverTex;
            float4 _CoverTex_ST;

            float _NormalScale;

            float _CoverDirectionX;
            float _CoverDirectionY;
            float _CoverDirectionZ;

            float _CoverPower;
            // float3 _CoverDirection;



            float _CoverAmount;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv_Main = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv_Cover = TRANSFORM_TEX(v.uv, _CoverTex);

                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uvNormal = TRANSFORM_TEX(v.uv, _NormalMap);
                o.tangent = v.tangent;
                o.tangent.xyz = UnityObjectToWorldDir(v.tangent.xyz);
                o.binormal = normalize(cross(v.normal, v.tangent.xyz) * v.tangent.w * unity_WorldTransformParams.w);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 localNormal = UnpackNormal(tex2D(_NormalMap, i.uvNormal)), _NormalScale;
                i.normal = i.tangent * localNormal.x + i.binormal * localNormal.y + i.normal * localNormal.z;
                fixed4 maintex = tex2D(_MainTex, i.uv_Main);
                fixed4 covertex = tex2D(_CoverTex, i.uv_Cover);

            /*#ifdef GRAPHICS_REALITY*/
                /*[[  RAMPテクスチャ  ]]*/
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                half Direction = dot(i.normal, -lightDirection) * 0.5;
                half3 ramptex = tex2D(_RampTex, float2(Direction, Direction)).rgb;
                maintex.rgb *= _LightColor0.rgb * ramptex;
            /*#else
                
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                fixed3 ligColor = _LightColor0.xyz;

                float t = dot(i.normal, lightDirection);
                t = max(0, t);
                
                float3 diffuseLig = ligColor * t;
                float4 finalColor = float4(1, 1, 1, 1);
                finalColor.xyz *= diffuseLig;
                return finalColor;
            #endif*/


            // #ifdef COVER_ON_OFF 
            //     float value = dot(normalize(i.normal), _CoverDirection);

            //     if (value < 1 - _CoverAmount)
            //         value = 0;

            //     //fixed4 maintex = tex2D(_MainTex, i.uv_Main);
            //     /*fixed4 covertex = tex2D(_CoverTex, i.uv_Cover);*/
                
            //     return lerp(maintex, covertex, value);

            // #else
            //     return maintex;
            // #endif

            #ifdef COVER_ON_OFF 
                float value = dot(normalize(i.normal), float3(_CoverDirectionX,_CoverDirectionY,_CoverDirectionZ)*_CoverPower);

                if (value < 1 - _CoverAmount)
                    value = 0;

                //fixed4 maintex = tex2D(_MainTex, i.uv_Main);
                /*fixed4 covertex = tex2D(_CoverTex, i.uv_Cover);*/
                

                return lerp(maintex, covertex, value);

            #else
                return maintex;
            #endif
                
            }
            ENDCG
        }
    }
}