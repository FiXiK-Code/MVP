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
                <MenuItem value={10}>Должность 1</MenuItem>
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
            : Employees.renderTasksTable(this.state.headers, this.state.tasks);

        return (
            <div>
                <h1>Сотрудники</h1>
                <Box sx={{
                    paddingTop: 2,
                    paddingBottom: 2
                }}>
                    <Stack spacing={2} direction="row">
                        <TaskAddModal />
                        <Button style={{minWidth: 180}} variant="contained">Добавить&nbsp;проект</Button>
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
        const response = await fetch('weatherforecast');
        const data = await response.json();
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
                "title": "Дата"
            },
            {
                "name": "taskCode",
                "title": "Шифр задачи"
            },
            {
                "name": "projectCode",
                "title": "Шифр проекта"
            },
            {
                "name": "name",
                "title": "Задача"
            },
            {
                "name": "status",
                "title": "Статус"
            },
            {
                "name": "responsible",
                "title": "Ответственный"
            },
            {
                "name": "reassign",
                "title": "Переназначить"
            },
            {
                "name": "priority",
                "title": "Приоритет"
            },
            {
                "name": "comment",
                "title": "Комментарий"
            },
            {
                "name": "planTime",
                "title": "План время"
            },
            {
                "name": "factTime",
                "title": "Факт время"
            },
            {
                "name": "dateOfStart",
                "title": "Начал"
            },
            {
                "name": "dateOfEnd",
                "title": "Завершил"
            },
        ];
        this.setState({ tasks: testData, headers: headers, loading: false });
    }
}
