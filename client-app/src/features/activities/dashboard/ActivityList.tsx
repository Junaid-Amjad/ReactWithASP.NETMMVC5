import { Button, Item, ItemExtra, Label, Segment } from 'semantic-ui-react'
import { Activity } from '../../../App/Models/activity'

interface Props{
    activities: Activity[];
    Selectactivity:(id:string) => void;
    deleteActivity: (id: string) => void;

}

export default function ActivityList({activities,Selectactivity,deleteActivity}: Props){
    return (
        <Segment>
            <Item.Group divided>
                {activities.map(activity =>(
                    <Item key={activity.id}>
                        <Item.Content>
                            <Item.Header as='a'>{activity.title}</Item.Header>
                            <Item.Header>{activity.date}</Item.Header>
                            <Item.Description>
                                <div>{activity.description}</div>
                                <div>{activity.city}, {activity.venue}</div>
                            </Item.Description>
                            <ItemExtra>
                                <Button onClick={() => Selectactivity(activity.id)} floated='right' content='View' color='blue' />
                                <Button onClick={() => deleteActivity(activity.id)} floated='right' content='Delete' color='red' />
                                <Label basic content={activity.category} />
                            </ItemExtra>
                        </Item.Content>
                    </Item>

                ))}
            </Item.Group>
        </Segment>

    )
}