import * as React from 'react';
import Button from '@mui/material/Button';
import DialogTitle from '@mui/material/DialogTitle';
import Dialog from '@mui/material/Dialog';
import TableCell from '@mui/material/TableCell';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';

const fields = [
    {
        "type": "textfield",
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
        "name": "status",
        "title": "Статус задачи",
        "value": "Выполнена"
    },
    {
        "type": "textfield",
        "name": "name",
        "title": "Формулировка",
        "value": "Прозвонить Копьева"
    },
    {
        "type": "textarea",
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
        "name": "priority",
        "title": "Приоритет",
        "value": "2"
    },
    {
        "type": "textfield",
        "name": "planTime",
        "title": "Планируемое время исполнения",
        "value": "00:15:00"
    },
    {
        "type": "textfield",
        "name": "dateOfStart",
        "title": "Начало работы над задачей",
        "value": "01/01/2001 00:00:00"
    },
    {
        "type": "textfield",
        "name": "dateOfEnd",
        "title": "Задача завершена",
        "value": "01/24/2023 10:23:17"
    },
];

function SimpleDialog(props) {
    const { onClose, selectedValue, open } = props;

    const handleClose = () => {
        onClose(selectedValue);
    };

    return (
        <Dialog onClose={handleClose} open={open} maxWidth="sm" fullWidth>
            <DialogTitle>Просмотр задачи</DialogTitle>
            <Box sx={{
                    padding: 2
                } }>
                <Stack spacing={2}>
                    <Button variant="contained">Редактировать</Button>
                    {fields.map((field) =>
                        <>
                            <TextField id={field.name} label={field.title} variant="outlined" disabled value={field.value} multiline />
                        </>
                    )}
                </Stack>
            </Box>
        </Dialog>
    );
}

export default function TaskViewModal(props) {

    const { children } = props;

    const tableStyling = {
        padding: "6px 10px"
    };

    const [open, setOpen] = React.useState(false);

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = (value) => {
        setOpen(false);
    };

    return (
        <>
            <TableCell sx={{ ...tableStyling }} onClick={handleClickOpen}>
                {children}
            </TableCell>
            <SimpleDialog
                open={open}
                onClose={handleClose}
            />
        </>
    );
}
