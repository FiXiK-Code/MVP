import * as React from 'react';
import { useNavigate } from "react-router-dom";

export const Unauthorized = (props) => {
    const { auth } = props;

    let navigate = useNavigate();

    React.useEffect(() => {
        if (!localStorage.getItem('access_token') || !auth) {
            localStorage.removeItem('access_token');
            localStorage.removeItem('username');
            navigate("/");
        }
    }, []);

    return (
        <></>
    )

}

export function fetchWithAuth(url, method = "get", body = false) {
    const token = localStorage.getItem('access_token');
    let request = {
        method: method,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + token
        }
    }
    if (body) request.body = JSON.stringify(body);
    return fetch(url, request)
        .then((response) => {
            if (response.status !== 200) {
                if (response.status === 401) {
                    console.log('401');
                    localStorage.removeItem('access_token');
                    localStorage.removeItem('username');
                    // redirect("/");
                    return response;
                }
                throw new Error(response.status)
            }
            return response.json()
        });
}

export function getCurrentDate(separator = '') {

    let newDate = new Date()
    let date = newDate.getDate();
    let month = newDate.getMonth() + 1;
    let year = newDate.getFullYear();

    return `${year}${separator}${month < 10 ? `0${month}` : `${month}`}${separator}${date < 10 ? `0${date}` : `${date}`}`
}

export function getHeaders() {
    let storage = JSON.parse(localStorage.getItem('tableSettings'));
    console.log(storage);

    if (!storage) {
        storage = [];
        for (let i = 0; i < 13; i++) {
            storage[i] = {};
            storage[i].show = true;
        }
    }

    return [
        {
            "name": "date",
            "type": "datefield",
            "title": "????????",
            "show": storage[0].show,
            "header": true,
            "createAvailability": true,
            "rowData": "dateRaw"
        },
        {
            "name": "projectCode",
            "type": "selectWithSearch",
            "title": "???????? ??????????????",
            "show": storage[1].show,
            "header": true,
            "createAvailability": true,
            "fieldToShow": "code",
            "rowData": "projectId"
        },
        {
            "name": "desc",
            "type": "textfield",
            "title": "????????????",
            "show": storage[2].show,
            "header": true,
            "createAvailability": true,
            "rowData": "desc"
        },
        {
            "name": "status",
            "title": "????????????",
            "show": storage[3].show,
            "header": true,
            "createAvailability": false,
            "rowData": "status"
        },
        {
            "name": "supervisor",
            "type": "select",
            "title": "??????????????????????????",
            "show": storage[4].show,
            "header": true,
            "createAvailability": true,
            "fieldToShow": "name",
            "rowData": "supervisorId"
        },
        {
            "name": "recipient",
            "type": "select",
            "title": "??????????????????????????",
            "show": storage[5].show,
            "header": true,
            "createAvailability": true,
            "fieldToShow": "name",
            "rowData": "recipientId"
        },
        {
            "name": "priority",
            "title": "??????????????????",
            "type": "number",
            "show": storage[6].show,
            "header": true,
            "createAvailability": false,
        },
        {
            "name": "liteTask",
            "title": "??????????????????",
            "type": "select",
            "header": false,
            "createAvailability": true,
            "fieldToShow": "value"
        },
        {
            "name": "comment",
            "type": "textfield",
            "title": "??????????????????????",
            "show": storage[7].show,
            "header": true,
            "createAvailability": true,
            "rowData": "comment"
        },
        {
            "name": "dedline",
            "type": "datetime",
            "title": "??????????????",
            "show": storage[8].show,
            "header": true,
            "createAvailability": true,
            "rowData": "dedlineRaw"
        },
        {
            "name": "plannedTime",
            "type": "timefield",
            "title": "???????? ??????????",
            "show": storage[9].show,
            "header": true,
            "createAvailability": true,
            "rowData": "plannedTime"
        },
        {
            "name": "actualTime",
            "type": "textfield",
            "title": "???????? ??????????",
            "show": storage[10].show,
            "header": true,
            "createAvailability": false,
            "rowData": "actualTime"
        },
        {
            "name": "start",
            "type": "datetime",
            "title": "??????????",
            "show": storage[11].show,
            "header": true,
            "createAvailability": false,
            "rowData": "startRaw"
        },
        {
            "name": "finish",
            "type": "datetime",
            "title": "????????????????",
            "show": storage[12].show,
            "header": true,
            "createAvailability": false,
            "rowData": "finishRaw"
        },
    ];
}

