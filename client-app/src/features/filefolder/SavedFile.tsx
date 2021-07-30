import { observer } from 'mobx-react-lite'
import { useEffect } from 'react';
import ReactPlayer from 'react-player';
import { Link, useParams } from 'react-router-dom';
import { Grid, Header, List, Breadcrumb } from 'semantic-ui-react'
import LoadingComponent from '../../App/Layout/LoadingComponents';
import { useStore } from '../../App/stores/store'

export default observer(function SavedFile() {
    const { savedFileStore } = useStore();
    let BreadCrumbPath = "";

    const { loadSavedFiles, getLoadedSavedFiles, loadDirectoryFiles, DirectoryLink } = savedFileStore;

    const { pathname } = useParams<{ pathname: string }>();

    let PathLink = "";
    DirectoryLink.forEach((object, i) => {
        PathLink += object + "/";
    })
    useEffect(() => {
        if (pathname !== undefined) {
            loadDirectoryFiles(pathname);
        }
        else {
            loadSavedFiles();
        }

    }, [loadSavedFiles, pathname, loadDirectoryFiles]
    )

    if (savedFileStore.loadingInitial) return <LoadingComponent content="Loading Files" />
    return (
        <>
            {DirectoryLink.map((object, i) => {
                BreadCrumbPath += "/" + object;
                return (
                    <Breadcrumb key={object}>
                        <Breadcrumb.Section key={object} as={Link} to={BreadCrumbPath} >{object}</Breadcrumb.Section>
                        <Breadcrumb.Divider />
                    </Breadcrumb>)
            })}

            <Header as='h1' content='Folders' color='teal' />
            <List divided relaxed >
                {getLoadedSavedFiles.map((key, value) => (
                    key.isDirectory ?
                        (
                            <List.Item key={key.path}>
                                <List.Content key={key.path}>
                                    <List.Header as={Link} to={'/' + PathLink + key.path} key={key.path}>{key.path}</List.Header>
                                </List.Content>
                            </List.Item>

                        ) : null

                ))}
            </List>

            <Header as='h1' content='Files' color='teal' />
            <Grid>
                <Grid.Row columns={4}>
                    {getLoadedSavedFiles.map((key, value) => (
                        key.isDirectory ?
                            null
                            : key.isSamePath ?
                                (
                                    <Grid.Column key={key.path} >
                                        <ReactPlayer url={key.path} controls width='100%' height='90%' />
                                    </Grid.Column>
                                ) : null
                    ))}
                </Grid.Row>

            </Grid>

        </>
    )




})
