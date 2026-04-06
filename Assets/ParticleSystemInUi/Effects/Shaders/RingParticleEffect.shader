Shader "MyShaders/RingParticleEffect"
{
    Properties
    {
    //# Blending
    //#
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Blend Source", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Blend Destination", Float) = 10
    
    //# --------------------------------------------------------

    //# Textures
    //#

        _MainTex ("Texture", 2D) = "white" {}
        [Toggle] _SingleChannel ("Single Channel Texture", Float) = 0

    //# --------------------------------------------------------

    //# Ring
    //#

        [Toggle(_CFXR_RADIAL_UV)] _UseRadialUV ("Enable Radial UVs", Float) = 0
        _RingTopOffset ("Ring Offset", float) = 0.05
        [Toggle(_CFXR_WORLD_SPACE_RING)] _WorldSpaceRing ("World Space", Float) = 0
        
        // Вынесенные глобальные переменные
        _RingSmooth ("Ring Smoothness", Float) = 0.1
        _RingRotation ("Ring Rotation", Float) = 0

    //# --------------------------------------------------------

        _HdrMultiply ("HDR Multiplier", Float) = 2
    }
    
    Category
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }
        Blend [_SrcBlend] [_DstBlend]
        Cull Off
        ZWrite Off

        SubShader
        {
            Pass
            {
                Name "BASE"
                Tags { "LightMode"="UniversalForward" }
                ZTest Always

                CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag
                
                #pragma target 2.0
                
                #pragma multi_compile_fog

                #pragma shader_feature_local _ _CFXR_RADIAL_UV
                #pragma shader_feature_local _ _CFXR_WORLD_SPACE_RING

                #include "UnityCG.cginc"

                // Properties
                sampler2D _MainTex;
                
                half _SingleChannel;
                half _HdrMultiply;
                float _RingTopOffset;
                float _RingSmooth;
                float _RingRotation;

                // Vertex input
                struct appdata
                {
                    float4 vertex : POSITION;
                    half4 color : COLOR;
                    float4 texcoord : TEXCOORD0;    // xy = uv, zw = random
                    float4 texcoord1 : TEXCOORD1;   // particle data: x = width, y = particle size
                    #if _CFXR_WORLD_SPACE_RING
                    float3 normal : NORMAL;
                    #endif
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                // Vertex to fragment
                struct v2f
                {
                    float4 pos : SV_POSITION;
                    half4 color : COLOR;
                    float4 uv_uv2 : TEXCOORD0;
                    float4 ringData : TEXCOORD1;
                    float4 uvRing_uvCartesian : TEXCOORD2;
                    UNITY_FOG_COORDS(3)
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                // Vertex shader
                v2f vert (appdata v)
                {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_TRANSFER_INSTANCE_ID(v, o);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                    // Extract ring parameters from texcoord1
                    float ringWidth = v.texcoord1.x;  // width из texcoord1.x
                    float particleSize = v.texcoord1.y; // size из texcoord1.y

                    // Используем глобальные переменные
                    float ringSmooth = _RingSmooth;
                    float ringRotation = _RingRotation;

                    // Avoid artifacts when vertices are pushed too much
                    ringWidth = min(particleSize, ringWidth);

                    // Prepare ring data for fragment shader
                    o.ringData.x = pow(1 - ringWidth / particleSize, 2);  // inner radius
                    o.ringData.y = 1 - _RingTopOffset;                    // outer radius
                    o.ringData.z = ringSmooth / particleSize;             // smoothing
                    o.ringData.w = ringRotation;                          // rotation

                    // Regular ring UVs
                    float2 uv = v.texcoord.xy + float2(ringRotation, 0);
                    o.uvRing_uvCartesian.xy = 1 - uv;

                    #if _CFXR_WORLD_SPACE_RING
                        // World space ring with width offset
                        v.vertex.xyz = v.vertex.xyz - v.normal.xyz * v.texcoord.y * ringWidth;
                        o.pos = UnityObjectToClipPos(v.vertex);
                    #else
                        // Screen space ring with width offset
                        float4 m = mul(UNITY_MATRIX_V, v.vertex);
                        m.xy += -v.texcoord.zw * v.texcoord.y * ringWidth;
                        o.pos = mul(UNITY_MATRIX_P, m);
                    #endif

                    // Calculate cartesian UVs for accurate ring calculation
                    o.uvRing_uvCartesian.zw = v.texcoord.zw - v.texcoord.zw * v.texcoord.y * ringWidth / particleSize;

                    o.color = v.color;
                    o.uv_uv2 = v.texcoord;

                    UNITY_TRANSFER_FOG(o, o.pos);
                    return o;
                }

                // Fragment shader
                half4 frag (v2f i) : SV_Target
                {
                    UNITY_SETUP_INSTANCE_ID(i);
                    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                    // Procedural ring generation
                    float b = i.ringData.x; // inner radius
                    float t = i.ringData.y; // outer radius
                    float smooth = i.ringData.z; // smoothing
                    float gradient = dot(i.uvRing_uvCartesian.zw, i.uvRing_uvCartesian.zw);
                    
                    // Create ring with smooth edges
                    float ring = saturate( 
                        smoothstep(b, b + smooth, gradient) - 
                        smoothstep(t - smooth, t, gradient) 
                    );

                    // Sample texture with radial or regular UVs
                    half4 mainTex;
                    #if _CFXR_RADIAL_UV
                        // Polar coordinates for radial UVs
                        float2 radialUv = float2(
                            (atan2(i.uvRing_uvCartesian.z, i.uvRing_uvCartesian.w) / UNITY_PI) * 0.5 + 0.5 + 0.23 - i.ringData.w,
                            (gradient * (1.0 / (t - b)) - (b / (t - b))) * 0.9 - 0.92 + 1
                        );
                        mainTex = tex2D(_MainTex, radialUv);
                    #else
                        mainTex = tex2D(_MainTex, i.uvRing_uvCartesian.xy);
                    #endif

                    if (_SingleChannel) 
                        mainTex = half4(1, 1, 1, mainTex.r);

                    mainTex *= ring;

                    half3 particleColor = mainTex.rgb * i.color.rgb;
                    half particleAlpha = mainTex.a * i.color.a;

                    // HDR boost
                    if (_HdrMultiply > 0)
                    {
                        #ifdef UNITY_COLORSPACE_GAMMA
                            half hdrMultiply = LinearToGammaSpaceApprox(_HdrMultiply);
                        #else
                            half hdrMultiply = _HdrMultiply;
                        #endif
                        particleColor.rgb *= hdrMultiply * 2.0;
                    }

                    half4 finalColor = half4(particleColor, particleAlpha);

                    UNITY_APPLY_FOG(i.fogCoord, finalColor);
                    return finalColor;
                }

                ENDCG
            }
        }
    }
    
    CustomEditor "CartoonFX.MaterialInspector"
}