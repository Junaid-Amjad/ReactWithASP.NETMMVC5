import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";


export default class SearchFile {

    constructor() {
        makeAutoObservable(this);
    }

    getSearchFilterFiles = async () => {
        try {
            const XMLFileOfTheFile = await agent.SearchFiles.getXMLFileofCamera();
            runInAction(() => {

            })

        } catch (Error) {
            console.log(Error);
        }
    }


}