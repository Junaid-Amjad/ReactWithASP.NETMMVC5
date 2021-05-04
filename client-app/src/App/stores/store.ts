import { createContext, useContext } from "react";
import ActivityStore from "./activityStore";
import Commonstore from "./commonStore";
import ModalStore from "./modalStore";
import UserStore from "./userStore";

interface Store{
    activityStore:ActivityStore;
    commonStore:Commonstore;
    userStore:UserStore;
    modalStore:ModalStore;
}

export const store:Store={
    activityStore: new ActivityStore(),
    commonStore: new Commonstore(),
    userStore: new UserStore(),
    modalStore: new ModalStore()
}

export const StoreContext = createContext(store);

export function useStore(){
    return useContext(StoreContext);
}
