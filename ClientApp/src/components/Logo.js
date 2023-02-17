import React, { Component } from 'react';
import styles from "./Logo.module.scss";
import { LogoImg } from "../img/svg/logo.svg";
import { Link } from 'react-router-dom';


export class Logo extends Component {

    render() {
        return (
            <Link className={styles.link} to="/">
                <div className={styles.wrapper}>
                    <img className={styles.img} src={LogoImg} alt="" />
                    <span className={ styles.text }>ТехноСтройПроект</span>
                </div>
            </Link>
        );
    }
}
