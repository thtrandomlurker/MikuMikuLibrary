#version 330

#define saturate(x) clamp(x, 0.0, 1.0)

out vec4 oColor;

in vec3 fPosition;
in vec3 fNormal;
in vec3 fTangent;
in vec3 fBitangent;
in vec2 fTexCoord0;
in vec2 fTexCoord1;
in vec4 fColor0;

uniform bool uHasNormal;
uniform bool uHasTexCoord0;
uniform bool uHasTexCoord1;
uniform bool uHasColor0;
uniform bool uHasTangent;

uniform bool uHasDiffuseTexture;
uniform bool uHasAmbientTexture;
uniform bool uHasNormalTexture;
uniform bool uHasSpecularTexture;
uniform bool uHasReflectionTexture;
uniform bool uHasToonCurveTexture;

uniform sampler2D uDiffuseTexture;
uniform sampler2D uAmbientTexture;
uniform sampler2D uNormalTexture;
uniform sampler2D uSpecularTexture;
uniform samplerCube uReflectionTexture;
uniform sampler2D uToonCurveTexture;

uniform vec4 uDiffuseColor;
uniform vec4 uAmbientColor;
uniform vec4 uSpecularColor;
uniform float uShininess;
uniform int uAnisoDirection;
uniform bool uPunchThrough;

uniform vec3 uViewPosition;

uniform vec4 uLightPosition;
uniform vec4 uLightAmbient;
uniform vec4 uLightDiffuse;
uniform vec4 uLightSpecular;
uniform vec4 uLightFresnel;
uniform vec3 uLightToneCurve;

const float ALPHA_THRESHOLD = 0.5;

vec4 divsq(vec4 a, float b)
{
	vec4 tmp = a / sqrt(abs(b));
	vec4 choice = abs(a);
	return mix(a, tmp, vec4(choice.x > 0, choice.y > 0, choice.z > 0, choice.w > 0));
}

vec2 yccLookup(float x)
{
    float v9 = 1.5;
    float samples = 32;
    float scale = 1.0 / samples;
    float i = x * 16 * samples;
    float v11 = exp(-i * scale);
    float v10 = pow(1.0 - v11, v9);
    v11 = v10 * 2.0 - 1.0;
    v11 *= v11;
    v11 *= v11;
    v11 *= v11;
    v11 *= v11;

    return vec2(v10, v10 * (samples / i) * (1.0 - v11));
}

vec3 yccToneMap(vec3 c)
{
    float exposure = 2.0;

    vec4 color;
    color.rgb = c;

    color.y = dot(color.rgb, vec3(0.30, 0.59, 0.11));
    color.rb -= color.y;
    color.yw = yccLookup(color.y * exposure * 0.0625);
    color.rb *= exposure * color.w;
    color.w = dot(color.rgb, vec3(-0.508475, 1.0, -0.186441));
    color.rb += color.y;
    color.g = color.w;

    return color.rgb;
}

void standard()
{
    vec3 viewDirection = normalize(uViewPosition - fPosition);
    vec3 lightDirection = normalize(vec3(0,0,0) + uLightPosition.xyz);
    vec3 halfwayDirection = normalize(viewDirection + lightDirection);

    vec4 diffuseColor = uDiffuseColor;
    vec4 specularColor = uSpecularColor;
    vec3 ambientColor = uAmbientColor.rgb;

    specularColor.rgb *= 2.0;

    if (uHasDiffuseTexture && uHasTexCoord0)
        diffuseColor *= texture(uDiffuseTexture, fTexCoord0);

    if (uHasColor0)
        diffuseColor *= fColor0;
        
    if (uPunchThrough && diffuseColor.a < ALPHA_THRESHOLD)
        discard;

    if (uHasSpecularTexture && uHasTexCoord0)
        specularColor *= texture(uSpecularTexture, fTexCoord0);

    if (uHasAmbientTexture && uHasTexCoord1)
        diffuseColor.rgb *= texture(uAmbientTexture, fTexCoord1).rgb;

    float fresnel = 0;
    vec3 directLighting = vec3(0);

    vec3 normal = normalize(fNormal);
    if (uHasNormal)
    {
        if (uHasTangent && uHasNormalTexture && uHasTexCoord0)
        {
            vec4 tmp = texture(uNormalTexture, fTexCoord0);
            tmp.xy = tmp.xy * 2.0 - 1.0;
            tmp.zw = tmp.xy * tmp.xy * tmp.xy;
            tmp *= vec4(1.5, 1.5, 2.0, 2.0);
            tmp.xy += tmp.zw;

            normal += fTangent * tmp.x;
            normal += fBitangent * tmp.y;
            normal = normalize(normal);
        }

        fresnel = pow(1 - clamp(dot(normal, viewDirection), 0, 1), 5);

        directLighting += diffuseColor.rgb;
        
        if (uAnisoDirection > 0 && uAnisoDirection < 3 && uHasTangent && uHasTexCoord0)
        {
            float dotTH = dot(uAnisoDirection == 2 ? normalize(fBitangent) : normalize(fTangent), halfwayDirection);
            float sinTH = sqrt(1 - dotTH * dotTH);
            float dirAtten = smoothstep(-1, 0, dotTH);
            directLighting += (dirAtten * pow(sinTH, uShininess)) * specularColor.rgb;
        }
        else
        {
            directLighting += pow(clamp(dot(normal, halfwayDirection), 0, 1), uShininess) * specularColor.rgb;
        }
    }

    directLighting *= clamp(dot(normal, lightDirection), 0, 1);

    vec3 indirectLighting = mix(ambientColor, vec3(1), fresnel) * diffuseColor.rgb;

    if (uHasNormal && uHasReflectionTexture)
        indirectLighting += texture(uReflectionTexture, reflect(-viewDirection, normal)).rgb * mix(specularColor.w * (dot(normal, lightDirection) * 0.5 + 0.5), 1, fresnel);

    oColor = vec4(yccToneMap(directLighting + indirectLighting), diffuseColor.a);
}

