import { observer } from 'mobx-react-lite';
import { Link, NavLink } from 'react-router-dom';
import { Button, Container, Dropdown, Image, Menu } from 'semantic-ui-react'
import { useStore } from '../stores/store';

export default observer(function NavBar() {
    const { cameraViewStore: { deleteAllFiles }, userStore: { user, logout } } = useStore();

    function handleItemClickLogout() {
        deleteAllFiles()
        logout()
    }
    function handleItemClick() {
        deleteAllFiles()
    }

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
    )
})