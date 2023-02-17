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
        this.state = {
            auth: true,
            tasks: {
                done: [],
                today: [],
                upcoming: [],
            },
            headers: [],
            loading: true
        };
    }

    componentDidMount() {
        this.populateWeatherData();
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
            : Tasks.renderTasksTable(this.state.headers, this.state.tasks);

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
                <Search tableSettingsHandler={tableSettingsHandler} headers={this.state.headers} />
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
        const testData = {
            "done": [
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "0",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                }
            ],
            "today": [
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "0",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                }
            ],
            "upcoming": [
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                },
                {
                    date: "01.01.2001",
                    taskCode: "Шифр задачи",
                    projectCode: "48/21 - ТСП",
                    name: "Прозвонить Копьева",
                    statusCode: 3,
                    statusName: "Выполнена",
                    responsible: "Мочалов А. Н.",
                    reassign: "Менщиков А. И.",
                    priority: "2",
                    comment: "Комментарий",
                    planTime: "00:15:00",
                    factTime: "00:00:14",
                    dateOfStart: "01/01/2001 00:00:00",
                    dateOfEnd: "01/24/2023 10:23:17"
                }
            ]
        };
        const headers = [
            {
                "name": "date",
                "title": "Дата",
                "show": true
            },
            {
                "name": "taskCode",
                "title": "Шифр задачи",
                "show": true
            },
            {
                "name": "projectCode",
                "title": "Шифр проекта",
                "show": true
            },
            {
                "name": "name",
                "title": "Задача",
                "show": true
            },
            {
                "name": "status",
                "title": "Статус",
                "show": true
            },
            {
                "name": "responsible",
                "title": "Ответственный",
                "show": true
            },
            {
                "name": "reassign",
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
                "name": "planTime",
                "title": "План время",
                "show": true
            },
            {
                "name": "factTime",
                "title": "Факт время",
                "show": true
            },
            {
                "name": "dateOfStart",
                "title": "Начал",
                "show": true
            },
            {
                "name": "dateOfEnd",
                "title": "Завершил",
                "show": true
            },
        ];
        this.setState({ tasks: data, headers: headers, loading: false });
    }
}
