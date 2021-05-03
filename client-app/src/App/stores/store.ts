import { createContext, useContext } from "react";
import ActivityStore from "./activityStore";
import Commonstore from "./commonStore";

interface Store{
    activityStore:ActivityStore;
    commonStore:Commonstore;
}

export const store:Store={
    activityStore: new ActivityStore(),
    commonStore: new Commonstore()
}

export const StoreContext = createContext(store);

export function useStore(){
    return useContext(StoreContext);
}
