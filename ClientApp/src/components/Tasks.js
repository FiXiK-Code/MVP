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
            loading: true
        };

        this.renderTasksTable = this.renderTasksTable.bind(this);
        this.searchHandleInput = this.searchHandleInput.bind(this);
        this.searchHandleSubmit = this.searchHandleSubmit.bind(this);
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    renderTasksTable(headers, tasks) {
        console.log("Render tasks: ", tasks);

        return (
            <CollapsibleTable search={ this.state.search } tasks={tasks} headers={headers} />
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
            : this.renderTasksTable(this.state.headers, this.state.tasks);

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
                        <TaskAddModal />
                        <Button variant="contained">Добавить проект</Button>
                    </Stack>
                </Box>
                {contents}
            </div>
        );
    }

    async populateWeatherData() {
        const response = await fetchWithAuth("/api/GetTasks");
        console.log(response);
        if (response.status === 401) {
            this.setState({
                auth: false
            })
        }

        const data = JSON.parse(response.value);
        const headers = [
            {
                "name": "date",
                "title": "Дата",
                "show": true
            },
            {
                "name": "projectCode",
                "title": "Шифр проекта",
                "show": true
            },
            {
                "name": "desc",
                "title": "Задача",
                "show": true
            },
            {
                "name": "status",
                "title": "Статус",
                "show": true
            },
            {
                "name": "supervisor",
                "title": "Ответственный",
                "show": true
            },
            {
                "name": "recipient",
                "title": "Переназначить",
                "show": true
            },
            {
                "name": "priority",
                "title": "Приоритет",
                "show": true
            },
            {
                "name": "comment",
                "title": "Комментарий",
                "show": true
            },
            {
                "name": "plannedTime",
                "title": "План время",
                "show": true
            },
            {
                "name": "actualTime",
                "title": "Факт время",
                "show": true
            },
            {
                "name": "start",
                "title": "Начал",
                "show": true
            },
            {
                "name": "finish",
                "title": "Завершил",
                "show": true
            },
        ];
        this.setState({ tasks: data, headers: headers, loading: false });
    }
}
