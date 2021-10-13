export interface RootObject {
  parent: Parent;
}

interface Parent {
  xmlnsXsi: string;
  nodeParent: NodeParent[];
}

interface NodeParent {
  index: string;
  label: string;
  catID: string;
  imgPath: string;
  cameraIP: string;
  nodes: Nodes;
}

interface Nodes {
  child: Child2;
}

interface Child2 {
  index: string;
  label: string;
  catID: string;
  imgPath: string;
  cameraIP: string;
  nodes?: Node;
}

interface Node {
  child: Child;
}

interface Child {
  index: string;
  label: string;
  catID: string;
  imgPath: string;
  cameraIP: string;
  nodes?: any;
}