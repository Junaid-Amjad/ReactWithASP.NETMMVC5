import { makeAutoObservable } from "mobx";

export enum size {
  mini = 1,
  tiny = 2,
  small = 3,
  large = 4,
  fullscreen = 5,
}

interface Modal {
  open: boolean;
  body: JSX.Element | null;
  size: size;
}

export default class ModalStore {
  modal: Modal = {
    open: false,
    body: null,
    size: size.small,
  };
  constructor() {
    makeAutoObservable(this);
  }
  openModal = (Content: JSX.Element, size: size) => {
    this.modal.open = true;
    this.modal.body = Content;
    this.modal.size = size;
  };

  closeModal = () => {
    this.modal.open = false;
    this.modal.body = null;
    this.modal.size = size.small;
  };
}
