import { makeAutoObservable, runInAction } from "mobx";
import { TreeNodeInArray } from "react-simple-tree-menu";
import agent from "../../api/agent";
import { ParameterForDelete } from "../../common/class/parameterForDelete";
import { MapList } from "../../Models/Map/mapList";
import { MapPosition } from "../../Models/Map/mapPosition";
import { store } from "../store";

export interface IMapCategories {
  text: string;
  value: number;
}
export interface IMapListWithPosition {
  mapListID: number;
  parentID: number;
  mapName: string;
  rotation: number;
  Position: { x: number; y: number };
  dimension: { width: number; height: number };
  bound: { top: number; bottom: number };
}

export default class Mapstore {
  isloadingMapFloor: boolean = false;
  isloadingInitial: boolean = false;
  isloading: boolean = false;
  isloadingPopup: boolean = false;
  isRefreshRequired: boolean = true;

  defaultImageURL: string =
    "https://react.semantic-ui.com/images/wireframe/image.png";
  serverAPILink: string | undefined = "";

  defaultWidthheight: number = 50;

  MapCategories: IMapCategories[] = [];
  savedMapList: MapList[] = [];
  mapChildImageList: IMapListWithPosition[] = [];
  mapChildImageListAll: IMapListWithPosition[] = [];
  treeView: TreeNodeInArray[] = [];

  constructor() {
    makeAutoObservable(this);
  }
  deleteMapListAll = async (parentID: number) => {
    try {
      /*
      console.log("In deleting stage");
      runInAction(() => {
        if (this.mapChildImageListAll.length > 0) {
          for (let i = this.mapChildImageListAll.length; i > 0; i--) {
            if (this.mapChildImageListAll[i].parentID === parentID) {
              this.mapChildImageListAll.splice(i, 1);
            }
          }
        }
      });*/
    } catch (error) {
      console.log(error);
    }
  };
  saveMapPosition = async (mapChildPosition: IMapListWithPosition) => {
    try {
      let mapPosition: MapPosition = {
        mapPositionID: 0,
        mapListID: mapChildPosition.mapListID,
        x: Math.floor(mapChildPosition.Position.x),
        y: Math.floor(mapChildPosition.Position.y),
        width: Math.floor(mapChildPosition.dimension.width),
        height: Math.floor(mapChildPosition.dimension.height),
        savedStatusID: 2,
        rotation: mapChildPosition.rotation,
        userID: store.userStore.user!.id.toString(),
        systemIP: "",
        systemName: "",
      };
      const r = await agent.SystemInformation.getSystemIP();
      const d = await r.json();
      runInAction(async () => {
        mapPosition.systemIP = d.IPv4;
        mapPosition.systemName = d.country_code;
        await agent.Map.saveMapPosition(mapPosition);
      });
    } catch (error) {
      console.log(error);
    }
  };

  setPositionOfTheObject = async (mapListID: number, { x, y }: any) => {
    let result = this.mapChildImageList.filter(
      (element) => element.mapListID === mapListID
    );
    runInAction(() => {
      result[0].Position = { x, y };
    });
  };
  setRotationOfTheObject = async (mapListID: number) => {
    let result = this.mapChildImageList.filter(
      (element) => element.mapListID === mapListID
    );
    runInAction(() => {
      let newRotation = result[0].rotation + 45;
      if (newRotation >= 360) {
        newRotation = -360;
      }
      result[0].rotation = newRotation;
    });
  };

