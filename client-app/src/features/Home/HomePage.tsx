import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import {
  Button,
  Container,
  Grid,
  Header,
  Image,
  Segment,
} from "semantic-ui-react";
import { useStore } from "../../App/stores/store";
import LoginForm from "../users/LoginForm";
import RegisterForm from "../users/RegisterForm";

export default observer(function HomePage() {
  const { userStore, modalStore } = useStore();
  return (
    <Segment inverted textAlign="center" vertical className="masthead">
      <Container text>
        <Grid stackable columns={1}>
          <Grid.Column>
            <Grid.Row>
              <Header as="h1" inverted>
                <Image
                  size="massive"
                  src="/asset/logo.svg"
                  alt="logo"
                  style={{ marginBottom: 12 }}
                />
              </Header>
            </Grid.Row>
            <Grid.Row>
              {userStore.isLoggedIn ? (
                <>
                  {/*<Header as='h2' inverted content='Welcome to StrandUSA Application' />*/}
                  <Button as={Link} to="/redirect" size="huge" inverted>
                    Go to Dashboard
                  </Button>
                </>
              ) : (
                <>
                  <Button
                    onClick={() => modalStore.openModal(<LoginForm />)}
                    size="huge"
                    inverted
                  >
                    Login
                  </Button>
                  <Button
                    onClick={() => modalStore.openModal(<RegisterForm />)}
                    size="huge"
                    inverted
                  >
                    Register
                  </Button>
                </>
              )}
            </Grid.Row>
          </Grid.Column>
        </Grid>
      </Container>
    </Segment>
  );
});
