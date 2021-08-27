import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import convertStringToAscii, { sleep } from "../common/class/globalCollection";
import { CameraView } from "../Models/cameraView";

interface cameralaodeddetails {
  FilePath: string;
  iterate: number;
  maximumiteratealllowed: number;
}

export default class CameraViewStore {
  loadingInitial = false;
  browserLinkChanged = false;
  calltimerInterval = false;
  maximumallowed = 4;
  istimerstart = false;
  cameraViewRegistry = new Map<string, CameraView>();
  processedcamerafile: cameralaodeddetails[] = [];
  noofcolumns = 1;
  layoutName = "";
  isReloading = false;

  constructor() {
    makeAutoObservable(this);
  }

  get getCameraViewResult() {
    return Array.from(this.cameraViewRegistry.values()).sort();
  }
  deleteFiles = async (fileName: string) => {
    //this.loadingToDeleteFile=true;
    try {
      this.browserLinkChanged = false;

      const filedeleted = await agent.StreamIP.deleteFiles(fileName);
      runInAction(() => {
        if (filedeleted.statusID !== 1) return true;
        else return false;
      });
    } catch (error) {
      console.log(error);
    }
  };

  getIndividualIPStream = async (UUID: string) => {
    this.isReloading = true;
    if (!this.istimerstart) this.calltimerInterval = false;
    let resultfromCameraObject = this.cameraViewRegistry.get(UUID);
    if (resultfromCameraObject) {
      await this.callTheFileForProcessing(resultfromCameraObject!);
    }
    runInAction(() => {
      this.isReloading = false;
    });
  };

  loadCameraView = async (id: string) => {
    this.loadingInitial = true;
    let isRecordFound = false;
    this.calltimerInterval = false;
    try {
      this.cameraViewRegistry.clear();
      await this.deleteFiles("0");
      const camerav = await agent.cameraView.getLiveVideUrlFromDB(id);
      runInAction(async () => {
        this.browserLinkChanged = false;
        camerav.forEach((cameraview) => {
          this.setCameraResult(cameraview);
          this.noofcolumns = cameraview.noofColumns;
          this.layoutName = cameraview.layoutName;
          isRecordFound = true;
          if (isRecordFound) {
            this.getCameraViewResult.forEach(async (value, key) => {
              await this.callTheFileForProcessing(value);
            });
          }
          this.loadingInitial = false;
        });
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loadingInitial = false;
      });
    }
  };
  private uuidv4() {
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(
      /[xy]/g,
      function (c) {
        var r = (Math.random() * 16) | 0,
          v = c === "x" ? r : r && 0x3 | 0x8;
        return v.toString(16);
      }
    );
  }

  private setCameraResult = (cameraView: CameraView) => {
    cameraView.FileName = this.uuidv4() + ".m3u8";
    cameraView.FilePath = this.uuidv4() + "/" + cameraView.FileName;
    this.cameraViewRegistry.set(cameraView.guid.toString(), cameraView);
  };

  async processRemainingFile(camerafile: string) {
    const FileExist = await agent.StreamIP.getFileExist(camerafile);
    if (FileExist.statusID === 1) {
      return true;
    } else return false;
  }

  async isFileProcessed(camerafile: string) {
    const FileExist = await agent.StreamIP.getFileExist(camerafile);
    if (FileExist.statusID === 1) {
      return true;
    } else return false;
  }

  async loopandcheckingvalue() {
    let timeInterval = setInterval(async () => {
      this.istimerstart = true;
      var i = this.processedcamerafile.length;
      while (i--) {
        const value = this.processedcamerafile[i];
        if (value.iterate > value.maximumiteratealllowed) {
          runInAction(() => {
            this.getCameraViewResult.forEach((valueofcamera, key) => {
              if (
                convertStringToAscii(valueofcamera.FilePath) === value.FilePath
              ) {
                valueofcamera.isProcessed = false;
                valueofcamera.isFileProcessed = false;
                this.deleteFiles(valueofcamera.processID.toString());
              }
            });
          });
          this.processedcamerafile.splice(i, 1);
        } else {
          const result = await this.processRemainingFile(value.FilePath);
          if (result) {
            runInAction(() => {
              this.getCameraViewResult.forEach((valueofcamera, key) => {
                if (
                  convertStringToAscii(valueofcamera.FilePath) ===
                  value.FilePath
                ) {
                  valueofcamera.isFileProcessed = true;
                }
              });
            });
            this.processedcamerafile.splice(i, 1);
          }
          value.iterate++;
        }
      }
      if (this.processedcamerafile.length === 0) {
        clearInterval(timeInterval);
        this.istimerstart = false;
      }
      if (this.browserLinkChanged) {
        this.deleteFiles("0");
      }
    }, 5000);
  }

  async callTheFileForProcessing(cameraView: CameraView) {
    try {
      if (!cameraView.isProcessed) {
        const cameraurl = convertStringToAscii(cameraView.url);
        const camerafile = convertStringToAscii(cameraView.FilePath);
        cameraView.IPAddressPath = process.env.REACT_APP_IP_Addr;
        const result = await agent.StreamIP.setStreamOfCamera(
          camerafile,
          cameraurl
        );
        runInAction(async () => {
          if (result.iPfound) {
            cameraView.isProcessed = true;
            cameraView.processID = result.processID;
            sleep(5000);
            if (await this.isFileProcessed(camerafile)) {
              runInAction(() => {
                cameraView.isFileProcessed = true;
              });
            } else {
              let isFound = false;
              let s: cameralaodeddetails = {
                FilePath: camerafile,
                maximumiteratealllowed: this.maximumallowed,
                iterate: 0,
              };
              for (let i = 0; i < this.processedcamerafile.length; i++) {
                if (this.processedcamerafile[i].FilePath === s.FilePath) {
                  isFound = true;
                  break;
                }
              }

              if (!isFound) {
                this.processedcamerafile.push(s);
              }
              if (!this.calltimerInterval) {
                this.calltimerInterval = true;
                this.loopandcheckingvalue();
              }
            }
          } else {
            cameraView.isProcessed = false;
            cameraView.isFileProcessed = false;
          }
        });
      }
    } catch (error) {
      console.log(error);
    }
  }
}
