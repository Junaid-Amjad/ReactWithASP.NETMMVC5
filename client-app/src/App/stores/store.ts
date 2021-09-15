import { createContext, useContext } from "react";
import ActivityStore from "./activityStore";
import CameraViewStore from "./cameraViewStore";
import Commonstore from "./commonStore";
import GridLayoutStore from "./gridLayoutStore";
import ModalStore from "./modalStore";
import ProfileStore from "./Profile/profileStore";
import SavedFileStore from "./savedFileStore";
import SearchFile from "./searchfile";
import UserStore from "./userStore";

interface Store {
  activityStore: ActivityStore;
  commonStore: Commonstore;
  userStore: UserStore;
  modalStore: ModalStore;
  savedFileStore: SavedFileStore;
  cameraViewStore: CameraViewStore;
  searchFileStore: SearchFile;
  gridLayoutStore: GridLayoutStore;
  profileStore: ProfileStore;
}

export const store: Store = {
  activityStore: new ActivityStore(),
  commonStore: new Commonstore(),
  userStore: new UserStore(),
  modalStore: new ModalStore(),
  savedFileStore: new SavedFileStore(),
  cameraViewStore: new CameraViewStore(),
  searchFileStore: new SearchFile(),
  gridLayoutStore: new GridLayoutStore(),
  profileStore: new ProfileStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
