Shader "Custom/DimensionSeere"
{
    Properties
    {

    }
    SubShader
    {
        Tags { "Queue" = "Geometry-1" }
        
        ColorMask 0

        ZWrite Off

        Stencil
        {
            Ref 1
            Comp always
            Pass replace
        }

        CGPROGRAM
        
        #pragma surface surf Lambert

        struct Input
        {
            float3 worldPos;//Because empty input causes error
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = fixed4(1,1,1,1);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
