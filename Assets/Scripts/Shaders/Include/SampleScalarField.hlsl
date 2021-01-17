#include "3Dto1D.hlsl"

StructuredBuffer<float> ScalarFieldToSample;
float BoundaryScale;

// Explore optimizing these without if statements
float SampleVertical(uint3 index, int direction)
{
    if (index.y == (GridSize.y - 1) || index.y == 0)
    {
        return BoundaryScale * ScalarFieldToSample[GetIndex1D(index)];
    }
    return ScalarFieldToSample[GetIndex1D(index + uint3(0, direction, 0))];
}

float SampleHorizontal(uint3 index, int direction)
{
    if (index.x == (GridSize.x - 1) || index.x == 0)
    {
        return BoundaryScale * ScalarFieldToSample[GetIndex1D(index)];
    }
    return ScalarFieldToSample[GetIndex1D(index + uint3(direction, 0, 0))];
}

float SampleDepth(uint3 index, int direction)
{
    if (index.z == (GridSize.z - 1) || index.z == 0)
    {
        return BoundaryScale * ScalarFieldToSample[GetIndex1D(index)];
    }
    return ScalarFieldToSample[GetIndex1D(index + uint3(0, 0, direction))];
}

float SampleUpDownRightLeftForwardBack(uint3 index)
{
    return SampleVertical(index, 1) + SampleVertical(index, -1) + SampleHorizontal(index, 1) + SampleHorizontal(index, -1) + SampleDepth(index, 1) + SampleDepth(index, -1);
}