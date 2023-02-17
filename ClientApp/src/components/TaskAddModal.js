import * as React from 'react';
import Button from '@mui/material/Button';
import DialogTitle from '@mui/material/DialogTitle';
import Drawer from '@mui/material/Drawer';
import FormGroup from '@mui/material/FormGroup';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import BasicSelect from './BasicSelect';

const fields = [
    {
        "type": "datefield",
        "name": "date",
        "title": "Дата",
        "value": "01/01/2001"
    },
    {
        "type": "textfield",
        "name": "taskCode",
        "title": "Шифр задачи",
        "value": "Шифр задачи"
    },
    {
        "type": "textfield",
        "name": "projectCode",
        "title": "Шифр проекта",
        "value": "48/21 - ТСП"
    },
    {
        "type": "textfield",
        "name": "relateTo",
        "title": "Относится к",
        "value": " "
    },
    {
        "type": "textfield",
        "name": "name",
        "title": "Формулировка",
        "value": "Прозвонить Копьева"
    },
    {
        "type": "textfield",
        "name": "comment",
        "title": "Комментарий",
        "value": "Менщиков А.И.: Копъев переобулся. Теперь ему нужны распечатанные тома в 2 экз. Печати поставит и вернет согласованные. Отдал Светлане на печать."
    },
    {
        "type": "textfield",
        "name": "responsible",
        "title": "Ответственный",
        "value": "Мочалов Александр Николаевич"
    },
    {
        "type": "textfield",
        "name": "reassigned",
        "title": "Исполнитель",
        "value": "Менщиков Андрей Игоревич"
    },
    {
        "type": "textfield",
        "name": "planTime",
        "title": "Планируемое время исполнения",
        "value": "00:15:00"
    }
];

function getCurrentDate(separator = '') {

    let newDate = new Date()
    let date = newDate.getDate();
    let month = newDate.getMonth() + 1;
    let year = newDate.getFullYear();

    return `${year}${separator}${month < 10 ? `0${month}` : `${month}`}${separator}${date}`
}

function SimpleDialog(props) {
    const { onClose, selectedValue, open } = props;

    const handleClose = () => {
        onClose(selectedValue);
    };

    return (
        <Drawer anchor="right" onClose={handleClose} open={open}>
            <DialogTitle>Создание задачи</DialogTitle>
            <Box sx={{
                padding: 2,
                minWidth: "400px"
            }}>

                <FormGroup>
                    <Stack spacing={2}>
                        <BasicSelect />
                        {fields.map((field) =>
                            <>
                                {field.type === "datefield" &&
                                    <TextField
                                        id={field.name}
                                        label={field.title}
                                        type="date"
                                        defaultValue={getCurrentDate()}
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                    />
                                }
                                {field.type === "textfield" &&
                                    <TextField id={field.name} label={field.title} variant="outlined" multiline />
                                }
                               
                            </>
                        )}
                        <Button variant="contained" onClick={handleClose}>Сохранить</Button>
                    </Stack>
                </FormGroup>

            </Box>
        </Drawer>
    );
}

export default function TaskAddModal(props) {

    const { children } = props;

    const [open, setOpen] = React.useState(false);

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = (value) => {
        setOpen(false);
    };

    return (
        <>
            <Button style={{ minWidth: 180 }} variant="contained" onClick={handleClickOpen}>Добавить&nbsp;задачу</Button>
            <SimpleDialog
                open={open}
                onClose={handleClose}
            />
        </>
    );
}
