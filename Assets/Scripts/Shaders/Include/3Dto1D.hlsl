uint3 GridSize;
uint GridSizeXByY; // GridSize.x * GridSize.y
uint GridSizeXByYByZ; // GridSize.x * GridSize.y * GridSize.z

uint GetIndex1D(uint3 index3D)
{
    return index3D.x + index3D.y * GridSize.x + index3D.z * GridSizeXByY;
}