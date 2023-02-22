import React, { Component } from 'react';
import CollapsibleTable from './CollapsibleTable';
import { Search } from './Search';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import TaskAddModal from './TaskAddModal';
import { fetchWithAuth, Unauthorized, getHeaders } from '../utils';

export class Tasks extends Component {
    static displayName = Tasks.name;

    constructor(props) {
        super(props);
        this.state = {
            auth: true,
            tasks: {
                done: [],
                today: [],
                upcoming: [],
            },
            searchTerm: "",
            search: false,
            headers: [],
            loading: true,
            projectCode: [],
            supervisor: []
        };

        this.renderTasksTable = this.renderTasksTable.bind(this);
        this.searchHandleInput = this.searchHandleInput.bind(this);
        this.searchHandleSubmit = this.searchHandleSubmit.bind(this);
    }

    shouldComponentUpdate(nextProps, nextState) {
        if (nextState.searchTerm !== this.state.searchTerm) {
            return false;
        }
        return true;
    }

    componentDidMount() {
        this.populateWeatherData();
        this.getEmployees();
        this.getProjects();
    }

    renderTasksTable(headers, tasks, supervisor, projectCode ) {
        return (
            <CollapsibleTable search={this.state.search} tasks={tasks} headers={headers} supervisor={supervisor} projectCode={projectCode} recipient={supervisor }  />
        );
    }

    async searchHandleSubmit(e) {
        e.preventDefault();
        
        let term = this.state.searchTerm;
        if (term.length > 0) {
            const response = await fetchWithAuth(`/api/GetSearch?param=${encodeURIComponent(term)}`);
            if (response.statusCode === 200) {
                this.setState({
                    tasks: {
                        done: response.value,
                        today: [],
                        upcoming: []
                    },
                    search: this.state.searchTerm
                })
                console.log(response);
            }
        } else {
            console.log('empty!');
            this.setState({
                    search: false
            })
            this.populateWeatherData();
        }


    }

    searchHandleInput(e) {
        this.setState({
            searchTerm: e.target.value
        });
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Загрузка данных...</em></p>
            : this.renderTasksTable(this.state.headers, this.state.tasks, this.state.supervisor, this.state.projectCode);

        const tableSettingsHandler = (event) => {
            console.log(event.target.value);
            console.log(event.target.checked);
            console.log(event.target.name);
            let newState = this.state;
            newState.headers[event.target.name].show = event.target.checked;
            this.setState(newState);
        }

        return (
            <div>
                <Unauthorized auth={this.state.auth} />
                <h1>Задачи</h1>
                <Search handleInput={this.searchHandleInput} handleSubmit={this.searchHandleSubmit} tableSettingsHandler={tableSettingsHandler} headers={this.state.headers} />
                <Box sx={{
                    paddingTop: 2,
                    paddingBottom: 2
                }}>
                    <Stack spacing={2} direction="row">
                        <TaskAddModal headers={this.state.headers} supervisor={this.state.supervisor} projectCode={this.state.projectCode} recipient={this.state.supervisor }  />
                        <Button variant="contained">Добавить проект</Button>
                    </Stack>
                </Box>
                {contents}
            </div>
        );
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

    async populateWeatherData() {
        const response = await fetchWithAuth("/api/GetTasks");
        console.log(response);
        if (response.status === 401) {
            this.setState({
                auth: false
            })
        }

        const data = response.value;
        const headers = getHeaders();
        this.setState({ tasks: data, headers: headers, loading: false });
    }
}
