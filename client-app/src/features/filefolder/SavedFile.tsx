import { observer } from 'mobx-react-lite'
import {  useEffect } from 'react';
import ReactPlayer from 'react-player';
import { Grid,Header,List } from 'semantic-ui-react'
import LoadingComponent from '../../App/Layout/LoadingComponents';
import { useStore } from '../../App/stores/store'

export default observer(function SavedFile(){
    const{savedFileStore} = useStore();
    const{loadSavedFiles,savedRegistry,getLoadedSavedFiles} = savedFileStore;

    useEffect(() => {
        if(savedRegistry.size <= 1) loadSavedFiles();
        },[savedRegistry.size,loadSavedFiles])
      
        if(savedFileStore.loadingInitial) return <LoadingComponent content="Loading Files" />

    return(
        <>
            <Header as='h1' content='Folders' color='teal' />
            <List divided relaxed >
                {getLoadedSavedFiles.map((key,value) =>(
                    key.isDirectory ? 
                    (            
                        <List.Item key={key.path}>
                        <List.Content key={key.path}>
                            <List.Header as='a' key={key.path}>{key.path}</List.Header>
                        </List.Content>
                        </List.Item>
                    
                    ) : null

                )   )}
            </List>

            <Header as='h1' content='Files' color='teal' />
            <Grid>
                <Grid.Row columns={4}>
                {getLoadedSavedFiles.map((key,value) => (
                    key.isDirectory ? 
                    null 
                    : key.isSamePath ? 
                    (
                    <Grid.Column key={key.path} >
                        <ReactPlayer url={key.path} controls width='100%' height='90%' />
                    </Grid.Column>
                    ) : null
                 ) )}
                </Grid.Row>

            </Grid>
          
        </>  
    )




})