void chara()
{
    /*vec4 diffuse = texture(uDiffuseTexture, fTexCoord0);

    if (uPunchThrough && diffuse.a < ALPHA_THRESHOLD)
        discard;

    vec4 specular = texture(uSpecularTexture, fTexCoord0);

    vec3 normal = normalize(fNormal);
    vec3 viewDirection = normalize(uViewPosition - fPosition);
    vec3 halfwayDirection = normalize(viewDirection + lightDirection);

    if (uAnisoDirection > 0)
        halfwayDirection = normalize(viewDirection + vec3(0, 1, 0));

    float nDotV = dot(normal, viewDirection);
    float nDotL = dot(normal, lightDirection);
    float nDotH = dot(normal, halfwayDirection);

    vec3 diffuseToonCurve = texture(uToonCurveTexture, vec2(nDotL * 0.5 + 0.5, 0.875)).rgb;
    vec3 specularToonCurve = texture(uToonCurveTexture, vec2(clamp(nDotH, 0, 1), 0.625)).rgb;
    vec3 fresnelToonCurve = texture(uToonCurveTexture, vec2(clamp(1 - nDotV, 0, 1), 0.375)).rgb;

    vec3 diffuseLighting = diffuse.rgb * diffuse.rgb * diffuseToonCurve.rgb;
    vec3 specularLighting = specular.rgb * specular.rgb * specularToonCurve.rgb * 1.551;

    if (uAnisoDirection == 0)
        specularLighting *= (1 - pow(clamp(1 - nDotL, 0, 1), 8));
    
    vec3 fresnelLighting = fresnelToonCurve * specular.a * 0.431;

    oColor = vec4(pow(clamp(diffuseLighting + specularLighting + fresnelLighting, 0, 1), vec3(0.625)), diffuse.a);*/

    vec3 lightDirection = normalize(vec3(0,0,0) + uLightPosition.xyz);
    
    vec3 viewDirection = normalize(uViewPosition - fPosition);

    float diffuseToneCurveOfs = 0.875;

    vec3 halfwayDirection = viewDirection + lightDirection;

    if (uAnisoDirection > 0)
        halfwayDirection = normalize(viewDirection + vec3(0, 1, 0));

    vec3 normal = normalize(fNormal);

    float nDotL = dot(normal, lightDirection);

    vec3 nHalfwayDirection = normalize(halfwayDirection);
    float cond = nDotL;

    float nDotV = saturate(dot(normal, viewDirection));
    float nDotL_adj = (nDotL + 1) * 0.5;

    vec4 specularTex = texture(uSpecularTexture, fTexCoord0);
    specularTex.rgb *= specularTex.rgb;
    float nNdotLadj = saturate(-nDotL + 1);

    vec3 fresnelColor = specularTex.w * (uLightFresnel.rgb * 0.8);

    float nDotH = saturate(dot(normal, nHalfwayDirection));

    float nNdotLadjadj = nNdotLadj * nNdotLadj;

    float specularToneCurveOfs = 0.625;

    nDotL = nNdotLadjadj * nNdotLadjadj;

    //vec3 emissionColor = g_material_state_emission.rgb;

    vec3 specularCol = -nDotL * specularTex.xyz + specularTex.xyz;
	
	vec3 lightDir = normalize(lightDirection);

    specularCol.xyz *= uLightSpecular.rgb;

    if (cond > 0 || uAnisoDirection > 0) {
      vec3 specularToneCurve = texture(uToonCurveTexture, vec2(nDotH, specularToneCurveOfs)).rgb;
      specularCol *= specularToneCurve;
    }

    vec3 diffuseToneCurve = texture(uToonCurveTexture, vec2(nDotL_adj, diffuseToneCurveOfs)).rgb;

    // for the tonecurve it sets R2.w to R3.w - tone_curve.xxxx.w
    // then it sets r1.x to saturate(r2.w * tone_curve.yyyy.x)
    // last it does is R3.xyz = R1.xyz * pr
    if (uLightToneCurve.z > 0.0) {
	    float toneStart = nDotL - uLightToneCurve.x;
	    float toneDepth = saturate(toneStart * uLightToneCurve.y);
	    
	    vec3 toneColor = toneDepth * -uLightAmbient.xyz + toneDepth;
	    toneColor += uLightAmbient.xyz;
	    diffuseToneCurve = (toneColor - diffuseToneCurve) * uLightToneCurve.z + diffuseToneCurve;
    }

    float nNdotVadj = -nDotV + 1;
    vec4 diffuseTex = texture(uDiffuseTexture, fTexCoord0);

    if (uPunchThrough && diffuseTex.a < ALPHA_THRESHOLD)
        discard;

    vec3 diffuseCol = (diffuseToneCurve.xyz * uLightDiffuse.rgb);

    float fresnelToneCurveOfs = 0.375;

    vec3 fresnelToneCurve = texture(uToonCurveTexture, vec2(nNdotVadj, fresnelToneCurveOfs)).xyz;

    vec3 directLighting = fresnelToneCurve * fresnelColor + specularCol;

    vec3 combinedLighting = (diffuseCol * diffuseTex.xyz + directLighting);

    oColor = vec4(yccToneMap(combinedLighting), diffuseTex.a);
	//stencil_depth_target = float4(0, frg_position.z, 0, 0);
    return;
}

void main()
{
    if (uHasToonCurveTexture)
        chara();

    else
        standard();
}