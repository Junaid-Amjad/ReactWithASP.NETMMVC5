import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import {  SavedFile } from "../Models/savedFile";


export default class SavedFileStore{
    loadingInitial= false;
    savedRegistry = new Map<string,SavedFile>();

//    savedRegistry:string[] = [];
    constructor(){
        makeAutoObservable(this);
    }

    get SavedRegistrySort(){
        return Array.from(this.savedRegistry.values()).sort();
    }

    get getLoadedSavedFiles(){
        return this.SavedRegistrySort;    
    }

    loadSavedFiles = async () => {
        this.loadingInitial=true;                
        try{
            const savedFiles = await agent.SavedFiles.getFileNames();
            runInAction(() => {
                savedFiles.forEach(savedFile =>{
                    this.setActivity(savedFile);
                })
                this.loadingInitial=false;    
            })
        }
        catch(error){
            console.log(error);
            runInAction(() => {
                this.loadingInitial=false;
            })

        }
    }
    private setActivity = (savedFile:SavedFile) =>{
        this.savedRegistry.set(savedFile.path,savedFile);        
    }
 
}