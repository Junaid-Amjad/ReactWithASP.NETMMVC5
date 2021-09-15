import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { useHistory, useParams } from "react-router";
import { Button, Header, Segment } from "semantic-ui-react";
import LoadingComponent from "../../../App/Layout/LoadingComponents";
import { useStore } from "../../../App/stores/store";
import { v4 as uuid } from "uuid";
import { Link } from "react-router-dom";
import { Formik, Form } from "formik";
import * as Yup from "yup";
import MyTextInput from "../../../App/common/form/MyTextInput";
import MyTextArea from "../../../App/common/form/MyTextArea";
import MySelectInput from "../../../App/common/form/MySelectInput";
import MyDateInput from "../../../App/common/form/MyDateInput";
import { categoryOptions } from "../../../App/common/options/CategoryOptions";
import { Activity } from "../../../App/Models/activity";

export default observer(function ActivityForm() {
  const history = useHistory();

  const { activityStore } = useStore();

  const {
    createActivity,
    updateActivity,
    loading,
    loadActivity,
    loadingInitial,
  } = activityStore;

  const { id } = useParams<{ id: string }>();
  const [activity, setActivity] = useState<Activity>({
    activityID: "",
    title: "",
    category: "",
    description: "",
    date: null,
    city: "",
    venue: "",
  });

  const validationSchema = Yup.object({
    title: Yup.string().required("The Activity Title is required"),
    description: Yup.string().required("The Activity Description is required"),
    category: Yup.string().required(),
    date: Yup.string().required("Date is required").nullable(),
    city: Yup.string().required(),
    venue: Yup.string().required(),
  });

  useEffect(() => {
    if (id) loadActivity(id).then((activity) => setActivity(activity!));
  }, [id, loadActivity]);

  function handleFormSubmit(activity: Activity) {
    if (activity.activityID.length === 0) {
      let newActivity = {
        ...activity,
        activityID: uuid(),
      };
      createActivity(newActivity).then(() =>
        history.push(`/activities/${newActivity.activityID}`)
      );
    } else {
      updateActivity(activity).then(() =>
        history.push(`/activities/${activity.activityID}`)
      );
    }
  }

  if (loadingInitial)
    return <LoadingComponent content="Loading Components..." />;
  return (
    <Segment clearing>
      <Header content="Activity Details" sub color="teal" />
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={activity}
        onSubmit={(values) => handleFormSubmit(values)}
      >
        {({ handleSubmit, isValid, isSubmitting, dirty }) => (
          <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
            <MyTextInput name="title" placeholder="Title" />
            <MyTextArea name="description" placeholder="Description" rows={3} />
            <MySelectInput
              options={categoryOptions}
              name="category"              
              placeholder="Category"
            />
            <MyDateInput
              showTimeSelect
              timeCaption="time"
              dateFormat="MMMM dd, yyyy h:mm aa"
              name="date"
              placeholderText="Date"
            />
            <Header content="Location Details" sub color="teal" />
            <MyTextInput name="city" placeholder="City" />
            <MyTextInput name="venue" placeholder="Venue" />
            <Button
              disabled={isSubmitting || !dirty || !isValid}
              loading={loading}
              floated="right"
              positive
              type="submit"
              content="Submit"
              
            />
            <Button
              as={Link}
              to="/activities"
              floated="right"
              type="button"
              content="Cancel"
            />
          </Form>
        )}
      </Formik>
    </Segment>
  );
});
