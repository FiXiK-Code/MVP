import React, { Component } from 'react';
import CollapsibleTable from './CollapsibleTable';
import { Search } from './Search';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import TaskAddModal from './TaskAddModal';
import { fetchWithAuth, Unauthorized } from '../utils';

export class Tasks extends Component {
    static displayName = Tasks.name;

    constructor(props) {
        super(props);

        this.kek = 'yes';
        this.state = {
            auth: true,
            tasks: {
                done: [],
                today: [],
                upcoming: [],
            },
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

    componentDidMount() {
        this.populateWeatherData();
        this.getEmployees();
        this.getProjects();
    }

    renderTasksTable(headers, tasks, supervisor, projectCode ) {
        console.log("Render tasks: ", tasks);

        return (
            <CollapsibleTable search={this.state.search} tasks={tasks} headers={headers} supervisor={supervisor} projectCode={ projectCode }  />
        );
    }

    async searchHandleSubmit(e) {
        e.preventDefault();
        let term = this.state.search;
        if (term.length > 0) {
            const response = await fetchWithAuth(`/api/GetSearch?param=${encodeURIComponent(term)}`);
            if (response.statusCode === 200) {
                this.setState({
                    tasks: {
                        done: response.value,
                        today: [],
                        upcoming: []
                    }
                })
                console.log(response);
                console.log("tasks state: ", this.state);
                console.log(this.kek);
            }
        } else {
            this.populateWeatherData();
        }


    }

    searchHandleInput(e) {
        console.log(e.target.value)
        this.setState({
            search: e.target.value
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
                        <TaskAddModal headers={this.state.headers} supervisor={this.state.supervisor} projectCode={this.state.projectCode }  />
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
        const headers = [
            {
                "name": "date",
                "type": "datefield",
                "title": "Дата",
                "show": true,
                "createAvailability": true
            },
            {
                "name": "projectCode",
                "type": "select",
                "title": "Шифр проекта",
                "show": true,
                "createAvailability": true,
                "fieldToShow": "code",
            },
            {
                "name": "desc",
                "type": "textfield",
                "title": "Задача",              
                "show": true,
                "createAvailability": true
            },
            {
                "name": "status",
                "title": "Статус",
                "show": true,
                "createAvailability": false
            },
            {
                "name": "supervisor",
                "type": "select",
                "title": "Ответственный",
                "show": true,
                "createAvailability": true,
                "fieldToShow": "name"
            },
            {
                "name": "recipient",
                "type": "select",
                "title": "Переназначить",
                "show": true,
                "createAvailability": false
            },
            {
                "name": "priority",
                "title": "Приоритет",
                "show": true,
                "createAvailability": false
            },
            {
                "name": "comment",
                "type": "textfield",
                "title": "Комментарий",
                "show": true,
                "createAvailability": true
            },
            {
                "name": "plannedTime",
                "type": "timefield",
                "title": "План время",
                "show": true,
                "createAvailability": true
            },
            {
                "name": "actualTime",
                "title": "Факт время",
                "show": true,
                "createAvailability": false
            },
            {
                "name": "start",
                "title": "Начал",
                "show": true,
                "createAvailability": false
            },
            {
                "name": "finish",
                "title": "Завершил",
                "show": true,
                "createAvailability": false
            },
        ];
        this.setState({ tasks: data, headers: headers, loading: false });
    }
}
