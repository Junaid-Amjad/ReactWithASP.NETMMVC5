import { observer } from 'mobx-react-lite'
import { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import LoadingComponent from '../../App/Layout/LoadingComponents';
import { useStore } from '../../App/stores/store';

export default observer(function SavedFileRedirect(){
    const{savedFileStore} = useStore();

    

//    console.log("here");
//    console.log(window.location.href);
//    const{pathname} = useParams<{pathname: string}>();
//    console.log(pathname);


    return(<h1></h1>);

})