Shader "MyShader/OutlineEffect" {
    Properties {
        _OutlineWidth("Outline Width", Range(0, 10)) = 8
        _StartTime ("startTime", Float) = 0 // _StartTime用于控制每个选中的对象颜色渐变不同步
    }
 
    SubShader {
        Tags {
            // 渲染队列: Background(1000, 后台)、Geometry(2000, 几何体, 默认)、Transparent(3000, 透明)、Overlay(4000, 覆盖)
            "Queue" = "Transparent+110"
            "RenderType" = "Transparent"
            "DisableBatching" = "True"
        }
 
        // 将待描边物体的屏幕区域像素对应的模板值标记为1
        Pass {
            Name "StencilWrite"
            Tags { "LightMode" = "SRPDefaultUnlit" }

            Cull Off // 关闭剔除渲染, 取值有: Off、Front、Back, Off表示正面和背面都渲染
            ZTest Always // 总是通过深度测试, 使得物体即使被遮挡时, 也能生成模板
            ZWrite Off // 关闭深度缓存, 避免该物体遮挡前面的物体
            ColorMask 0 // 允许通过的颜色通道, 取值有: 0、R、G、B、A、RGBA的组合(RG、RGB等), 0表示不渲染颜色
 
            Stencil { // 模板测试, 只有通过模板测试的像素才会渲染
                Ref 1 // 设定参考值为1
                Pass Replace // 如果通过模板测试, 将像素的模板值设置为参考值(1), 模板值的初值为0, 没有Comp表示总是通过模板测试
            }
        }
 
        // 绘制模板标记外的物体像素, 即膨胀的外环上的像素
        Pass {
            Name "OutlinePass"
            Tags { "LightMode" = "UniversalForward" }

            Cull Off // 关闭剔除渲染, 取值有: Off、Front、Back, Off表示正面和背面都渲染
            ZTest Always // 总是通过深度测试, 使得物体即使被遮挡时, 也能生成描边
            ZWrite Off // 关闭深度缓存, 避免该物体遮挡前面的物体
            Blend SrcAlpha OneMinusSrcAlpha // 混合测试, 与背后的物体颜色混合
            ColorMask RGB // 允许通过的颜色通道, 取值有: 0、R、G、B、A、RGBA的组合(RG、RGB等), 0表示不渲染颜色
 
            Stencil { // 模板测试, 只有通过模板测试的像素才会渲染
                Ref 1 // 设定参考值为1
                Comp NotEqual // 这里只有模板值为0的像素才会通过测试, 即只有膨胀的外环上的像素能通过模板测试
            }
 
            CGPROGRAM
            #include "UnityCG.cginc"
 
            #pragma vertex vert
            #pragma fragment frag
 
            uniform float _OutlineWidth;
            uniform float _StartTime;
   
            struct a2v {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 smoothNormal : TEXCOORD3; // 平滑的法线, 对相同顶点的所有法线取平均值
            };
 
            struct v2f {
                float4 pos : SV_POSITION;
            };
 
            v2f vert(a2v v) {
                v2f o;
                float3 normal = any(v.smoothNormal) ? v.smoothNormal : v.normal; // 光滑的法线
                float3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, normal)); // 观察坐标系下的法线向量
                float3 viewPos = UnityObjectToViewPos(v.vertex); // 观察坐标系下的顶点坐标
                // 裁剪坐标系下的顶点坐标, 将顶点坐标沿着法线方向向外延伸, 延伸的部分就是描边部分
                // 乘以(-viewPos.z)是为了抵消透视变换造成的描边宽度近大远小效果, 使得物体无论距离相机多远, 描边宽度都不发生变化
                // 除以1000是为了将描边宽度单位转换到1mm(这里的宽度是世界坐标系中的宽度, 而不是屏幕上的宽度)
                o.pos = UnityViewToClipPos(viewPos + viewNormal * _OutlineWidth * (-viewPos.z) /1000);
                return o;
            }
 
            fixed4 frag(v2f i) : SV_Target {
                float t1 = sin(_Time.z - _StartTime); // _Time = float4(t/20, t, t*2, t*3)
                float t2 = cos(_Time.z - _StartTime);
                // 描边颜色随时间变化, 描边透明度随时间变化, 视觉上感觉描边在膨胀和收缩
                float alpha = abs(sin(_Time.y * 2)); // 随时间闪烁，频率可调
                return float4(1, 1, 1, alpha); // 固定白色，Alpha 在 0~1 之间波动
            }
 
            ENDCG
        }
    }
}