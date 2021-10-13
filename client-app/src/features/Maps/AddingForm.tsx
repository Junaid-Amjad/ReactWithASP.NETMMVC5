import { Formik } from "formik";
import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import {
  Form,
  Grid,
  Input,
  Segment,
  Image,
  Button,
  Label,
} from "semantic-ui-react";
import MySelectInput from "../../App/common/form/MySelectInput";
import MyTextInput from "../../App/common/form/MyTextInput";
import { MapList } from "../../App/Models/Map/mapList";
import { Ilistofcamera } from "../../App/stores/gridLayoutStore";
import { IMapCategories } from "../../App/stores/Map/mapStore";
import { useStore } from "../../App/stores/store";
import * as Yup from "yup";
import { toast } from "react-toastify";
import LoadingComponent from "../../App/Layout/LoadingComponents";
import { Resizable } from "react-resizable";

export interface selectedTreeNode {
  index: number;
  level: number;
  name: string;
  hasNode: boolean;
}

interface Props {
  selectedTreeNode: selectedTreeNode;
  isEditMode: boolean;
}

export default observer(function AddingForm({
  selectedTreeNode,
  isEditMode,
}: Props) {
  const { mapStore, gridLayoutStore, userStore } = useStore();

  const {
    MapCategories,
    loadMapCategories,
    saveRecord,
    isloading,
    isloadingPopup,
    loadMapRecord,
    updateRecord,
    serverAPILink,
    getMapCameraRotation,
    defaultWidthheight,
  } = mapStore;
  const { loadCameraList, listOfCameraIP } = gridLayoutStore;
  const { user } = userStore;

  const [maplistcat, setmaplistcategories] = useState<IMapCategories[]>([]);
  const [listOfCamera, setListOfCamera] = useState<Ilistofcamera[]>([]);
  const [rotation, setRotation] = useState<number>(0);
  const [dimension, setDimension] = useState<{ width: number; height: number }>(
    { width: defaultWidthheight, height: defaultWidthheight }
  );
  const [mapvalues, setmapvalues] = useState<MapList>({
    mapListID: isEditMode ? selectedTreeNode.index : 0,
    mapListName: "",
    mapCategoriesID: 0,
    entryDate: new Date().toString(),
    //    imageName: "",
    userID: "",
    systemIP: "",
    systemName: "",
    isActive: true,
    isCancel: false,
    imageFile: null,
    imageSrc: "",
    cameraIP: "",
    levelNo: isEditMode ? 0 : selectedTreeNode.level + 1,
    parentID: isEditMode ? 0 : selectedTreeNode.index,
  });

  function handleImageChange(
    e: any,
    maplistvalues: MapList,
    setMapValues: any
  ) {
    let reader = new FileReader();
    let file = e.target.files[0];
    if (file) {
      reader.onloadend = () => {
        setMapValues({
          ...maplistvalues,
          imageFile: file,
          imageSrc: [reader.result],
        });
      };
      reader.readAsDataURL(file);
    }
  }
  function submitValues(values: MapList) {
    if (
      values.mapCategoriesID === 1 &&
      values.imageFile === null &&
      !isEditMode
    ) {
      toast("Please select the Image", { type: "error" });
      return;
    }

    if (values.mapCategoriesID === 2) {
      values.imageFile = null;
      values.imageSrc = "";
    } else if (values.mapCategoriesID === 1) {
      values.cameraIP = "";
    } else {
      values.imageFile = null;
      values.imageSrc = "";
      values.cameraIP = "";
    }

    setmapvalues(values);
    const data = new FormData();
    data.append("mapList.mapListID", JSON.stringify(values.mapListID));
    data.append("mapList.mapListName", values.mapListName);
    data.append("mapList.mapCategoriesID", values.mapCategoriesID.toString());
    data.append("mapList.imageFile", values.imageFile);
    data.append("mapList.cameraIP", values.cameraIP);
    data.append("mapList.isActive", JSON.stringify(values.isActive));
    data.append("mapList.isCancel", JSON.stringify(values.isCancel));
    data.append("mapList.userID", user!.id.toString());
    data.append("mapList.levelNo", JSON.stringify(values.levelNo));
    data.append("mapList.parentID", JSON.stringify(values.parentID));
    data.append(
      "mapList.imageSrc",
      values.imageSrc.toString().replace(serverAPILink, "")
    );
    data.append("mapPosition.rotation", JSON.stringify(rotation));
    data.append(
      "mapPosition.width",
      JSON.stringify(Math.round(dimension!.width))
    );
    data.append(
      "mapPosition.height",
      JSON.stringify(Math.round(dimension!.height))
    );

    if (isEditMode) updateRecord(data).then((res) => {});
    else saveRecord(data).then((res) => {});
  }

  const validationSchema = Yup.object({
    mapListName: Yup.string().required("Please Enter the Name"),
    mapCategoriesID: Yup.number().min(1).required("Please Select the Category"),
    cameraIP: Yup.string().when("mapCategoriesID", {
      is: 2,
      then: Yup.string().required("Please Enter the Camera IP"),
    }),
  });

  useEffect(() => {
    if (maplistcat.length === 0) {
      loadMapCategories().then(() => {
        MapCategories.forEach((value) => {
          setmaplistcategories((maplistcat) => [...maplistcat, value]);
        });
      });
    }
  }, [loadMapCategories, MapCategories, maplistcat]);
  useEffect(() => {
    if (listOfCamera.length === 0) {
      loadCameraList().then((listCamera) => {
        listOfCamera.splice(0, listOfCamera.length);
        listCamera?.forEach((value) => {
          setListOfCamera((listOfCamera) => [...listOfCamera, value]);
        });
      });
    }
  }, [listOfCamera, listOfCameraIP, loadCameraList]);

  useEffect(() => {
    if (mapvalues.mapListID > 0) {
      loadMapRecord(mapvalues.mapListID).then((mapvalue) => {
        setmapvalues(mapvalue!);
        getMapCameraRotation(mapvalue!.mapListID).then((res) => {
          setRotation(res!.rotation);
          setDimension({
            width: res?.width === 0 ? defaultWidthheight : res?.width!,
            height: res?.height === 0 ? defaultWidthheight : res?.height!,
          });
        });
      });
    }
  }, [
    mapvalues.parentID,
    loadMapRecord,
    mapvalues.mapListID,
    getMapCameraRotation,
    defaultWidthheight,
  ]);

  if (isloadingPopup) return <LoadingComponent content="loading!!" />;
  return (
    <Segment>
      <Formik
        enableReinitialize
        initialValues={mapvalues}
        onSubmit={(values) => submitValues(values)}
        validationSchema={validationSchema}
      >
        {({
          handleSubmit,
          values,
          setValues,
          isSubmitting,
          isValid,
          dirty,
        }) => (
          <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
            <Grid columns={2} stackable>
              {!isEditMode && (
                <Grid.Row>
                  <Grid.Column>
                    <Label content={selectedTreeNode.name} tag color="red" />
                  </Grid.Column>
                </Grid.Row>
              )}
              <Grid.Row>
                <Grid.Column>
                  <MyTextInput name="mapListName" placeholder="Name" />
                </Grid.Column>
                <Grid.Column>
                  <MySelectInput
                    name="mapCategoriesID"
                    options={maplistcat}
                    key="mapCategoriesID"
                    placeholder="Please select Map Categories"
                  />
                </Grid.Column>
              </Grid.Row>
              {values.mapCategoriesID === 1 ? (
                <>
                  <Grid.Row>
                    <Grid.Column width={16}>
                      <Input
                        type="file"
                        accept="image/*"
                        name="file"
                        onChange={(event) => {
                          handleImageChange(event, values, setValues);
                        }}
                      />
                    </Grid.Column>
                  </Grid.Row>
                  <Grid.Row>
                    {values.imageSrc ? (
                      <Image
                        src={values.imageSrc}
                        name="imageSrc"
                        size="massive"
                      />
                    ) : null}
                  </Grid.Row>
                </>
              ) : values.mapCategoriesID === 2 ? (
                <>
                  <Grid.Row>
                    <Grid.Column>
                      <MySelectInput
                        name="cameraIP"
                        options={listOfCamera}
                        key="cameraIP"
                        placeholder="Please select Camera"
                      />
                    </Grid.Column>
                    <Grid.Column>
                      <Resizable
                        width={dimension.width}
                        height={dimension.height}
                        onResize={(e: any, data: any) => {
                          setDimension({
                            ...dimension,
                            width: data.size.width,
                            height: data.size.height,
                          });
                        }}
                      >
                        <div
                          className="box"
                          style={{
                            width: dimension.width + "px",
                            height: dimension.height + "px",
                          }}
                        >
                          <Image
                            //className="fitImage"
                            style={{
                              transform: `rotate(${rotation}deg)`,
                              width: "100%",
                            }}
                            src="/asset/Images/D.svg"
                            id="rotation"
                            key="rotation"
                            onMouseEnter={(e: any) => {}}
                          />
                          <Image
                            className="rotater"
                            src="/asset/Images/Untitled.png"
                            onClick={() => {
                              let newRotation = rotation + 45;
                              if (newRotation >= 360) {
                                newRotation = -360;
                              }
                              setRotation(newRotation);
                            }}
                          />
                        </div>
                      </Resizable>
                    </Grid.Column>
                  </Grid.Row>
                </>
              ) : (
                <></>
              )}

              <Grid.Row>
                <Grid.Column>
                  <Button
                    disabled={isSubmitting || !isValid}
                    loading={isloading}
                    type="submit"
                    positive
                    floated="right"
                    content="Submit"
                  />
                </Grid.Column>
              </Grid.Row>
            </Grid>
          </Form>
        )}
      </Formik>
    </Segment>
  );
});
