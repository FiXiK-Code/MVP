import React, { Component } from 'react';
import { createTheme } from '@mui/material/styles';
import { ThemeProvider, LocalizationProvider } from '@mui/material/styles';

import {
    createBrowserRouter,
    RouterProvider,
} from "react-router-dom";
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import SignIn from './components/SignIn';
import { Tasks } from './components/Tasks';
import { Employees } from './components/Employees';
import { Projects } from './components/Projects';
import './custom.css';
import './main.scss';

const router = createBrowserRouter([
    {
        element: <Layout />,
        children: [
            {
                path: "/counter",
                element: <Counter />,
            },
            {
                path: "/fetch-data",
                element: <FetchData />,
            },
            {
                path: "/sign-in",
                element: <SignIn />,
            },
            {
                path: "/tasks",
                element: <Tasks />,
            },
            {
                path: "/employees",
                element: <Employees />,
            },
            {
                path: "/projects",
                element: <Projects />,
            },
        ],
    },
    {
        path: "/",
        element: <SignIn />
    },
]);

const theme = createTheme({
    typography: {
        fontFamily: [
            'Mulish',
            'Roboto',
            '"Helvetica Neue"',
            'Arial',
            'sans-serif'
        ].join(','),
    }
});

export default class App extends Component {
    static displayName = App.name;



    render() {
        return (

            <ThemeProvider theme={theme}>
                <RouterProvider router={router} />
            </ThemeProvider>


        );
    }
}
