import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ISearchFile } from "../Models/searchFile";

interface SearchForminterface {
  DateFrom: Date | null;
  TimeFrom: Date | null;
  DateTo: Date | null;
  TimeTo: Date | null;
  OrderByID: Number;
  StartTickDate: Number;
  EndTickDate: Number;
}

export default class SearchFile {
  loadingSearchingFile = false;
  isLoadingContent = false;
  isRefreshArray = false;
  savedFileSearchedRegistry: ISearchFile[] = [];

  constructor() {
    makeAutoObservable(this);
  }

  refreshvalue = () => {
    runInAction(() => {
      this.isLoadingContent = false;
    });
  };

  setIsRefreshvalue = () => {
    runInAction(() => {
      this.isRefreshArray = false;
    });
  };

  getSearchFilterFiles = async (
    searchingformparameter: SearchForminterface
  ) => {
    try {
      this.loadingSearchingFile = true;
      this.savedFileSearchedRegistry.splice(
        0,
        this.savedFileSearchedRegistry.length
      );
      let stringqueryparameter =
        searchingformparameter.StartTickDate +
        "," +
        searchingformparameter.EndTickDate +
        "," +
        searchingformparameter.OrderByID;
      const XMLFileOfTheFile = await agent.SearchFiles.getXMLFileofCamera(
        stringqueryparameter
      );
      runInAction(() => {
        XMLFileOfTheFile.forEach((object, i) => {
          this.setSearchedFileintheobject(object);
        });
        this.loadingSearchingFile = false;
        this.isRefreshArray = true;
        if (XMLFileOfTheFile.length > 0) this.isLoadingContent = true;
        else this.isLoadingContent = false;
      });
    } catch (Error) {
      console.log(Error);
      this.loadingSearchingFile = false;
    }
  };
  setSearchedFileintheobject = (searchFile: ISearchFile) => {
    this.savedFileSearchedRegistry.push(searchFile);
  };
}