  getMapCameraRotation = async (parentID: number) => {
    this.isloadingPopup = true;
    try {
      var resultOFRotation = await agent.Map.getMapPosition(parentID);
      runInAction(() => {
        this.isloadingPopup = false;
      });
      return resultOFRotation;
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.isloadingPopup = false;
      });
    }
  };
  getChildCamera = async (parentID: number) => {
    this.isloadingMapFloor = true;
    this.mapChildImageList.splice(0, this.mapChildImageList.length);
    // let resultAlreadyExist = this.mapChildImageListAll.filter(
    //   (x) => x.parentID === parentID
    // );
    // if (resultAlreadyExist.length > 0) {
    //   resultAlreadyExist.forEach((element) => {
    //     this.mapChildImageList.push({
    //       mapListID: element.mapListID,
    //       parentID: parentID,
    //       mapName: element.mapName,
    //       Position: {
    //         x: element.Position.x,
    //         y: element.Position.y,
    //       },
    //     });
    //   });
    //   return this.mapChildImageList;
    // }
    try {
      let returnValue = this.savedMapList.filter(
        (a) => a.parentID === parentID && a.mapCategoriesID === 2
      );
      for (const element of returnValue) {
        await agent.Map.getMapPosition(element.mapListID).then((resp) => {
          runInAction(() => {
            let x = resp.x === undefined ? 0 : resp.x;
            let y = resp.y === undefined ? 0 : resp.y;
            this.mapChildImageList.push({
              mapListID: element.mapListID,
              parentID: parentID,
              mapName: element.mapListName,
              Position: { x: x, y: y },
              rotation: resp.rotation,
              dimension: {
                height:
                  resp.height === 0 ? this.defaultWidthheight : resp.height,
                width: resp.width === 0 ? this.defaultWidthheight : resp.width,
              },
              bound: { top: 0, bottom: 0 },
            });
            this.mapChildImageListAll.push({
              mapListID: element.mapListID,
              parentID: parentID,
              mapName: element.mapListName,
              Position: { x: x, y: y },
              rotation: 0,
              dimension: {
                height:
                  resp.height === 0 ? this.defaultWidthheight : resp.height,
                width: resp.width === 0 ? this.defaultWidthheight : resp.width,
              },
              bound: { top: 0, bottom: 0 },
            });
          });
        });
      }
      runInAction(() => {
        this.isloadingMapFloor = false;
      });
      return this.mapChildImageList;
    } catch (error) {
      console.log("getChildCamera," + error);
      runInAction(() => {
        this.isloadingMapFloor = false;
      });
      return this.mapChildImageList;
    }
  };

  getMapURL = async (key: string) => {
    this.isloadingMapFloor = true;
    let link = process.env.REACT_APP_IP_Addr;
    link = link?.replace("video", "Map");
    let URl = "";
    try {
      let result = this.savedMapList.find((element) => {
        return element.mapListID === parseInt(key);
      });
      if (result?.imageSrc === null) URl = this.defaultImageURL;
      else URl = link + result?.imageSrc;
      runInAction(() => {
        this.isloadingMapFloor = false;
        this.serverAPILink = link;
      });

      return URl;
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.isloadingMapFloor = false;
      });
      return URl;
    }
  };

  loadTreeView = async (userID: string) => {
    this.isloadingInitial = true;
    try {
      this.savedMapList = await agent.Map.getMapData(userID);
      runInAction(() => {
        this.treeView.splice(0, this.treeView.length);
        let t: TreeNodeInArray;
        this.savedMapList.forEach((value) => {
          if (value.parentID === 0) {
            t = {
              index: value.mapListID,
              key: value.mapListID.toString(),
              label: value.mapListName,
              nodes: [],
            };
            this.recursiveLoop(t, this.savedMapList, value.mapListID);
            this.treeView.push(t);
          }
        });
        this.isRefreshRequired = false;
        this.isloadingInitial = false;
      });
      return this.treeView;
    } catch (err) {
      runInAction(() => {
        this.isRefreshRequired = false;
        this.isloadingInitial = false;
      });
    }
  };
  recursiveLoop = (
    resulttreeview: TreeNodeInArray,
    resultMapList: MapList[],
    MapListParentID: number
  ) => {
    resultMapList.forEach((value) => {
      if (value.parentID === MapListParentID) {
        resulttreeview.nodes?.push({
          index: value.mapListID,
          key: value.mapListID.toString(),
          label: value.mapListName,
          nodes: [],
        });
        this.recursiveLoop(
          resulttreeview.nodes?.[resulttreeview.nodes?.length - 1]!,
          resultMapList,
          value.mapListID
        );
      }
    });
  };
  updateRecord = async (formdata: FormData) => {
    this.isloading = true;
    const r = await agent.SystemInformation.getSystemIP();
    const d = await r.json();
    try {
      runInAction(async () => {
        formdata.append("mapList.systemIP", d.IPv4);
        formdata.append("mapList.systemName", d.country_code);

        await agent.Map.updateMapRecord(formdata);
        runInAction(() => {
          this.isloading = false;
          store.modalStore.closeModal();
          this.isRefreshRequired = true;
        });
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.isloading = false;
        this.isRefreshRequired = true;
      });
    }
  };
  saveRecord = async (formdata: FormData) => {
    this.isloading = true;
    const r = await agent.SystemInformation.getSystemIP();
    const d = await r.json();

    try {
      runInAction(async () => {
        formdata.append("mapList.systemIP", d.IPv4);
        formdata.append("mapList.systemName", d.country_code);
        await agent.Map.saveMapData(formdata);
        runInAction(() => {
          this.isloading = false;
          store.modalStore.closeModal();
          this.isRefreshRequired = true;
        });
      });
    } catch (e) {
      console.log(e);
      runInAction(() => {
        this.isloading = false;
        this.isRefreshRequired = true;
      });
    }
  };

  loadMapRecord = async (mapListID: number) => {
    this.isloadingPopup = true;
    try {
      let result = await agent.Map.getMapRecord(mapListID);
      runInAction(() => {
        result.cameraIP = result.cameraIP == null ? "" : result.cameraIP;

        this.isloadingPopup = false;
      });
      return result;
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.isloadingPopup = false;
      });
    }
  };

  deleteMapRecord = async (mapListID: number, userID: string) => {
    this.isloadingInitial = true;
    const r = await agent.SystemInformation.getSystemIP();
    const d = await r.json();

    try {
      let parameterForDelete: ParameterForDelete = {
        transactionID: mapListID.toString(),
        userIP: d.IPv4,
        userSystem: d.country_code,
        userID: userID,
      };
      await agent.Map.deleteMapRecord(parameterForDelete);
      runInAction(() => {
        this.isloadingInitial = false;
        this.isRefreshRequired = true;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.isloadingInitial = false;
      });
    }
  };

  loadMapCategories = async () => {
    //    this.isloadingInitial = true;
    this.isloadingPopup = true;
    this.MapCategories.splice(0, this.MapCategories.length);
    try {
      const getMapCategories = await agent.Map.getMapCategories();
      runInAction(() => {
        getMapCategories.forEach((MapCateg) => {
          let obj: IMapCategories = {
            text: MapCateg.mapCategoriesName,
            value: MapCateg.mapCategoriesID,
          };
          this.MapCategories.push(obj);
        });
        this.isloadingPopup = false;
        //        this.isloadingInitial = false;
        return this.MapCategories;
      });
    } catch (e) {
      console.log(e);
      runInAction(() => {
        this.isloadingPopup = false;
        //        this.isloadingInitial = false;
      });
    }
  };
}
