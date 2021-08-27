import { Container } from "semantic-ui-react";
import NavBar from "./Navbar";
//import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import { observer } from "mobx-react-lite";
import { Route, Switch } from "react-router";
import HomePage from "../../features/Home/HomePage";
//import ActivityForm from "../../features/activities/form/ActivityForm";
//import ActivityDetails from "../../features/activities/details/ActivityDetail";
import TestErrors from "../../features/errors/TestError";
import { ToastContainer } from "react-toastify";
import NotFound from "../../features/errors/NotFound";
import ServerError from "../../features/errors/ServerError";
import { useStore } from "../stores/store";
import { useEffect } from "react";
import LoadingComponent from "./LoadingComponents";
import ModalContainer from "../../App/common/modals/ModalContainer";
import SavedFile from "../../features/filefolder/SavedFile";
import PrivateRoute from "./PrivateRoute";
import CameraView from "../../features/cameraView/CameraView";
import SearchForm from "../../features/Searching/SearchForm";
import DashBoard from "../../features/Dashboard/DashBoard";
import CreatingGrid from "../../features/Grid/CreatingGrid";

function App() {
  //const location = useLocation();
  const { commonStore, userStore } = useStore();

  useEffect(() => {
    if (commonStore.token) {
      userStore.getUser().finally(() => commonStore.setAppLoaded());
    } else {
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore]);

  if (!commonStore.appLoaded)
    return <LoadingComponent content="Loading app...." />;
  return (
    <>
      <ToastContainer position="bottom-right" hideProgressBar />
      <ModalContainer />
      <Route exact path="/" component={HomePage} />
      <Route
        path={"/(.+)"}
        render={() => (
          <>
            <NavBar />
            <Container style={{ marginTop: "1em" }}>
              <Switch>
                <PrivateRoute path="/dashboard" component={DashBoard} />
                {/*<PrivateRoute exact path='/activities' component={ActivityDashboard} />*/}
                {/*<PrivateRoute
                  path="/activities/:id"
                  component={ActivityDetails}
                />*/}
                {/*<PrivateRoute
                  key={location.key}
                  path={["/createActivity", "/manage/:id"]}
                  component={ActivityForm}
                />*/}
                <Route path="/errors" component={TestErrors} />
                {/* <PrivateRoute
                  path={["/creategrid", "/creategrid/:id"]}
                  component={CreatingGrid}
                /> */}
                <PrivateRoute
                  exact
                  path="/creategrid"
                  component={CreatingGrid}
                />
                <PrivateRoute path="/creategrid/:id" component={CreatingGrid} />

                <PrivateRoute exact path="/files" component={SavedFile} />
                <PrivateRoute path="/files/:pathname+" component={SavedFile} />
                <PrivateRoute
                  exact
                  path="/cameraView/:id"
                  component={CameraView}
                />
                <PrivateRoute exact path="/Search" component={SearchForm} />
                <Route path="/server-error" component={ServerError} />
                {/* <PrivateRoute path="/creategrid" component={CreatingGrid} /> */}
                <Route component={NotFound} />
              </Switch>
            </Container>
          </>
        )}
      />
    </>
  );
}

export default observer(App);
