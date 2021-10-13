import { makeAutoObservable, runInAction } from "mobx";
import { history } from "../..";
import agent from "../api/agent";
import { User, UserFormValues } from "../Models/user";
import { store } from "./store";

export default class UserStore {
  user: User | null = null;
  constructor() {
    makeAutoObservable(this);
  }
  get isLoggedIn() {
    return !!this.user;
  }
  login = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.login(creds);
      store.commonStore.setToken(user.token);
      runInAction(() => (this.user = user));
      // if (this.user?.userViewID === 0) {
      //   alert("Please select the View for the Dashboard");
      //   history.push(`/profile/${user.id}`);
      // } else if (this.user?.userViewID === 3) {
      //   history.push("/map");
      // } else history.push("/dashboard");
      history.push("/redirect");
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  };
  logout = () => {
    store.commonStore.setToken(null);
    window.localStorage.removeItem("jwt");
    this.user = null;
    history.push("/");
  };
  getUser = async () => {
    try {
      const user = await agent.Account.current();
      runInAction(() => {
        this.user = user;
      });
    } catch (error) {
      console.log(error);
    }
  };
  register = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.register(creds);
      store.commonStore.setToken(user.token);
      runInAction(() => (this.user = user));
      history.push("/dashboard");
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  };
}
