import * as React from 'react';
import Bell from '../img/svg/bell.svg';
import styles from './HeaderProfile.module.scss';
import { Link } from 'react-router-dom';
import Placeholder from '../img/svg/profile.svg';
import Popover from '@mui/material/Popover';
import Typography from '@mui/material/Typography';
import { useNavigate } from "react-router-dom";


export function BasicPopover(props) {

    let navigate = useNavigate();

    const handleSignOut = (e) => {
        localStorage.removeItem('access_token');
        navigate("/");
    }

    const open = Boolean(props.anchor);
    const id = open ? 'simple-popover' : undefined;

    return (
        <Popover
            id={id}
            open={open}
            anchorEl={props.anchor}
            onClose={props.handleClose}
            anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'left',
            }}
            transformOrigin={{
                vertical: 'top',
                horizontal: 'left',
            }}
        >
            <Typography sx={{ p: 2 }} onClick={ handleSignOut }>Выйти</Typography>
        </Popover>
    );
}

export default function HeaderProfile() {
    const [anchorEl, setAnchorEl] = React.useState(null);

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const userData = {
        name: localStorage.getItem('full_name'),
        position: localStorage.getItem('role'),
        photo: false
    };

    return (
        <div className={styles.wrapper}>
            <Link to="/notifications">
                <img src={Bell} alt="" />
            </Link>
            <div className={styles.profile}>
                <div className={styles.photoWrapper}>
                    <img className={styles.photo} src={userData.photo ? userData.photo : Placeholder} alt="" />
                </div>
                <div className={styles.profileText} onClick={handleClick}>
                    <span className={styles.name}>{userData.name}</span>
                    <span className={styles.position}>{userData.position}</span>
                </div>
            </div>
            <BasicPopover anchor={anchorEl} setAnchor={setAnchorEl} handleClick={handleClick} handleClose={handleClose} />
        </div>
    );

}
