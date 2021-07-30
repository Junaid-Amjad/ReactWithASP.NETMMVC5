import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { GridLayoutDetail } from "../apiClass/GridLayout/GridLayoutDetail";
import { GridLayoutMaster } from "../apiClass/GridLayout/GridLayoutMaster";
import { convertAsciiToString } from "../common/class/globalCollection";
import { GridLayoutDto } from "../DTO/GridLayoutDto";
import { ICameraSettingList } from "../Models/gridLayout";
import { IGridLayout } from "../Models/IGridLayout";

interface Ilistofcamera {
  text: string;
  value: string;
}

export default class GridLayoutStore {
  loadingInitial = false;
  isEditMode = false;
  isRefreshNeeded = true;
  objectOfCameras: ICameraSettingList[] = [];
  listOfCameraIP: Ilistofcamera[] = [];
  objectOfGridLayoutMaster: GridLayoutMaster[] = [];

  constructor() {
    makeAutoObservable(this);
  }

  deleteGridLayout = async (gridLayoutMasterID: number) => {
    this.loadingInitial = true;
    try {
      await agent.gridLayout.deleteGridLayout(gridLayoutMasterID);
      runInAction(() => {
        this.isRefreshNeeded = true;
        this.loadingInitial = false;
      });
    } catch (error) {
      console.log(error);
      this.loadingInitial = false;
      this.isRefreshNeeded = true;
    }
  };

  loadGridLayoutDetail = async (gridLayoutMasterID: string) => {
    gridLayoutMasterID = convertAsciiToString(gridLayoutMasterID);
    let ID: number = parseInt(gridLayoutMasterID);
    let loadingGridLayoutData: IGridLayout = {
      GridLayoutMasterID: 0,
      column: "",
      nameOfTheGrid: "",
      cameras: [],
    };
    this.loadingInitial = true;
    this.isEditMode = true;

    try {
      var resultfromGridLayoutDetail =
        await agent.gridLayout.loadGridLayoutDetail(ID);
      runInAction(() => {
        loadingGridLayoutData!.GridLayoutMasterID =
          resultfromGridLayoutDetail.master.gridLayoutMasterID;
        loadingGridLayoutData!.column =
          resultfromGridLayoutDetail.master.noofColumns;
        loadingGridLayoutData!.nameOfTheGrid =
          resultfromGridLayoutDetail.master.layoutName;

        resultfromGridLayoutDetail.detail.forEach((value, index) => {
          loadingGridLayoutData!.cameras.push({
            cameraIP: value.cameraIP,
            itemindex: index,
          });
        });
        this.loadingInitial = false;
      });
      return loadingGridLayoutData;
    } catch (error) {
      console.log("Error in Loading");
      runInAction(() => {
        this.loadingInitial = false;
        this.isEditMode = false;
        return loadingGridLayoutData;
      });
    }
  };

  loadgridLayoutMasterData = async () => {
    if (this.loadingInitial) return; //For duplicate request.
    this.loadingInitial = true;
    try {
      while (this.objectOfGridLayoutMaster.length > 0) {
        this.objectOfGridLayoutMaster.pop();
      }

      var resultOfGridLayoutMaster =
        await agent.gridLayout.getgridLayoutMaster();
      runInAction(() => {
        resultOfGridLayoutMaster.forEach((value) => {
          this.objectOfGridLayoutMaster.push(value);
        });
        this.loadingInitial = false;
        this.isRefreshNeeded = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loadingInitial = false;
      });
    }
  };

  updateGridLayout = async (gridLayout: IGridLayout) => {
    this.loadingInitial = true;
    try {
      const valuedecentralized = await this.convertUILayoutToDBLayout(
        gridLayout
      );
      await agent.gridLayout.updateGridLayout(
        gridLayout.GridLayoutMasterID,
        valuedecentralized
      );
      runInAction(() => {
        this.loadingInitial = false;
        this.isEditMode = false;
        this.isRefreshNeeded = true;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loadingInitial = false;
        this.isRefreshNeeded = true;
        this.isEditMode = false;
      });
    }
  };

  loadCameraList = async () => {
    this.loadingInitial = true;
    try {
      this.objectOfCameras.slice(0, this.objectOfCameras.length);
      this.listOfCameraIP.slice(0, this.listOfCameraIP.length);

      const listobjectOfCameras = await agent.gridLayout.getresultofcamera();
      runInAction(() => {
        listobjectOfCameras.forEach((camera) => {
          let obj: Ilistofcamera = {
            text: camera.videoSourceString,
            value: camera.videoSourceString,
          };
          this.listOfCameraIP.push(obj);
        });
        this.loadingInitial = false;
        this.isRefreshNeeded = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loadingInitial = false;
      });
    }
  };

  convertUILayoutToDBLayout = async (UILayout: IGridLayout) => {
    let cameraString: GridLayoutDetail[] = [];
    UILayout.cameras.forEach((value, index) => {
      cameraString.push({ cameraIP: value.cameraIP });
    });
    const valuedecentralized: GridLayoutDto = {
      master: {
        gridLayoutMasterID: 0,
        layoutName: UILayout.nameOfTheGrid,
        noofColumns: UILayout.column,
      },
      detail: cameraString,
    };
    return valuedecentralized;
  };

  saveGridLayout = async (gridLayout: IGridLayout) => {
    this.loadingInitial = true;
    try {
      const valuedecentralized = await this.convertUILayoutToDBLayout(
        gridLayout
      );
      await agent.gridLayout.saveGridLayout(valuedecentralized);
      runInAction(() => {
        this.loadingInitial = false;
        this.isEditMode = false;
        this.isRefreshNeeded = true;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loadingInitial = false;
        this.isRefreshNeeded = true;
      });
    }
  };
}
