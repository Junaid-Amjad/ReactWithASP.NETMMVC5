import {  useEffect, useState } from 'react';
import axios from 'axios';
import { Container } from 'semantic-ui-react';
import { Activity } from '../Models/activity';
import NavBar from './Navbar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import {v4 as uuid} from 'uuid';

function App() {
  const [Activities,setActivities] = useState<Activity[]>([]);
  const [selectedActivity,setSelectedActivity] = useState<Activity | undefined>(undefined);
  const [editMode,setEditMode] = useState(false);

  useEffect(() => {
    axios.get<Activity[]>("http://localhost:5000/api/activities").then(response => {
      setActivities(response.data);    
      })
  },[])

  function handleSelectActivity(id: string){
      setSelectedActivity(Activities.find(x=>x.id === id));
  }
  function handleCancelSelectedActivity(){
    setSelectedActivity(undefined);
  }

  function handleFormOpen(id?: string)
  {
    id ? handleSelectActivity(id) : handleCancelSelectedActivity();
    setEditMode(true);
  }

  function handleFormClose(){
    setEditMode(false);
  }

  function handleCreateOrEditActivity(activity: Activity){
    activity.id ? setActivities([...Activities.filter(x => x.id !== activity.id),activity])
    :setActivities([...Activities,{...activity,id:uuid()}]);
    setEditMode(false);
    setSelectedActivity(activity);
  }
  function handleDeleteActivity(id:string){
    setActivities([...Activities.filter(x=>x.id !== id)])
  }

  return (
    <>
      <NavBar openForm={handleFormOpen} />
        <Container style={{marginTop :'7em'}}>
          <ActivityDashboard 
          activities={Activities} 
          selectedActivity={selectedActivity}
          Selectactivity ={handleSelectActivity}
          cancelSelectActivity={handleCancelSelectedActivity}
          editMode={editMode}
          openForm={handleFormOpen}
          closeForm={handleFormClose}
          createOrEdit={handleCreateOrEditActivity}
          deleteActivity={handleDeleteActivity}
          />

        </Container>
    </>
  );
}

export default App;
