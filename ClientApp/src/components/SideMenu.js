import React, { Component } from 'react';
import styles from './SideMenu.module.scss';
import { NavLink } from 'react-router-dom';
import TaskIcon from '../img/svg/tasks.svg';
import EmployeeIcon from '../img/svg/employee.svg';
import ProjectIcon from '../img/svg/project.svg';
import PercentIcon from '../img/svg/percent.svg';
import StatisticIcon from '../img/svg/statistic.svg';
import SettingsIcon from '../img/svg/settings.svg';

const activeClassName = styles.link + " " + styles.active;
const passiveClassName = styles.link;

export class SideMenu extends Component {

    render() {
        return (
            <aside className={styles.aside}>
                <nav>
                    <ul>
                        <li>
                            <NavLink className={({ isActive }) =>
                                isActive ? activeClassName : passiveClassName
                            } to="/tasks">
                                <img className={styles.icon} src={TaskIcon} alt="" />
                            </NavLink>
                        </li>
                        <li>
                            <NavLink className={({ isActive }) =>
                                isActive ? activeClassName : passiveClassName
                            } to="/employees" >
                                <img className={styles.icon} src={EmployeeIcon} alt="" />
                            </NavLink>
                        </li>
                        <li>
                            <NavLink className={({ isActive }) =>
                                isActive ? activeClassName : passiveClassName
                            } to="/projects">
                                <img className={styles.icon} src={ProjectIcon} alt="" />
                            </NavLink>
                        </li>
                        <li>
                            <NavLink className={({ isActive }) =>
                                isActive ? activeClassName : passiveClassName
                            } to="/page1">
                                <img className={styles.icon} src={PercentIcon} alt="" />
                            </NavLink>
                        </li>
                        <li>
                            <NavLink className={({ isActive }) =>
                                isActive ? activeClassName : passiveClassName
                            } to="/statistic">
                                <img className={styles.icon} src={StatisticIcon} alt="" />
                            </NavLink>
                        </li>
                        <li>
                            <NavLink className={({ isActive }) =>
                                isActive ? activeClassName : passiveClassName
                            } to="/settings">
                                <img className={styles.icon} src={SettingsIcon} alt="" />
                            </NavLink>
                        </li>
                    </ul>
                </nav>
            </aside>
        );
    }
}
