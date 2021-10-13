import { observer } from "mobx-react-lite";
import { Modal } from "semantic-ui-react";
import { size } from "../../stores/modalStore";
import { useStore } from "../../stores/store";

export default observer(function ModalContainer() {
  const { modalStore } = useStore();
  return (
    <Modal
      dimmer="blurring"
      open={modalStore.modal.open}
      onClose={modalStore.closeModal}
      size={
        modalStore.modal.size === size.small
          ? "small"
          : modalStore.modal.size === size.large
          ? "large"
          : modalStore.modal.size === size.mini
          ? "mini"
          : modalStore.modal.size === size.tiny
          ? "tiny"
          : "fullscreen"
      }
    >
      <Modal.Content>{modalStore.modal.body}</Modal.Content>
    </Modal>
  );
});
