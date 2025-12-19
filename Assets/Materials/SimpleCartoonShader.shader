Shader "Custom/SimpleMinimapShader"
{
    Properties
    {
        _Color ("Base Color", Color) = (1, 1, 1, 1)  // Default white color
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            // Input properties
            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = _Color;  // Apply the solid color
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return i.color;  // Return the solid color without any lighting
            }

            ENDCG
        }
    }
    FallBack "Unlit/Color"
}
