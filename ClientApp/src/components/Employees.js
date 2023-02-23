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
import { fetchWithAuth, Unauthorized, getHeaders } from '../utils';

function TaskSelect() {
    const [task, setTask] = React.useState(0);

    const handleChange = (event: SelectChangeEvent) => {
        setTask(event.target.value);
    };

    return (
        <FormControl fullWidth>
            <InputLabel id="demo-simple-select-label">Задачи</InputLabel>
            <Select
                labelId="demo-simple-select-label"
                id="demo-simple-select"
                value={task}
                label="Задачи"
                onChange={handleChange}
            >
                <MenuItem value={0}>Все задачи</MenuItem>
                <MenuItem value={10}>Задача 1</MenuItem>
                <MenuItem value={20}>Задача 2</MenuItem>
            </Select>
        </FormControl>
    );
}

function PositionSelect() {
    const [position, setPosition] = React.useState(0);

    const handleChange = (event: SelectChangeEvent) => {
        setPosition(event.target.value);
    };

    return (
        <FormControl fullWidth>
            <InputLabel id="demo-simple-select-label">Должности</InputLabel>
            <Select
                labelId="demo-simple-select-label"
                id="demo-simple-select"
                value={position}
                label="Должности"
                onChange={handleChange}
            >
                <MenuItem value={0}>Все должности</MenuItem>
                <MenuItem value={10}>Все </MenuItem>
                <MenuItem value={20}>Должность 2</MenuItem>
            </Select>
        </FormControl>
    );
}

function EmployeeSelect() {
    const [employee, setEmployee] = React.useState(0);

    const handleChange = (event: SelectChangeEvent) => {
        setEmployee(event.target.value);
    };

    return (
        <FormControl fullWidth>
            <InputLabel id="demo-simple-select-label">Сотрудники</InputLabel>
            <Select
                labelId="demo-simple-select-label"
                id="demo-simple-select"
                value={employee}
                label="Сотрудники"
                onChange={handleChange}
            >
                <MenuItem value={0}>Все сотрудники</MenuItem>
                <MenuItem value={10}>Сотрудник 1</MenuItem>
                <MenuItem value={20}>Сотрудник 2</MenuItem>
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
                staffs: []
            },
            headers: [],
            loading: true,
            projectCode: [],
            supervisor: []
        };
    }

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

    static renderTasksTable(headers, tasks) {
        console.log(tasks);

        return (
            <CollapsibleTable tasks={tasks} headers={headers} />
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Загрузка данных...</em></p>
            : Employees.renderTasksTable(this.state.headers, this.state.tasks);

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
                        <TaskSelect />
                        <PositionSelect />
                        <EmployeeSelect />
                    </Stack>
                </Box>
                {contents}
            </div>
        );
    }

    async populateWeatherData() {
        const response = await fetchWithAuth("/api/GetEmployees");
        console.log(response);
        if (response.status === 401) {
            this.setState({
                auth: false
            })
        }

        const data = response.value;
        const headers = getHeaders();
        this.setState({ tasks: data, supervisor: data.staffs, headers: headers, loading: false });
    }
}
