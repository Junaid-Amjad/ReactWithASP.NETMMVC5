import { observer } from "mobx-react-lite";
import { useState } from "react";
import { Link, NavLink } from "react-router-dom";
import { Button, Dropdown, Grid, Icon, Image, Menu } from "semantic-ui-react";
import { useStore } from "../stores/store";

export default observer(function NavBar() {
  const {
    cameraViewStore: { deleteFiles },
    userStore: { user, logout },
    searchFileStore: { refreshvalue, isLoadingContent },
  } = useStore();

  function handleItemClickLogout() {
    refreshvalue();
    if (!deleteFiles("0")) {
      alert("Error deleting files");
    }
    logout();
  }
  function handleItemClick(value: any) {
    if (isLoadingContent) {
      refreshvalue();
    }
    if (!deleteFiles("0")) alert("Error deleting files");
  }

  const [state, setState] = useState({
    dropdownMenuStyle: {
      display: "none",
    },
  });

  let handleToggleDropdownMenu = () => {
    let newState = Object.assign({}, state);
    if (newState.dropdownMenuStyle.display === "none") {
      newState.dropdownMenuStyle = { display: "flex" };
    } else {
      newState.dropdownMenuStyle = { display: "none" };
    }

    setState(newState);
  };

  return (
    <>
      <Grid padded className="tablet computer only">
        <Menu fluid size="huge">
          <Menu.Item
            as={NavLink}
            exact
            to="/"
            header
            onClick={() => handleItemClick("/home")}
          >
            <img
              src="/Asset/logo.svg"
              alt="logo" /*style={{ marginRight: '10px' }} */
            />
          </Menu.Item>
          {/*<Menu.Item as={NavLink} to='/activities' name="Activities" onClick={handleItemClick} />
                    <Menu.Item as={NavLink} to='/errors' name="Errors" onClick={handleItemClick} />*/}

          <Menu.Item
            as={NavLink}
            to="/dashboard"
            name="DashBoard"
            onClick={() => handleItemClick("/dashboard")}
          />
          <Menu.Item
            as={NavLink}
            to="/files"
            name="Files"
            onClick={() => handleItemClick("/files")}
          />
          {/* <Menu.Item
            as={NavLink}
            to="/cameraView"
            name="Live"
            onClick={() => handleItemClick("/cameraView")}
          /> */}
          <Menu.Item
            as={NavLink}
            to="/Search"
            name="Search"
            onClick={() => handleItemClick("/Search")}
          />
          <Menu.Item position="right">
            <Image
              src={user?.image || "/asset/user.png"}
              avatar
              spaced="right"
            />
            <Dropdown pointing="top left" text={user?.displayName}>
              <Dropdown.Menu className="dropdownitem">
                <Dropdown.Item
                  as={Link}
                  to={`/profile/${user?.id}`}
                  text="My Profile"
                  icon="user"
                  onClick={() => handleItemClick("/profile")}
                />
                <Dropdown.Item
                  onClick={handleItemClickLogout}
                  text="Logout"
                  icon="power"
                />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Item>
        </Menu>
      </Grid>
      <Grid padded className="mobile only">
        <Menu borderless fluid size="huge">
          <Menu.Item as={NavLink} exact to="/" header onClick={handleItemClick}>
            <img
              src="/Asset/logo.svg"
              alt="logo" /*style={{ marginRight: '10px' }}*/
            />
          </Menu.Item>
          <Menu.Menu position="right">
            <Menu.Item>
              <Button icon basic toggle onClick={handleToggleDropdownMenu}>
                <Icon name="content" />
              </Button>
            </Menu.Item>
          </Menu.Menu>
          <Menu vertical borderless fluid style={state.dropdownMenuStyle}>
            <Menu.Item
              as={NavLink}
              to="/files"
              name="Files"
              onClick={handleItemClick}
            />
            <Menu.Item
              as={NavLink}
              to="/cameraView"
              name="Live"
              onClick={handleItemClick}
            />
            <Menu.Item
              as={NavLink}
              to="/Search"
              name="Search"
              onClick={handleItemClick}
            />
            <Menu.Item>
              <Image
                src={user?.image || "/asset/user.png"}
                avatar
                spaced="right"
              />
              <Dropdown pointing="top left" text={user?.displayName}>
                <Dropdown.Menu>
                  <Dropdown.Item
                    as={Link}
                    to={`/profile/${user?.id}`} //${user?.username}
                    text="My Profile"
                    icon="user"
                    onClick={handleItemClick}
                  />
                  <Dropdown.Item
                    onClick={handleItemClickLogout}
                    text="Logout"
                    icon="power"
                  />
                </Dropdown.Menu>
              </Dropdown>
            </Menu.Item>
          </Menu>
        </Menu>
      </Grid>
    </>
  );
  /*
    return (
        <Menu inverted fixed='top'>
                <Container>
                    <Menu.Item as={NavLink} exact to='/' header onClick={handleItemClick}>
                        <img src="/Asset/logo.png" alt="logo" style={{ marginRight: '10px' }} />
                        React With DotNet
                    </Menu.Item>
                    <Menu.Item as={NavLink} to='/activities' name="Activities" onClick={handleItemClick} />
                    <Menu.Item as={NavLink} to='/errors' name="Errors" onClick={handleItemClick} />
                    <Menu.Item as={NavLink} to='/files' name="Files" onClick={handleItemClick} />
                    <Menu.Item as={NavLink} to='/cameraView' name="Live" onClick={handleItemClick} />
                    <Menu.Item as={NavLink} to='/Search' name="Search" onClick={handleItemClick} />
                    <Menu.Item >
                        <Button as={NavLink} to='/createActivity' positive content='Create Activity' onClick={handleItemClick} />
                    </Menu.Item>
                    <Menu.Item position='right'  >
                        <Image src={user?.image || '/asset/user.png'} avatar spaced='right' />
                        <Dropdown pointing='top left' text={user?.displayName}>
                            <Dropdown.Menu>
                                <Dropdown.Item as={Link} to={`/profile/${user?.username}`} text="My Profile" icon='user' onClick={handleItemClick} />
                                <Dropdown.Item onClick={handleItemClickLogout} text='Logout' icon='power' />
                            </Dropdown.Menu>
                        </Dropdown>
                    </Menu.Item>
                </Container>

            </Menu>
            )*/
});
