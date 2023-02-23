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

    return `${year}${separator}${month < 10 ? `0${month}` : `${month}`}${separator}${date}`
}

export function getHeaders() {
    return [
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
            "createAvailability": true,
            "fieldToShow": "name"
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
            "name": "dedline",
            "type": "datetime",
            "title": "Дедлайн",
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
}

export function getProjectHeaders() {
    return [
        {
            "name": "plannedFinishDate",
            "type": "datetime",
            "title": "Планируемая дата завершения",
            "show": true,
            "createAvailability": true
        },
        {
            "name": "link",
            "type": "textfield",
            "title": "Облако",
            "show": true,
            "createAvailability": true
        },
        {
            "name": "code",
            "type": "textfield",
            "title": "Шифр проекта",
            "show": true,
            "createAvailability": true
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
            "name": "priority",
            "type": "number",
            "title": "Приоритет",
            "show": true,
            "createAvailability": true
        },
        {
            "name": "shortName",
            "type": "textfield",
            "title": "Краткое описание",
            "show": true,
            "createAvailability": true
        },
        {
            "name": "name",
            "type": "textfield",
            "title": "Полное описание",
            "show": true,
            "createAvailability": true
        },
        {
            "name": "allStages",
            "type": "textfield",
            "title": "Стадии проекта (через запятую)",
            "show": true,
            "createAvailability": true
        },
    ];
}
