import React, { Component } from 'react';
import styles from './Layout.module.scss';
import { Header } from './Header';
import { Footer } from './Footer';
import { SideMenu } from './SideMenu';
import Container from "@mui/material/Container";
import { Outlet } from 'react-router-dom';
import { Unauthorized }  from "../utils";

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <>
                <div>
                    <Unauthorized auth="true"/>
                    <Header />
                    <Container className={styles.container} maxWidth="xl">
                        <SideMenu />
                        <main className={styles.main}>
                            <Container maxWidth="xl">
                                <Outlet />
                            </Container>

                        </main>
                    </Container>
                    <Footer />
                </div>

            </>
        );
    }
}
