import { createContext, useContext } from "react";
import ActivityStore from "./activityStore";
import CameraViewStore from "./cameraViewStore";
import Commonstore from "./commonStore";
import ModalStore from "./modalStore";
import SavedFileStore from "./savedFileStore";
import UserStore from "./userStore";

interface Store{
    activityStore:ActivityStore;
    commonStore:Commonstore;
    userStore:UserStore;
    modalStore:ModalStore;
    savedFileStore:SavedFileStore;
    cameraViewStore:CameraViewStore;
}

export const store:Store={
    activityStore: new ActivityStore(),
    commonStore: new Commonstore(),
    userStore: new UserStore(),
    modalStore: new ModalStore(),
    savedFileStore: new SavedFileStore(),
    cameraViewStore: new CameraViewStore()
}

export const StoreContext = createContext(store);

export function useStore(){
    return useContext(StoreContext);
}
