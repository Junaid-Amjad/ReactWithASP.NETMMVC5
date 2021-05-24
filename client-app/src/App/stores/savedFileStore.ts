import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import {  SavedFile } from "../Models/savedFile";


export default class SavedFileStore{
    loadingInitial= false;
    savedRegistry = new Map<string,SavedFile>();
    DirectoryLink:string[] = [];
    APIfirstParameter = "files";

    constructor(){
        makeAutoObservable(this);
    }

    get SavedRegistrySort(){
        return Array.from(this.savedRegistry.values()).sort();
    }

    get getLoadedSavedFiles(){
        return this.SavedRegistrySort;  
    }
    /*
    get getLoadedDirectoryFiles(){
        return Array.from(this.DirectoryLink);    
    }*/

    loadSavedFiles = async () => {
        this.loadingInitial=true; 
        this.savedRegistry.clear(); 
        try{
            const savedFiles = await agent.SavedFiles.getFileNames();
            runInAction(() => {

            this.validateRow(this.APIfirstParameter);
            this.DirectoryLink.push(this.APIfirstParameter);            
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

    validateRow =async(PathName:string) =>{

        var startingRow = 0;
        var found = false;
        var EndingRow = this.DirectoryLink.length;
        for(var row=0;row<EndingRow;row++){
            if(this.DirectoryLink[row] === PathName){
                startingRow=row;
                found=true;
            }
        }
        if(found){
            this.DirectoryLink.splice(startingRow,EndingRow);
        }
    }

    loadDirectoryFiles = async (PathName: string) => {

        this.loadingInitial=true;
        this.savedRegistry.clear(); 
        try{
            const savedDirectory = await agent.SavedFiles.getDirectoryDetail(PathName);
            runInAction(() => {
            this.DirectoryLink.splice(0,1);
            this.DirectoryLink.unshift(this.APIfirstParameter);            
            const PathNameSplit = PathName.split('/');
            PathNameSplit.forEach((object,i) => {
                this.validateRow(object);
                this.DirectoryLink.push(object);
            })            

                savedDirectory.forEach(savedDirectory =>{
                    this.setActivity(savedDirectory);
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