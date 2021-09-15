import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { history } from "../..";
import { GridLayoutMaster } from "../apiClass/GridLayout/GridLayoutMaster";
import { GridLayoutDto } from "../DTO/GridLayoutDto";
import { Activity } from "../Models/activity";
import { CameraIPResult } from "../Models/cameraIPResult";
import { CameraView } from "../Models/cameraView";
import { globalMessage } from "../Models/globalMessage";
import { ICameraSettingList } from "../Models/gridLayout";
import { userProfile } from "../Models/Profile/userProfile";
import { userView } from "../Models/Profile/userView";
import { SavedFile } from "../Models/savedFile";
import { ISearchFile } from "../Models/searchFile";
import { User, UserFormValues } from "../Models/user";
import { store } from "../stores/store";

const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
};

axios.defaults.baseURL = process.env.REACT_APP_API_URL;

axios.interceptors.request.use((config) => {
  const token = store.commonStore.token;
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

axios.interceptors.response.use(
  async (response) => {
    if (process.env.NODE_ENV === "development") await sleep(1000);
    return response;
  },
  (error: AxiosError) => {
    const { data, status, config } = error.response!;
    switch (status) {
      case 400:
        if (typeof data === "string") {
          toast.error("Bad Request");
        }
        if (config.method === "get" && data.errors.hasOwnProperty("id")) {
          history.push("/not-found");
        }
        if (data.errors) {
          const modalStateErrors = [];
          for (const key in data.errors) {
            if (data.errors[key]) {
              modalStateErrors.push(data.errors[key]);
            }
          }
          throw modalStateErrors.flat();
        }

        break;
      case 401:
        toast.error("unauthorized");
        break;
      case 404:
        history.push("/not-found");
        break;
      case 500:
        store.commonStore.setServerError(data);
        history.push("/server-error");
        break;
    }
    return Promise.reject(error);
  }
);

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) => axios.post<T>(url, {}).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, {}).then(responseBody),
  del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Activities = {
  list: () => requests.get<Activity[]>("/activities"),
  details: (id: string) => requests.get<Activity>(`/activities/${id}`),
  create: (activity: Activity) => axios.post<void>(`/activities`, activity), //requests.post<void>(`/activities`,activity),
  update: (activity: Activity) =>
    axios.put<void>(`/activities/${activity.activityID}`, activity),
  delete: (id: string) => axios.delete<void>(`/activities/${id}`),
};

const Account = {
  current: () => requests.get<User>("/Account"),
  login: (user: UserFormValues) =>
    axios.post<User>("/Account/login", user).then(responseBody),
  register: (user: UserFormValues) =>
    axios.post<User>("/Account/register", user).then(responseBody),
};

const SavedFiles = {
  getFileNames: () => requests.get<SavedFile[]>("/SavedFile"),
  getDirectoryDetail: (Pathname: string) =>
    requests.get<SavedFile[]>(`/SavedFile/${Pathname}`),
};

const SystemInformation = {
  getSystemIP: () => fetch("https://geolocation-db.com/json/"),
};

const Profile = {
  getUserViews: () => requests.get<userView[]>("/Profile/GetUserViews"),
  getUserProfile: (userID: string) =>
    requests.get<userProfile>(`Account/GetUserData/${userID}`),
  getUserViewsByID: (viewID: number) =>
    requests.get<string>(`/Profile/GetUserViewById/${viewID}`),
  updateUserProfile: (guid: string, userProfile: userProfile) =>
    axios.put<userProfile>(`/Account/UpdateUser/${guid}`, userProfile),
};

const cameraView = {
  getLiveVideoUrl: () => requests.get<CameraView[]>("/CameraView"),
  getLiveVideUrlFromDB: (keyid: string) =>
    requests.get<CameraView[]>(`/CameraView/${keyid}`),
};

const StreamIP = {
  setStreamOfCamera: (FilePath: string, URL: string) =>
    requests.get<CameraIPResult>(`/StreamFFMPEG/${URL}/${FilePath}`),
  getFileExist: (FilePath: string) =>
    requests.get<globalMessage>(`/StreamFFMPEG/${FilePath}`),
  deleteFiles: (FilePath: string) =>
    requests.get<globalMessage>(`/StreamFFMPEG/deleteFile/${FilePath}`),
  canceltoken: (FilePath: string) =>
    requests.get<boolean>(`/Stream/CancelToken/${FilePath}`),
};

const SearchFiles = {
  getXMLFileofCamera: (stringobjectparameters: string) =>
    requests.get<ISearchFile[]>(
      `/SearchFile/GetXMLListOFCamera/${stringobjectparameters}`
    ),
};

const gridLayout = {
  getresultofcamera: () => requests.get<ICameraSettingList[]>(`/GridLayout`),
  saveGridLayout: (gridLayout: GridLayoutDto) =>
    axios.post<void>("/GridLayout", gridLayout),
  getgridLayoutMaster: () =>
    requests.get<GridLayoutMaster[]>(`/GridLayout/getGridLayoutMaster`),
  deleteGridLayout: (gridLayoutMasterID: number) =>
    axios.delete<void>(`/GridLayout/${gridLayoutMasterID}`),
  loadGridLayoutDetail: (gridLayoutMasterID: number) =>
    requests.get<GridLayoutDto>(`/GridLayout/${gridLayoutMasterID}`),
  updateGridLayout: (
    gridLayoutMasterID: number,
    gridLayoutDto: GridLayoutDto
  ) => axios.put<void>(`/GridLayout/${gridLayoutMasterID}`, gridLayoutDto),
};

const agent = {
  Activities,
  Account,
  SavedFiles,
  cameraView,
  StreamIP,
  SearchFiles,
  gridLayout,
  Profile,
  SystemInformation,
};

export default agent;
