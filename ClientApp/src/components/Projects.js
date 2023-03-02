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

function ProjSelect(props) {
    return (
        <FormControl fullWidth>
            <InputLabel id="task-simple-select-label">Проекты</InputLabel>
            <Select
                labelId="task-simple-select-label"
                id="task-simple-select"
                value={props.value}
                label="Проекты"
                onChange={props.handleChange}
            >
                <MenuItem value={0}>Все проекты</MenuItem>
                {props.data.map((post, index) =>
                    <MenuItem key={index} value={post}>{post}</MenuItem>
                )
                }
            </Select>
        </FormControl>
    );
}

function GipSelect(props) {
    return (
        <FormControl fullWidth>
            <InputLabel id="position-simple-select-label">ГИПы</InputLabel>
            <Select
                labelId="position-simple-select-label"
                id="position-simple-select"
                value={props.value}
                label="ГИПы"
                onChange={props.handleChange}
            >
                <MenuItem value={0}>Все ГИПы</MenuItem>
                {props.data.map((post, index) =>
                    <MenuItem key={index} value={post}>{post}</MenuItem>
                )
                }
            </Select>
        </FormControl>
    );
}

function RecipientSelect(props) {
    return (
        <FormControl fullWidth>
            <InputLabel id="employees-simple-select-label">Ответственные</InputLabel>
            <Select
                labelId="employees-simple-select-label"
                id="employees-simple-select"
                value={props.value}
                label="Ответственные"
                onChange={props.handleChange}
            >
                <MenuItem value={0}>Все ответственные</MenuItem>
                {props.data.map((role, index) =>
                    <MenuItem key={index} value={role}>{role}</MenuItem>
                )
                }
            </Select>
        </FormControl>
    );
}


export class Projects extends Component {
    static displayName = Projects.name;

    constructor(props) {
        super(props);
        this.state = {
            tasks: {
                done: [],
                today: [],
                upcoming: [],
                staffs: [],
                filters: {
                    filterProj: [],
                    filterGip: [],
                    filterResipirnt: []
                }
            },
            headers: [],
            loading: true,
            projectCode: [],
            supervisor: [],
            gips: [],
            filter: {
                filterProj: 0,
                filterGip: 0,
                filterResipirnt: 0
            },
        };

        this.handleProjFilterChange = this.handleProjFilterChange.bind(this);
        this.handleGipFilterChange = this.handleGipFilterChange.bind(this);
        this.handleResipirntFilterChange = this.handleResipirntFilterChange.bind(this);
    }

    handleProjFilterChange(event) {
        console.log('event is ', event.target.value);
        let newState = this.state.filter;
        newState.filterProj = event.target.value;
        this.setState({ filter: newState });
        this.populateWeatherData(newState);
    };

    handleGipFilterChange(event) {
        console.log('event is ', event.target.value);
        let newState = this.state.filter;
        newState.filterGip = event.target.value;
        this.setState({ filter: newState });
        this.populateWeatherData(newState);
    };

    handleResipirntFilterChange(event) {
        console.log('event is ', event.target.value);
        let newState = this.state.filter;
        newState.filterResipirnt = event.target.value;
        this.setState({ filter: newState });
        this.populateWeatherData(newState);
    };

    componentDidMount() {
        this.populateWeatherData();
        this.getEmployees();
        this.getGIPs();
    }

    async getEmployees() {
        const response = await fetchWithAuth("/api/GetEmployees");
        console.log(response);
        if (response.status === 401) {
            this.setState({
                auth: false
            })
        }

        const data = response.value.staffs;
        this.setState({ supervisor: data });
    }

    async getGIPs() {
        const response = await fetchWithAuth("/api/GetEmployees?filterStaff=GIP");
        console.log(response);
        if (response.status === 401) {
            this.setState({
                auth: false
            })
        }

        const data = response.value.staffs;
        this.setState({ gips: data });
    }

    static renderTasksTable(headers, tasks, supervisor, projectCode, gips) {
        console.log(tasks);

        return (
            <CollapsibleTable gips={gips} tasks={tasks} headers={headers} supervisor={supervisor} projectCode={projectCode} recipient={supervisor} />
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Загрузка данных...</em></p>
            : Projects.renderTasksTable(this.state.headers, this.state.tasks, this.state.supervisor, this.state.projectCode, this.state.gips);

        return (
            <div>
                <h1>Проекты</h1>
                <Box sx={{
                    paddingTop: 2,
                    paddingBottom: 2
                }}>
                    <Stack spacing={2} direction="row">
                        <TaskAddModal type="1" title="Добавить&nbsp;задачу" headers={this.state.headers} supervisor={this.state.supervisor} projectCode={this.state.projectCode} recipient={this.state.supervisor} />
                        <TaskAddModal type="2" title="Добавить&nbsp;проект" headers={this.state.headers} supervisor={this.state.supervisor} projectCode={this.state.projectCode} recipient={this.state.supervisor} />
                        <ProjSelect value={this.state.filter.filterProj} handleChange={this.handleProjFilterChange} data={this.state.tasks.filters.filterProj} />
                        <GipSelect value={this.state.filter.filterGip} handleChange={this.handleGipFilterChange} data={this.state.tasks.filters.filterGip} />
                        <RecipientSelect value={this.state.filter.filterResipirnt} handleChange={this.handleResipirntFilterChange} data={this.state.tasks.filters.filterResipirnt} />
                    </Stack>
                </Box>
                {contents}
            </div>
        );
    }

    async populateWeatherData(filter = undefined) {
        let newFilter = {};
        for (let prop in filter) {
            if (filter.hasOwnProperty(prop) && filter[prop] !== 0) {
                newFilter[prop] = filter[prop]
            }
        }
        let url = "/api/GetProjects";
        if (newFilter) {
            url += "?" + new URLSearchParams(newFilter).toString();
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
