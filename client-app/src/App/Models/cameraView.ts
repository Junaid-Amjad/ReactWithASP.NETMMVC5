export interface CameraView {
  guid: string;
  url: string;
  FilePath: string;
  FileName: string;
  isProcessed: boolean | undefined;
  IPAddressPath: string | undefined;
  isFileProcessed: boolean | undefined;
  outputFolder: string;
  processID: number;
  noofColumns: number;
  layoutName: string;
}
