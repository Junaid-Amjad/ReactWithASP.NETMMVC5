import { observer } from "mobx-react-lite"
import React, { useEffect } from "react";
import ReactPlayer from "react-player";
import { Button, Grid, Loader, Message, Segment } from "semantic-ui-react";
import LoadingComponent from "../../App/Layout/LoadingComponents";
import { useStore } from "../../App/stores/store";


export default observer(function CameraView() {


  const { cameraViewStore } = useStore();

  const { getCameraViewResult, loadCameraView, getIndividualIPStream } = cameraViewStore;
  useEffect(() => {
    //      if(getCameraViewResult.length<=0){

    loadCameraView();

    //    }

  }, [loadCameraView])
  if (cameraViewStore.loadingInitial) return <LoadingComponent content="Loading Video Camera" />



  function handleRelod(e: any) {
    getIndividualIPStream(e.target.value);
  }

  return (
    <>
      <Segment>
        <Grid columns={2} stackable>
          {getCameraViewResult.map((value, key) => (

            value.isProcessed === false ?
              <Grid.Column key={value.guid}>
                <Message info key={value.guid}>
                  <Message.Header key={value.guid}>IP Camera {value.url} is Un-Reachable</Message.Header>
                  <p>IP Camera is not accessible. Click the Button to Load the Component</p>
                  <Button basic color='red' value={value.guid} onClick={handleRelod}>Reload</Button>
                </Message>
              </Grid.Column>
              :
              value.isFileProcessed ?
                < Grid.Column key={value.guid} >
                  <ReactPlayer className='react-player' key={value.guid}
                    // This is the video address passed from the superior page
                    url={value.IPAddressPath?.replaceAll('video', 'TempFiles') + value.FilePath}
                    width='100%'
                    controls
                    playing
                    config={{
                      file: {
                        forceHLS: true,
                      }
                    }}
                  />
                </Grid.Column>
                : <Loader key={value.guid} active inline='centered' >Fetching Video Result</Loader>
          ))}
        </Grid >
      </Segment>
    </>
  )

})