import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import ReactPlayer from "react-player";
import { useParams } from "react-router-dom";
import {
  Button,
  Grid,
  Label,
  Loader,
  Message,
  Segment,
} from "semantic-ui-react";
import LoadingComponent from "../../App/Layout/LoadingComponents";
import { useStore } from "../../App/stores/store";

export default observer(function CameraView() {
  const { cameraViewStore } = useStore();
  let { id } = useParams<{ id: string }>();
  const {
    getCameraViewResult,
    loadCameraView,
    getIndividualIPStream,
    noofcolumns,
    layoutName,
    isReloading,
  } = cameraViewStore;
  useEffect(() => {
    loadCameraView(id);
  }, [loadCameraView, id]);

  function handleRelod(e: any) {
    getIndividualIPStream(e.target.value);
  }
  if (cameraViewStore.loadingInitial)
    return <LoadingComponent content="Loading Video Camera" />;

  return (
    <>
      <Segment>
        <Label attached="top" size="huge" color="black">
          {layoutName}
        </Label>
        <Grid
          columns={
            noofcolumns === 1
              ? 1
              : noofcolumns === 2
              ? 2
              : noofcolumns === 3
              ? 3
              : noofcolumns === 4
              ? 4
              : noofcolumns === 5
              ? 5
              : noofcolumns === 6
              ? 6
              : noofcolumns === 7
              ? 7
              : noofcolumns === 8
              ? 8
              : noofcolumns === 9
              ? 9
              : noofcolumns === 10
              ? 10
              : noofcolumns === 11
              ? 11
              : noofcolumns === 12
              ? 12
              : noofcolumns === 13
              ? 13
              : noofcolumns === 14
              ? 14
              : noofcolumns === 15
              ? 15
              : noofcolumns === 16
              ? 16
              : 16
          }
          stackable
        >
          {getCameraViewResult.map((value, key) =>
            value.isProcessed === false ? (
              isReloading ? (
                <Grid.Column key={value.guid}>
                  <LoadingComponent content="Reloading Again" />
                </Grid.Column>
              ) : (
                <Grid.Column key={value.guid}>
                  <Message info key={value.guid}>
                    <Message.Header key={value.guid}>
                      IP Camera {value.url} is Un-Reachable
                    </Message.Header>
                    <p>
                      IP Camera is not accessible. Click the Button to Load the
                      Component
                    </p>
                    <Button
                      basic
                      color="red"
                      value={value.guid}
                      onClick={handleRelod}
                    >
                      Reload
                    </Button>
                  </Message>
                </Grid.Column>
              )
            ) : value.isFileProcessed ? (
              <Grid.Column key={value.guid}>
                <ReactPlayer
                  className="react-player"
                  key={value.guid}
                  // This is the video address passed from the superior page
                  url={
                    value.IPAddressPath?.replaceAll("video", "TempFiles") +
                    value.FilePath
                  }
                  width="100%"
                  controls
                  playing
                  config={{
                    file: {
                      forceHLS: true,
                    },
                  }}
                />
              </Grid.Column>
            ) : (
              <Loader key={value.guid} active inline="centered">
                Fetching Video Result
              </Loader>
            )
          )}
        </Grid>
      </Segment>
    </>
  );
});
