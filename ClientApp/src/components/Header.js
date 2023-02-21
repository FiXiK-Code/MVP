import React, { Component } from 'react';
import { SideMenu } from './SideMenu';
import { Logo } from "./Logo";
import { HeaderProfile } from "./HeaderProfile";
import Container from "@mui/material/Container";
import styles from './Header.module.scss';

export const Header = (props) => (
    <>
        <header className={styles.header }>
            <Container maxWidth="xl" className={styles.container}>
                <Logo />
                <HeaderProfile />
            </Container>
        </header>
    </>
)

