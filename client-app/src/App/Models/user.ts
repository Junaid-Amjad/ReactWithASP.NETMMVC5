export interface User {
  userName: string;
  displayName: string;
  token: string;
  image?: string;
  id: string;
  userViewID: number;
}

export interface UserFormValues {
  email: string;
  password: string;
  displayname?: string;
  username?: string;
}
