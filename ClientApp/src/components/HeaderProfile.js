import React, { Component } from 'react';
import Bell from '../img/svg/bell.svg';
import styles from './HeaderProfile.module.scss';
import { Link } from 'react-router-dom';
import Placeholder from '../img/svg/profile.svg';

const userData = {
    name: "Григорьев В.А.",
    position: "ГИП",
    photo: false
};

export class HeaderProfile extends Component {

    render() {
        return (
            <div className={styles.wrapper}>
                <Link to="/notifications">
                    <img src={Bell} alt="" />
                </Link>
                <div className={styles.profile}>
                    <div className={styles.photoWrapper}>
                        <img className={styles.photo} src={userData.photo ? userData.photo : Placeholder} alt="" />
                    </div>
                    <div className={styles.profileText}>
                        <span className={styles.name}>{userData.name}</span>
                        <span className={styles.position}>{userData.position}</span>
                    </div>
                </div>
            </div>
        );
    }
}
