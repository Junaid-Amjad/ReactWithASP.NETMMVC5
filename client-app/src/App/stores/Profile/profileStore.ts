import { makeAutoObservable, runInAction } from "mobx";
import agent from "../../api/agent";
import { userProfile } from "../../Models/Profile/userProfile";

interface IUserViews {
  text: string;
  value: number;
}

export default class ProfileStore {
  isloadingInitial = false;
  userViews: IUserViews[] = [];
  loading = false;
  constructor() {
    makeAutoObservable(this);
  }

  updateUserData = async (userData: userProfile) => {
    this.isloadingInitial = true;
    const r = await agent.SystemInformation.getSystemIP();
    const d = await r.json();
    try {
      runInAction(async () => {
        userData.userIP = d.IPv4;
        userData.userSystem = d.country_code;
        await agent.Profile.updateUserProfile(userData.id, userData);
        runInAction(() => {
          this.isloadingInitial = false;
          return true;
        });
      });
    } catch (e) {
      console.log(e);
      this.isloadingInitial = false;
      return false;
    }
  };

  loadUserDetails = async (UserID: string) => {
    this.isloadingInitial = true;
    try {
      var result = await agent.Profile.getUserProfile(UserID);
      runInAction(async () => {
        this.isloadingInitial = false;
      });
      return result;
    } catch (error) {
      console.log(error);
      this.isloadingInitial = false;
    }
  };

  loadUserView = async () => {
    this.isloadingInitial = true;
    try {
      const getUserView = await agent.Profile.getUserViews();
      runInAction(() => {
        getUserView.forEach((userView) => {
          let obj: IUserViews = {
            text: userView.userViewName,
            value: userView.userViewID,
          };
          this.userViews.push(obj);
        });
        this.isloadingInitial = false;
      });
    } catch (e) {
      console.log(e);
      runInAction(() => {
        this.isloadingInitial = false;
      });
    }
  };
}
