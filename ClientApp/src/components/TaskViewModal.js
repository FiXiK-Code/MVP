import * as React from 'react';
import Button from '@mui/material/Button';
import DialogTitle from '@mui/material/DialogTitle';
import Drawer from '@mui/material/Drawer';
import TableCell from '@mui/material/TableCell';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import { fetchWithAuth } from '../utils';

function SimpleDialog(props) {
    const { onClose, selectedValue, open, task } = props;

    const handleClose = () => {
        onClose(selectedValue);
    };

    const handleSubmit = () => {
        console.log('save');
        fetchWithAuth()
    };

    const [edit, setEdit] = React.useState(false);
    let title, content;
    if (edit) {
        title = "Редактирование задачи";
        content =
            <Stack spacing={2} component="form" onSubmit={handleSubmit}>               
                {props.headers.map((field) =>
                    <>
                        <TextField id={field.name} label={field.title} variant="outlined" value={task[field.name]} multiline />
                    </>
                )}
                <Button variant="contained" type="submit">Сохранить</Button>
            </Stack>
    } else {
        title = "Просмотр задачи";
        content =
            <Stack spacing={2}>
                <Button variant="contained" onClick={() => setEdit(true)}>Редактировать</Button>
                {props.headers.map((field) =>
                    <>
                        <TextField id={field.name} label={field.title} variant="outlined" disabled value={task[field.name]} multiline />
                    </>
                )}           
            </Stack>
    }



    return (
        <Drawer anchor="right" onClose={handleClose} open={open} maxWidth="sm" fullWidth>
            <DialogTitle>{title}</DialogTitle>
            <Box sx={{
                padding: 2,
                minWidth: '400px'
            }}>
                {content}
            </Box>
        </Drawer>
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
                task={props.task}
                headers={props.headers}
            />
        </>
    );
}
