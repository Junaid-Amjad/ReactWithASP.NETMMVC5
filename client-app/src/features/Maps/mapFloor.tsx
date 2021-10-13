import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { confirmAlert } from "react-confirm-alert";
import Draggable from "react-draggable";
import { Resizable } from "react-resizable";
import TreeMenu, { TreeNodeInArray } from "react-simple-tree-menu";
import { toast } from "react-toastify";
import {
  Button,
  ButtonGroup,
  Grid,
  Segment,
  Image,
  Embed,
} from "semantic-ui-react";
// import default minimal styling or your own styling
import "../../../node_modules/react-simple-tree-menu/dist/main.css";
import LoadingComponent from "../../App/Layout/LoadingComponents";
import { size } from "../../App/stores/modalStore";
import { useStore } from "../../App/stores/store";
import AddingForm, { selectedTreeNode } from "./AddingForm";
import "react-resizable/css/styles.css";
import { runInAction } from "mobx";

export default observer(function MapFloor() {
  const { modalStore, mapStore, userStore } = useStore();
  const {
    loadTreeView,
    isRefreshRequired,
    isloadingInitial,
    deleteMapRecord,
    getMapURL,
    defaultImageURL,
    getChildCamera,
    mapChildImageList,
    setPositionOfTheObject,
    saveMapPosition,
    deleteMapListAll,
    isloadingMapFloor,
  } = mapStore;
  const { user } = userStore;

  const [treedata, settreedata] = useState<TreeNodeInArray[]>();
  const [disableButton, setDisableButton] = useState<boolean>(true);
  const [childMapList, setChildMapList] = useState<boolean>(false);

  const [cameraObjectClicked, setCameraObjectClicked] =
    useState<boolean>(false);
  const initialSelectedNodeValue: selectedTreeNode = {
    hasNode: false,
    name: "",
    level: 0,
    index: 0,
  };
  const [boundTreeView, setBoundTreeView] = useState<{
    left: number;
    right: number;
    top: number;
    bottom: number;
    clientHeight: number;
  }>({
    bottom: 0,
    left: 0,
    right: 0,
    top: 0,
    clientHeight: 0,
  });

  const [selectedTree, setSelectedTree] = useState<selectedTreeNode>(
    initialSelectedNodeValue
  );
  const [mapImageSrc, setMapImageSrc] = useState(defaultImageURL);

  function deleteMapList() {
    //selectedTree.index === undefined
    //  ? console.log("")
    //  : deleteMapListAll(selectedTree.index);
  }

  function deleteRecord() {
    if (selectedTree.index === 0) {
      toast("Please select the Index!");
      return;
    }
    let message = selectedTree.hasNode
      ? `Are you sure you want to delete ${selectedTree.name} record and it's Child Element?`
      : `Are you sure you want to delete ${selectedTree.name} record?`;
    confirmAlert({
      title: "Delete Confirmation",
      message: message,
      buttons: [
        {
          label: "Yes",
          onClick: () => {
            deleteMapRecord(selectedTree.index, user?.id!).then(() => {
              setSelectedTree(initialSelectedNodeValue);
            });
          },
        },
        {
          label: "No",
          onClick: () => {},
        },
      ],
    });
  }

  useEffect(() => {
    if ((user?.id && isRefreshRequired) || (user?.id && !treedata)) {
      loadTreeView(user?.id).then((response) => {
        settreedata(response);
      });
    }
  }, [isRefreshRequired, loadTreeView, user, treedata]);

  if (isloadingInitial) return <LoadingComponent content="Loading!!" />;
  return (
    <Segment>
      {/* <div
        ref={(el) => {
          // el can be null - see https://reactjs.org/docs/refs-and-the-dom.html#caveats-with-callback-refs
          if (!el) return;

          console.log(
            el.getBoundingClientRect().left,
            el.getBoundingClientRect().right,
            el.getBoundingClientRect().top,
            el.getBoundingClientRect().bottom
          ); // prints 200px
        }}
        style={{
          display: "inline-block",
          width: "200px",
          height: "100px",
          background: "blue",
        }}
      /> */}
      <Grid columns="3" stackable>
        <Grid.Column width={3}>
          <ButtonGroup labeled fluid>
            {/* icon */}
            <Button
              //              icon="add"
              content="Add"
              basic
              primary
              onClick={() => {
                setDisableButton(true);
                deleteMapList();
                //              setSelectedTree(initialSelectedNodeValue);
                //setMapImageSrc(defaultImageURL);
                modalStore.openModal(
                  <AddingForm
                    selectedTreeNode={selectedTree}
                    isEditMode={false}
                  />,
                  size.large
                );
              }}
            />
            <Button
              //icon="edit"
              content="Edit"
              basic
              color="teal"
              disabled={disableButton}
              onClick={() => {
                setDisableButton(true);
                deleteMapListAll(selectedTree.index);
                //                setSelectedTree(initialSelectedNodeValue);
                modalStore.openModal(
                  <AddingForm
                    selectedTreeNode={selectedTree}
                    isEditMode={true}
                  />,
                  size.large
                );
              }}
            />
            <Button
              //icon="delete"
              content="Delete"
              basic
              negative
              disabled={disableButton}
              onClick={() => {
                deleteRecord();
                deleteMapListAll(selectedTree.index);
                setMapImageSrc(defaultImageURL);
              }}
            />
          </ButtonGroup>
          <TreeMenu
            data={treedata}
            hasSearch={false}
            resetOpenNodesOnDataUpdate={false}
            onClickItem={({ index, level, label, hasNodes, ...props }) => {
              setDisableButton(false);
              getMapURL(index).then((resp) => {
                setMapImageSrc(resp);
              });
              getChildCamera(index).then((resp) => {
                resp !== undefined
                  ? resp?.length > 0
                    ? setChildMapList(true)
                    : setChildMapList(false)
                  : setChildMapList(false);
              });
              setSelectedTree({
                index: index,
                level: level,
                name: label.toUpperCase(),
                hasNode: hasNodes,
              });
            }}
          />
        </Grid.Column>

        {cameraObjectClicked ? (
          <>
            <Grid.Column width={10}>
              <Embed
                icon="right circle arrow"
                placeholder="/images/image-16by9.png"
                url="http://192.168.137.51:4747/video"
              />
              {/* <iframe
                src="http://192.168.137.51:4747/video"
                title="Streaming"
              /> */}
            </Grid.Column>
          </>
        ) : (
          <>
            <Grid.Column width={10}>
              <div
                ref={(el) => {
                  // el can be null - see https://reactjs.org/docs/refs-and-the-dom.html#caveats-with-callback-refs
                  if (!el) return;

                  // console.log(
                  //   el.getBoundingClientRect().left,
                  //   el.getBoundingClientRect().right,
                  //   el.getBoundingClientRect().top,
                  //   el.getBoundingClientRect().bottom
                  // ); // prints 200px
                  //console.log(boundTreeView.isLoaded);

                  if (boundTreeView.clientHeight !== el.clientHeight) {
                    // console.log("Before");
                    console.log(
                      "Client Height:",
                      el.clientHeight,
                      "Bottom",
                      el.getBoundingClientRect().bottom,
                      "Top",
                      el.getBoundingClientRect().top
                    );
                    setBoundTreeView({
                      ...boundTreeView,
                      left:
                        el.getBoundingClientRect().left -
                        el.getBoundingClientRect().right -
                        50, // -50 is for the padding
                      bottom: el.clientHeight,
                      right: 0,
                      top: -el.getBoundingClientRect().top,
                      clientHeight: el.clientHeight,
                    });

                    // console.log("After", boundTreeView);
                    // console.log(
                    //   "Left:",
                    //   el.getBoundingClientRect().left,
                    //   "Top:",
                    //   el.getBoundingClientRect().top,
                    //   "Right:",
                    //   el.getBoundingClientRect().right,
                    //   "Bottom:",
                    //   el.getBoundingClientRect().bottom
                    // );
                  }
                }}
              >
                <Image src={mapImageSrc} />
              </div>
            </Grid.Column>

            {isloadingMapFloor ? (
              <LoadingComponent content="Loading Map!!" />
            ) : (
              <Grid.Column width={1}>
                {childMapList === true
                  ? mapChildImageList.map((element) => {
                      return (
                        <div
                          key={element.mapListID + 8}
                          ref={(el) => {
                            if (!el) return;

                            runInAction(() => {
                              element.bound.top =
                                el.getBoundingClientRect().top;
                              element.bound.bottom =
                                el.getBoundingClientRect().bottom;
                            });
                            console.log("---------");
                            console.log(
                              element.mapName,
                              "Parent Bottom:",
                              boundTreeView.bottom,
                              "Child Bottom:",
                              element.bound.bottom,
                              "Child Top",
                              element.bound.top,
                              "Total Bottom",
                              boundTreeView.bottom + element.bound.top
                            );
                            // console.log("In-Inner Div");
                            // console.log(
                            //   "Left:",
                            //   el.getBoundingClientRect().left,
                            //   "Top:",
                            //   el.getBoundingClientRect().top,
                            //   "Right:",
                            //   el.getBoundingClientRect().right,
                            //   "Bottom:",
                            //   el.getBoundingClientRect().bottom
                            // );
                          }}
                        >
                          <Draggable
                            defaultPosition={element.Position}
                            key={element.mapListID}
                            bounds={{
                              left: boundTreeView.left,
                              bottom:
                                boundTreeView.bottom +
                                element.bound.top -
                                element.bound.bottom -
                                50,
                              right: boundTreeView.right,
                              top: -(boundTreeView.top + element.bound.top),
                            }}
                            onStop={() => {
                              saveMapPosition(element).then(() => {});
                            }}
                            onDrag={(e, position) => {
                              //console.log(boundTreeView);
                              setPositionOfTheObject(element.mapListID, {
                                x: position.x,
                                y: position.y,
                              });
                            }}
                            cancel={".react-resizable-handle"}
                          >
                            <Resizable
                              height={element.dimension.height}
                              width={element.dimension.width}
                              onResize={(e, data) => {
                                runInAction(() => {
                                  element.dimension.width = data.size.width;
                                  element.dimension.height = data.size.height;
                                });
                                saveMapPosition(element).then(() => {});
                              }}
                            >
                              <div
                                className="box"
                                style={{
                                  width: element.dimension.width + "px",
                                  height: element.dimension.height + "px",
                                }}
                              >
                                <Image
                                  style={{
                                    transform: `rotate(${element.rotation}deg)`,
                                    width: "100%",
                                  }}
                                  src="/asset/Images/D.svg"
                                  id={element.mapListID}
                                  key={element.mapListID}
                                  onMouseEnter={(e: any) => {}}
                                />
                                <Image
                                  className="rotater"
                                  src="/asset/Images/Untitled.png"
                                  onClick={() => {
                                    let newRotation = element.rotation + 45;
                                    if (newRotation >= 360) {
                                      newRotation = -360;
                                    }
                                    runInAction(() => {
                                      element.rotation = newRotation;
                                    });
                                    saveMapPosition(element).then(() => {});
                                  }}
                                />
                                <span className="textcenter">
                                  {element.mapName}
                                </span>
                              </div>
                            </Resizable>
                          </Draggable>
                        </div>
                      );
                    })
                  : null}
              </Grid.Column>
            )}
          </>
        )}
      </Grid>
    </Segment>
  );
});
