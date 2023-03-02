import React, { Component } from 'react';
import CollapsibleTable from './CollapsibleTable';
import { Search } from './Search';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import TaskAddModal from './TaskAddModal';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select, { SelectChangeEvent } from '@mui/material/Select';
import { fetchWithAuth, Unauthorized, getHeaders, setLocaleDateInTasks } from '../utils';


function TaskSelect(props) {
    return (
        <FormControl fullWidth>
            <InputLabel id="task-simple-select-label">Задачи</InputLabel>
            <Select
                labelId="task-simple-select-label"
                id="task-simple-select"
                value={props.value}
                label="Задачи"
                onChange={props.handleChange}
            >
                <MenuItem value={"Мои задачи"}>Мои задачи</MenuItem>
                <MenuItem value={"Все задачи"}>Все задачи</MenuItem>
            </Select>
        </FormControl>
    );
}

function PositionSelect(props) {
    return (
        <FormControl fullWidth>
            <InputLabel id="position-simple-select-label">Должности</InputLabel>
            <Select
                labelId="position-simple-select-label"
                id="position-simple-select"
                value={props.value}
                label="Должности"
                onChange={props.handleChange}
            >
                <MenuItem value={0}>Все должности</MenuItem>
                {props.data.map((post, index) =>
                    <MenuItem key={index} value={post}>{post}</MenuItem>
                )
                }
            </Select>
        </FormControl>
    );
}

function EmployeeSelect(props) {
    return (
        <FormControl fullWidth>
            <InputLabel id="employees-simple-select-label">Сотрудники</InputLabel>
            <Select
                labelId="employees-simple-select-label"
                id="employees-simple-select"
                value={props.value}
                label="Сотрудники"
                onChange={props.handleChange}
            >
                <MenuItem value={0}>Все сотрудники</MenuItem>
                {props.data.map((role, index) =>
                    <MenuItem key={index} value={role}>{role}</MenuItem>
                )
                }
            </Select>
        </FormControl>
    );
}


export class Employees extends Component {
    static displayName = Employees.name;

    constructor(props) {
        super(props);
        this.state = {
            tasks: {
                done: [],
                today: [],
                upcoming: [],
                staffs: [],
                filters: {
                    filterPosts: [],
                    filterStaffs: [],
                    filterTasks: []
                }
            },
            headers: [],
            loading: true,
            projectCode: [],
            supervisor: [],
            filter: {
                filterTasks: "Мои задачи",
                filterPosts: 0,
                filterStaffs: 0
            },
        };

        this.handleTaskFilterChange = this.handleTaskFilterChange.bind(this);
        this.handlePositionFilterChange = this.handlePositionFilterChange.bind(this);
        this.handleEmployeeFilterChange = this.handleEmployeeFilterChange.bind(this);
    }

    handleTaskFilterChange(event) {
        console.log('event is ', event.target.value);
        let newState = this.state.filter;
        newState.filterTasks = event.target.value;
        this.setState({ filter: newState });
        this.populateWeatherData(newState);
    };

    handlePositionFilterChange(event) {
        console.log('event is ', event.target.value);
        let newState = this.state.filter;
        newState.filterPosts = event.target.value;
        this.setState({ filter: newState });
        this.populateWeatherData(newState);
    };

    handleEmployeeFilterChange(event) {
        console.log('event is ', event.target.value);
        let newState = this.state.filter;
        newState.filterStaffs = event.target.value;
        this.setState({ filter: newState });
        this.populateWeatherData(newState);
    };

    componentDidMount() {
        this.populateWeatherData();
        this.getProjects();
    }

    async getProjects() {
        const response = await fetchWithAuth("/api/GetProjects");
        console.log(response);
        if (response.status === 401) {
            this.setState({
                auth: false
            })
        }

        const data = response.value.projects;
        this.setState({ projectCode: data });
    }

    static renderTasksTable(headers, tasks, supervisor, projectCode) {
        console.log(tasks);

        return (
            <CollapsibleTable tasks={tasks} headers={headers} supervisor={supervisor} projectCode={projectCode} recipient={supervisor} />
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Загрузка данных...</em></p>
            : Employees.renderTasksTable(this.state.headers, this.state.tasks, this.state.supervisor, this.state.projectCode);

        return (
            <div>
                <h1>Сотрудники</h1>
                <Box sx={{
                    paddingTop: 2,
                    paddingBottom: 2
                }}>
                    <Stack spacing={2} direction="row">
                        <TaskAddModal type="1" title="Добавить&nbsp;задачу" headers={this.state.headers} supervisor={this.state.supervisor} projectCode={this.state.projectCode} recipient={this.state.supervisor} />
                        <TaskAddModal type="2" title="Добавить&nbsp;проект" headers={this.state.headers} supervisor={this.state.supervisor} projectCode={this.state.projectCode} recipient={this.state.supervisor} />
                        <TaskSelect value={this.state.filter.filterTasks} handleChange={this.handleTaskFilterChange} data={this.state.tasks.filters.filterTasks} />
                        <PositionSelect value={this.state.filter.filterPosition} handleChange={this.handlePositionFilterChange} data={this.state.tasks.filters.filterPosts} />
                        <EmployeeSelect value={this.state.filter.filterStaffs} handleChange={this.handleEmployeeFilterChange} data={this.state.tasks.filters.filterStaffs} />
                    </Stack>
                </Box>
                {contents}
            </div>
        );
    }

    async populateWeatherData(filter = undefined) {
        let url = "/api/GetEmployees";
        if (filter) {
            url += "?" + new URLSearchParams(filter).toString();
        }
        const response = await fetchWithAuth(url);
        console.log(response);
        if (response.status === 401) {
            this.setState({
                auth: false
            })
        }

        let data = response.value;
        data = setLocaleDateInTasks(data);
        const headers = getHeaders();
        this.setState({ tasks: data, supervisor: data.staffs, headers: headers, loading: false });
    }
}
