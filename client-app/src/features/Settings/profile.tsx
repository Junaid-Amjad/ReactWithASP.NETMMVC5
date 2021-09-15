import { Formik } from "formik";
import { observer } from "mobx-react-lite";
import { Form, Header, Label, Segment, Button, Grid } from "semantic-ui-react";
import MyTextInput from "../../App/common/form/MyTextInput";
import * as Yup from "yup";
import { useState } from "react";
import { useStore } from "../../App/stores/store";
import { useEffect } from "react";
import LoadingComponent from "../../App/Layout/LoadingComponents";
import MySelectInput from "../../App/common/form/MySelectInput";
import { userProfile } from "../../App/Models/Profile/userProfile";
import { useParams } from "react-router-dom";

export default observer(function Profile() {
  const { userName } = useParams<{ userName: string }>();

  const { profileStore, userStore } = useStore();

  const {
    loadUserView,
    isloadingInitial,
    userViews,
    loadUserDetails,
    loading,
    updateUserData,
  } = profileStore;

  const { user } = userStore;

  const [userProfile, setUserProfile] = useState<userProfile>({
    id: "",
    userName: "",
    contactNo: "",
    email: "",
    image: "",
    userViewID: "",
    displayName: "",
    token: "",
    userID: userName,
    userIP: "",
    userSystem: "",
  });
  const validationSchema = Yup.object({
    userName: Yup.string().required("UserName is required"),
    email: Yup.string().required("Email is required"),
    contactNo: Yup.string().required("Contact is required"),
    userViewID: Yup.number().min(1).required("Display Option is required"),
  });

  useEffect(() => {
    if (userViews.length === 0) {
      loadUserView();
    }
    if (userName)
      loadUserDetails(userName).then((UserProfile) => {
        setUserProfile(UserProfile!);
      });
  }, [loadUserView, userViews.length, loadUserDetails, userName]);

  function handleFormSubmit(userProfile: userProfile) {
    userProfile.userID = userName;
    updateUserData(userProfile).then(() => {
      user!.userViewID = parseInt(userProfile.userViewID);
      //      alert("Profile updated successfully.");
      setUserProfile(userProfile);
    });
  }
  if (isloadingInitial) return <LoadingComponent content="Loading!!" />;
  return (
    <Segment clearing>
      <Header content="Profile" color="teal" />
      <Grid stackable columns={1} divided>
        {/* <Grid.Column width={3}>
          <Card>
            <Image
              src="http://192.168.18.150/Images/matthew.png"
              size="medium"
            />
          </Card>
          <Button basic fluid>
            Browse
          </Button>
        </Grid.Column> */}
        <Grid.Column width={16}>
          <Formik
            validationSchema={validationSchema}
            enableReinitialize
            initialValues={userProfile}
            onSubmit={(values) => handleFormSubmit(values)}
          >
            {({ handleSubmit, isValid, isSubmitting, dirty }) => (
              <Form
                className="ui form"
                autoComplete="off"
                onSubmit={handleSubmit}
              >
                <Label content="UserName" color="teal" />
                <MyTextInput name="userName" placeholder="UserName" readOnly />
                <Label content="Email" color="teal" />
                <MyTextInput name="email" placeholder="Email" type="Email" />
                <Label content="Contact" color="teal" />
                <MyTextInput name="contactNo" placeholder="Contact" />
                <Label content="Display Option" color="teal" />
                <MySelectInput
                  options={userViews}
                  placeholder="Please select Display Options"
                  name="userViewID"
                />
                <Button
                  disabled={isSubmitting || !dirty || !isValid}
                  loading={loading}
                  floated="right"
                  positive
                  type="submit"
                  content="Submit"
                />
              </Form>
            )}
          </Formik>
        </Grid.Column>
      </Grid>
    </Segment>
  );
});
