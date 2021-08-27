import * as Yup from "yup";
import { FieldArray, Formik, getIn } from "formik";
import { observer } from "mobx-react-lite";
import { Link, useHistory, useParams } from "react-router-dom";
import {
  Button,
  Container,
  Dropdown,
  Form,
  Grid,
  Header,
  Icon,
  Label,
  Segment,
} from "semantic-ui-react";
import MySelectInput from "../../App/common/form/MySelectInput";
import MyTextInput from "../../App/common/form/MyTextInput";
import { columnOptions } from "../../App/common/options/ColumnOptions";
import { useStore } from "../../App/stores/store";
import { useEffect } from "react";
import LoadingComponent from "../../App/Layout/LoadingComponents";
import { useState } from "react";
import { IGridLayout } from "../../App/Models/IGridLayout";

export default observer(function CreatingGrid() {
  const history = useHistory();
  let { id } = useParams<{ id: string }>();

  const { gridLayoutStore } = useStore();

  const {
    loadCameraList,
    listOfCameraIP,
    loadingInitial,
    saveGridLayout,
    loadGridLayoutDetail,
    updateGridLayout,
  } = gridLayoutStore;

  useEffect(() => {
    if (listOfCameraIP.length === 0) {
      loadCameraList();
    }
    if (id) {
      loadGridLayoutDetail(id).then((loadingGridLayoutData) => {
        setinitialValues(loadingGridLayoutData!);
      });
    }
  }, [loadCameraList, listOfCameraIP.length, id, loadGridLayoutDetail]);

  const [initialValues, setinitialValues] = useState<IGridLayout>({
    GridLayoutMasterID: 0,
    nameOfTheGrid: "",
    column: "",
    cameras: [{ cameraIP: "", itemindex: 0 }],
  });

  const validationSchema = Yup.object().shape({
    nameOfTheGrid: Yup.string().required("Please Enter Name"),
    column: Yup.string().required("Please Select Number Of columns"),
    cameras: Yup.array().of(
      Yup.object().shape({
        //temp
        cameraIP: Yup.string().required(
          "Please Select IP Address of the camera"
        ),
      })
    ),
  });

  // handle input change
  const handleInputChange = (
    setValues: any,
    values: any,
    e: any,
    index: any
  ) => {
    const value = e.target.innerText;
    const cameras = [...values.cameras];
    cameras[index].cameraIP = value;
    setValues({ ...values, cameras });
  };

  // handle click event of the Remove button
  const handleRemoveClick = (setValues: any, values: any, index: any) => {
    const cameras = [...values.cameras];
    cameras.splice(index, 1);
    setValues({ ...values, cameras });
  };
  const handleAddClick = (setValues: any, values: any) => {
    const cameras = [...values.cameras];
    let dynamiccreatedgrid = cameras[cameras.length - 1].itemindex + 1;
    cameras.push({ cameraIP: "", itemindex: dynamiccreatedgrid });
    setValues({ ...values, cameras });
  };

  function handleFormSubmit(values: IGridLayout) {
    if (values.GridLayoutMasterID === 0) {
      //Create new Grid Layout
      saveGridLayout(values).then(() => history.push("/dashboard"));
    } else {
      //Update existing Grid Layout
      updateGridLayout(values).then(() => history.push("/dashboard"));
    }
  }
  if (loadingInitial) return <LoadingComponent content="Loading Content" />;
  return (
    <Segment clearing>
      <Header content="Creating Grid" color="teal" />
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={initialValues}
        onSubmit={(values) => handleFormSubmit(values)}
      >
        {({
          handleSubmit,
          isValid,
          isSubmitting,
          dirty,
          setValues,
          values,
          errors,
          touched,
          handleBlur,
        }) => (
          <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
            <Grid columns={2} doubling>
              <Grid.Row>
                <Grid.Column>
                  <MyTextInput
                    name="nameOfTheGrid"
                    key="nameOfTheGrid"
                    placeholder="Name"
                  />
                </Grid.Column>
                <Grid.Column>
                  <MySelectInput
                    options={columnOptions}
                    name="column"
                    key="column"
                    placeholder="Number Of Columns"
                  />
                </Grid.Column>
              </Grid.Row>
            </Grid>
            <Grid columns={2} doubling>
              <FieldArray name="listofcameras">
                {() =>
                  values.cameras.map((x, i) => {
                    const name = `cameras[${i}].cameraIP`;
                    const errorMessage = getIn(errors, name);
                    const touchedMessage = getIn(touched, name);
                    return (
                      <Grid.Row key={x.itemindex + 1}>
                        <Grid.Column key={x.itemindex + 2} width={14}>
                          <Form.Field>
                            <Dropdown
                              options={listOfCameraIP}
                              name={name}
                              key={x.itemindex + 3}
                              value={x.cameraIP}
                              onBlur={handleBlur(name)}
                              fluid
                              selection
                              clearable
                              scrolling
                              placeholder="Select Camera"
                              onChange={(text) =>
                                handleInputChange(setValues, values, text, i)
                              }
                            />
                            {errorMessage && touchedMessage && (
                              <Label
                                basic
                                color="red"
                                key={x.itemindex + 5}
                                pointing
                                content={errorMessage}
                              />
                            )}
                          </Form.Field>
                        </Grid.Column>
                        <Grid.Column key={x.itemindex + 4} width={2}>
                          {values.cameras.length - 1 === i && (
                            <Button
                              positive
                              key={x.itemindex}
                              onClick={() => handleAddClick(setValues, values)}
                              animated
                              type="button"
                            >
                              <Button.Content visible>
                                <Icon name="add" />
                              </Button.Content>
                              <Button.Content hidden>Add</Button.Content>
                            </Button>
                          )}
                          {values.cameras.length !== 1 && (
                            <Button
                              negative
                              onClick={() =>
                                handleRemoveClick(setValues, values, i)
                              }
                              animated
                            >
                              <Button.Content visible>
                                <Icon name="remove" />
                              </Button.Content>
                              <Button.Content hidden>Remove</Button.Content>
                            </Button>
                          )}
                        </Grid.Column>
                      </Grid.Row>
                    );
                  })
                }
              </FieldArray>
            </Grid>
            <Grid columns={1}>
              <Grid.Row>
                <Grid.Column>
                  <Container>
                    <Button
                      disabled={isSubmitting || !dirty || !isValid}
                      floated="right"
                      positive
                      type="submit"
                      content="Submit"
                    />
                    <Button
                      as={Link}
                      to="/dashboard"
                      floated="right"
                      type="button"
                      content="Back"
                    />
                  </Container>
                </Grid.Column>
              </Grid.Row>
            </Grid>
          </Form>
        )}
      </Formik>
    </Segment>
  );
});
