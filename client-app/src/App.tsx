import React, { useEffect, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import axios from 'axios';
import { Header, List, ListItem } from 'semantic-ui-react';

function App() {
  const [Activities,setActivities] = useState([]);

  useEffect(() => {
    axios.get("http://localhost:5000/api/activities").then(response => {
      console.log(response);
      setActivities(response.data);    
      })
  },[])

  return (
    <div className="App">
      <Header as='h2' icon='users' content='DotnetWithCore' />
        <List>
          {Activities.map((activities:any) => (
            <List.Item key={activities.id}>
              {activities.title}
            </List.Item>
          ))}
        </List>
    </div>
  );
}

export default App;
