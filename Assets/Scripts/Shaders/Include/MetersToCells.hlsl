#include "3Dto1D.hlsl"
float InverseResolution;
float HalfSize_m;

uint3 Ge3DIndexFromPositionSafe(float3 position)
{
    uint3 index3D = (position + HalfSize_m) * (InverseResolution);
    return uint3(min(index3D.x, GridSize.x - 1), min(index3D.y, GridSize.y - 1), min(index3D.z, GridSize.z - 1));
}

uint3 Ge3DIndexFromPosition(float3 position)
{
    return (position + HalfSize_m) * (InverseResolution);
}

uint GetIndex1DFromPosition(float3 position)
{
    return GetIndex1D(Ge3DIndexFromPosition(position));
}

uint GetIndex1DFromPositionSafe(float3 position)
{
    return GetIndex1D(Ge3DIndexFromPositionSafe(position));
}
