import { observer } from "mobx-react-lite";
import { useStore } from "../../App/stores/store";
import { history } from "../..";
import { useEffect } from "react";

export default observer(function RedirectingPage() {
  const { userStore } = useStore();

  const min = 3;
  const max = 5;
  const random = Math.floor(Math.random() * (max - min + 1)) + min;
  let userViewID = userStore.user!.userViewID;
  if (userViewID === undefined || userViewID === 1) {
    userViewID = random;
  }
  useEffect(() => {
    if (userStore.isLoggedIn) {
      if (userViewID === 0) {
        alert("Please select the View for the Dashboard");
        history.push(`/profile/${userStore.user!.id}`);
      } else if (userViewID === 3) {
        history.push("/map");
      } else if (userViewID === 4) {
        history.push("/dashboard");
      } else if (userViewID === 5) {
        history.push("/Search");
      }
    }
  }, [userStore.isLoggedIn, userStore.user, userViewID]);

  return <div>Redirecting!!!</div>;
});
