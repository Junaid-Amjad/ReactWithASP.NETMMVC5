import { GridLayoutDetail } from "../apiClass/GridLayout/GridLayoutDetail";
import { GridLayoutMaster } from "../apiClass/GridLayout/GridLayoutMaster";

export interface GridLayoutDto {
  master: GridLayoutMaster;
  detail: GridLayoutDetail[];
}
