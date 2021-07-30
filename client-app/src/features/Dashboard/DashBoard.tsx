import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { confirmAlert } from "react-confirm-alert";
import { Link, NavLink } from "react-router-dom";
import { Button, Grid, Header, Icon, Segment, Table } from "semantic-ui-react";
import LoadingComponent from "../../App/Layout/LoadingComponents";
import { useStore } from "../../App/stores/store";
import "react-confirm-alert/src/react-confirm-alert.css"; // Import css
import convertStringToAscii from "../../App/common/class/globalCollection";

export default observer(function Dashboard() {
  const { gridLayoutStore } = useStore();
  const {
    loadgridLayoutMasterData,
    objectOfGridLayoutMaster,
    loadingInitial,
    isRefreshNeeded,
    deleteGridLayout,
  } = gridLayoutStore;
  useEffect(() => {
    if (isRefreshNeeded || objectOfGridLayoutMaster.length === 0) {
      loadgridLayoutMasterData();
    }
  }, [isRefreshNeeded, loadgridLayoutMasterData, objectOfGridLayoutMaster.length]);
  const viewRecord = (index: any) => {
    const record = [...objectOfGridLayoutMaster];
    console.log(record[index].gridLayoutMasterID);
  };

  const deleteRecord = (index: any) => {
    const record = [...objectOfGridLayoutMaster];
    confirmAlert({
      title: "Delete Confirmation",
      message: `Are you sure you want to delete ${record[index].layoutName} record?`,
      buttons: [
        {
          label: "Yes",
          onClick: () => {
            deleteGridLayout(record[index].gridLayoutMasterID);
          },
        },
        {
          label: "No",
          onClick: () => {},
        },
      ],
    });
  };
  if (loadingInitial) return <LoadingComponent content="Loading Saved Data" />;
  return (
    <>
      <Segment>
        <Grid>
          <Grid.Column floated="left" width={5}>
            <Header as="h2">
              <Icon name="settings" />
              <Header.Content>
                Grid
                <Header.Subheader>Manage your preferences</Header.Subheader>
              </Header.Content>
            </Header>
          </Grid.Column>
          <Grid.Column floated="right" width={5}>
            <Button
              animated
              basic
              color="blue"
              size="huge"
              floated="right"
              as={NavLink}
              to="/creategrid"
            >
              <Button.Content visible>
                <Icon name="add square" />
              </Button.Content>
              <Button.Content hidden>Grid</Button.Content>
            </Button>
          </Grid.Column>
        </Grid>
        {objectOfGridLayoutMaster.length > 0 ? (
          <Table unstackable color="blue">
            <Table.Header>
              <Table.Row>
                <Table.HeaderCell>Name</Table.HeaderCell>
                <Table.HeaderCell>No Of Columns</Table.HeaderCell>
                <Table.HeaderCell textAlign="right">Action</Table.HeaderCell>
              </Table.Row>
            </Table.Header>
            <Table.Body>
              {objectOfGridLayoutMaster.map((value, index) => {
                return (
                  <Table.Row key={index}>
                    <Table.Cell>{value.layoutName}</Table.Cell>
                    <Table.Cell>{value.noofColumns}</Table.Cell>
                    <Table.Cell textAlign="right">
                      <Button.Group>
                        <Button primary onClick={() => viewRecord(index)}>
                          View
                        </Button>
                        <Button.Or />
                        <Button
                          as={Link}
                          to={`/creategrid/${convertStringToAscii(
                            value.gridLayoutMasterID.toString()
                          )}`}
                          positive
                        >
                          Edit
                        </Button>
                        <Button.Or />
                        <Button negative onClick={() => deleteRecord(index)}>
                          Delete
                        </Button>
                      </Button.Group>
                    </Table.Cell>
                  </Table.Row>
                );
              })}
            </Table.Body>
          </Table>
        ) : (
          <Header block as="h3">
            Looks like you didnot add Grid in the application. Click{" "}
            <Icon name="add square" />
            icon to add camera panel in the application.
          </Header>
        )}
      </Segment>
    </>
  );
});