export function getProjectHeaders() {
    return [
        {
            "name": "plannedFinishDate",
            "type": "datetime",
            "title": "?????????????????????? ???????? ????????????????????",
            "show": true,
            "createAvailability": true,
            "rowData": "plannedFinishDateRaw"
        },
        {
            "name": "link",
            "type": "textfield",
            "title": "????????????",
            "show": true,
            "createAvailability": true,
            "rowData": "link"
        },
        {
            "name": "code",
            "type": "textfield",
            "title": "???????? ??????????????",
            "show": true,
            "createAvailability": true,
            "rowData": "code"
        },
        {
            "name": "supervisor",
            "type": "select",
            "title": "??????????????????????????",
            "show": true,
            "createAvailability": true,
            "fieldToShow": "name",
            "rowData": "supervisorId"
        },
        {
            "name": "priority",
            "type": "number",
            "title": "??????????????????",
            "show": true,
            "createAvailability": true,
            "rowData": "priority"
        },
        {
            "name": "shortName",
            "type": "textfield",
            "title": "?????????????? ????????????????",
            "show": true,
            "createAvailability": true,
            "rowData": "shortName"
        },
        {
            "name": "name",
            "type": "textfield",
            "title": "???????????? ????????????????",
            "show": true,
            "createAvailability": true,
            "rowData": "name"
        },
        {
            "name": "allStages",
            "type": "textfield",
            "title": "???????????? ?????????????? (?????????? ??????????????)",
            "show": true,
            "createAvailability": true,
            "rowData": "allStages"
        },
        {
            "name": "archive",
            "type": "select",
            "title": "??????????",
            "show": true,
            "createAvailability": false,
            "rowData": "archive",
            "fieldToShow": "name"
        },
    ];
}

export function showLocaleDate(dateString, onlyDate = false) {
    let date;
    if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
        date = new Date(dateString);
    } else {
        date = new Date(dateString + "Z");
    }
    return date.toLocaleDateString() + (!onlyDate ? (" " + date.toLocaleTimeString()) : "");
}

export function setLocaleDateInTasks(data) {
    data.completed.map(task => {
        task.priorityRaw = task.priority;
        if (task.priority === -1) {
            task.priority = "?????? ??????????????";
        }
        task.dateRaw = task.date;
        task.date = showLocaleDate(task.date, true);
        task.dedlineRaw = task.dedline;
        task.dedline = showLocaleDate(task.dedline);
        task.startRaw = task.start;
        task.start = showLocaleDate(task.start);
        task.finishRaw = task.finish;
        task.finish = showLocaleDate(task.finish);
        return task;
    });
    data.future.map(task => {
        task.priorityRaw = task.priority;
        if (task.priority === -1) {
            task.priority = "?????? ??????????????";
        }
        task.dateRaw = task.date;
        task.date = showLocaleDate(task.date, true);
        task.dedlineRaw = task.dedline;
        task.dedline = showLocaleDate(task.dedline);
        task.startRaw = task.start;
        task.start = showLocaleDate(task.start);
        task.finishRaw = task.finish;
        task.finish = showLocaleDate(task.finish);
        return task;
    });
    data.today.map(task => {
        task.priorityRaw = task.priority;
        if (task.priority === -1) {
            task.priority = "?????? ??????????????";
        }
        task.dateRaw = task.date;
        task.date = showLocaleDate(task.date, true);
        task.dedlineRaw = task.dedline;
        task.dedline = showLocaleDate(task.dedline);
        task.startRaw = task.start;
        task.start = showLocaleDate(task.start);
        task.finishRaw = task.finish;
        task.finish = showLocaleDate(task.finish);
        return task;
    });
    if (data.projects) {
        data.projects.map(project => {
            project.plannedFinishDateRaw = project.plannedFinishDate;
            project.plannedFinishDate = showLocaleDate(project.plannedFinishDate, true);
            return project;
        });
    }
    return data;
}

export function getServerTimeFromLocale(dateString) {
    if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
        console.log(dateString);
        let date = new Date(dateString);
        date = date.getTime() - date.getTimezoneOffset() * 60 * 1000;
        date = new Date(date);
        console.log(date);
        return date.toISOString();

    } else {
        let value = new Date(dateString);
        return value.toISOString();
    }
}
