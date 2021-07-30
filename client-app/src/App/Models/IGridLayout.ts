export interface IGridLayout {
  GridLayoutMasterID: number;
  nameOfTheGrid: string;
  column: string;
  cameras: Array<{ cameraIP: string; itemindex: number }>;
}
