import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { CameraView } from "../Models/cameraView";


export default class CameraViewStore {
    loadingInitial = false;
    browserLinkChanged = false;
    //    loadingToDeleteFile=false;
    cameraViewRegistry = new Map<string, CameraView>();
    constructor() {
        makeAutoObservable(this);
    }


    get getCameraViewResult() {
        return Array.from(this.cameraViewRegistry.values()).sort();
    }
    deleteAllFiles = async () => {
        //      this.loadingToDeleteFile=true;
        try {
            this.browserLinkChanged = true;
            const filedeleted = await agent.StreamIP.deleteFiles("0");
            runInAction(async () => {
                if (filedeleted) {
                    this.cameraViewRegistry.forEach(async (value, key) => {
                        const camerafile = this.convertStringToAscii(value.FilePath);
                        const tokenCanceled = await agent.StreamIP.canceltoken(camerafile);
                        runInAction(() => {
                            if (tokenCanceled) {
                                //                                this.loadingToDeleteFile=false;
                            }

                        })

                    })

                }
            })
        }
        catch (error) {
            console.log(error);
            runInAction(() => {
                //             this.loadingToDeleteFile=false;
            })
        }
    }


    getIndividualIPStream = async (UUID: string) => {
        let resultfromCameraObject = this.cameraViewRegistry.get(UUID);
        console.log(resultfromCameraObject?.FilePath);
        if (resultfromCameraObject?.isProcessed) {
            const filedeleted = await agent.StreamIP.deleteFiles(resultfromCameraObject.FilePath);
            runInAction(async () => {
                if (filedeleted) {
                    resultfromCameraObject!.isProcessed = false;
                    await this.callTheFileForProcessing(resultfromCameraObject!);
                }
            })
        }
        else {
            resultfromCameraObject!.isProcessed = undefined;
            await this.callTheFileForProcessing(resultfromCameraObject!);
        }

    }

    loadCameraView = async () => {

        this.loadingInitial = true;
        let isRecordFound = false;

        try {
            this.cameraViewRegistry.clear();
            await this.deleteAllFiles();
            const camerav = await agent.cameraView.getLiveVideoUrl();
            runInAction(async () => {
                this.browserLinkChanged = false;
                camerav.forEach(cameraview => {
                    this.setCameraResult(cameraview);
                    isRecordFound = true;
                    if (isRecordFound) {
                        this.getCameraViewResult.forEach(async (value, key) => {
                            await this.callTheFileForProcessing(value);

                        })
                    }
                    this.loadingInitial = false;

                })
            })
        }
        catch (error) {
            console.log(error);
            runInAction(() => {
                this.loadingInitial = false;
            })
        }
    }
    private uuidv4() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r && 0x3 | 0x8);
            return v.toString(16);
        });
    }

    private setCameraResult = (cameraView: CameraView) => {
        cameraView.FileName = this.uuidv4() + '.m3u8';
        cameraView.FilePath = this.uuidv4() + '/' + cameraView.FileName;
        this.cameraViewRegistry.set(cameraView.guid.toString(), cameraView);
    }

    convertStringToAscii(value: string) {
        let u = "";
        for (var i = 0; i < value.length; i++) {
            u += value.charCodeAt(i) + '-';
        }
        return u;
    }
    async callTheFileForProcessing(cameraView: CameraView) {
        try {
            if (!cameraView.isProcessed) {
                const cameraurl = this.convertStringToAscii(cameraView.url);
                const camerafile = this.convertStringToAscii(cameraView.FilePath);

                cameraView.IPAddressPath = process.env.REACT_APP_IP_Addr;
                const result = await agent.StreamIP.setStreamOfCamera(camerafile, cameraurl);
                runInAction(async () => {
                    cameraView.outputFolder = result.fileServerAddress;
                    console.log(cameraView.url + " " + result.iPfound);
                    if (result.iPfound) {
                        cameraView.isProcessed = true;
                        const fileresult = await agent.StreamIP.getFileExist(camerafile);
                        runInAction(() => {
                            console.log(cameraView.IPAddressPath + "  " + cameraView.FilePath)
                            if (fileresult) {
                                cameraView.isFileProcessed = true;
                            }

                            if (this.browserLinkChanged) {
                                this.deleteAllFiles()
                            }
                        })
                    }
                    else {
                        cameraView.isProcessed = false;
                        cameraView.isFileProcessed = false;
                    }
                })
            }
        }
        catch (error) {
            console.log(error);
            runInAction(() => {


            })
        }

    }
}

