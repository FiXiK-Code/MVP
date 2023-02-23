import * as React from 'react';
import Button from '@mui/material/Button';
import DialogTitle from '@mui/material/DialogTitle';
import Drawer from '@mui/material/Drawer';
import TableCell from '@mui/material/TableCell';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import { fetchWithAuth, getCurrentDate } from '../utils';
import styles from './TaskViewModal.module.scss';

function AddSelect(props) {
    console.log(props);

    return (
        <Box sx={{ minWidth: 120 }}>
            <FormControl fullWidth>
                <InputLabel id={"select-label-" + props.header}>{props.label}</InputLabel>
                <Select
                    labelId={"select-label-" + props.header}
                    id={"select-" + props.header}
                    value={props.state}
                    label={props.label}
                    onChange={props.handleChange}
                >
                    {props.data.map((item) =>
                        <MenuItem key={item.id} value={item.id}>{item[props.header]}</MenuItem>
                    )}
                </Select>
            </FormControl>
        </Box>
    );
}

function SimpleDialog(props) {
    const { onClose, selectedValue, open, task } = props;

    const handleClose = () => {
        onClose(selectedValue);
    };

    const [data, setData] = React.useState({});

    const [type, setType] = React.useState(1);

    const [selectState1, setSelectState1] = React.useState("");
    const [selectState2, setSelectState2] = React.useState("");
    const [selectState3, setSelectState3] = React.useState("");

    const handleTextChange = (event) => {
        let label = event.label;
        setData(prevState => ({
            ...prevState,
            [label]: event.target.value
        }
        ))
    };

    const handleSelectChange1 = (event) => {
        setSelectState1(event.target.value)
        setData(prevState => ({
            ...prevState,
            supervisor: event.target.value
        }));
    };


    const handleSelectChange2 = (event) => {
        setSelectState2(event.target.value)
        setData(prevState => ({
            ...prevState,
            projectCode: event.target.value
        }));
    };

    const handleSelectChange3 = (event) => {
        setSelectState3(event.target.value)
        setData(prevState => ({
            ...prevState,
            recipient: event.target.value
        }));
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        if (type === 1) {
            fetchWithAuth('/api/PutTasks', 'put', data)
        } else {

        }
    };

    const [edit, setEdit] = React.useState(false);
    let title, content;
    if (edit) {
        title = "Редактирование задачи";
        content =
            <Stack spacing={2} component="form" onSubmit={handleSubmit}>
                {props.headers.map((field) => field.createAvailability &&
                    <>
                        {field.type === "datefield" &&
                            <TextField
                                id={field.name}
                                label={field.title}
                                type="date"
                                onChange={(e) => {
                                    e.label = field.name
                                    handleTextChange(e)
                                }}
                                defaultValue={getCurrentDate('-')}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        }
                        {field.type === "timefield" &&
                            <TextField
                                id={field.name}
                                label={field.title}
                                type="time"
                                inputProps={{
                                    step: 1,
                                }}
                                onChange={(e) => {
                                    e.label = field.name
                                    handleTextChange(e)
                                }}
                                defaultValue="00:00:00"
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        }
                        {field.type === "datetime" &&
                            <TextField
                                id={field.name}
                                label={field.title}
                                type="datetime-local"
                                inputProps={{
                                    step: 1,
                                }}
                                onChange={(e) => {
                                    e.label = field.name
                                    handleTextChange(e)
                                }}
                                defaultValue={getCurrentDate('-') + " 18:00:00"}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        }

                        {field.type === "textfield" &&
                            <TextField id={field.name} label={field.title} variant="outlined" multiline onChange={(e) => {
                                e.label = field.name;
                                handleTextChange(e);
                            }} />
                        }
                        {field.type === "select" &&
                            <AddSelect
                                label={field.title}
                                handleChange={field.name === "recipient" ? handleSelectChange3 : (field.name === "supervisor" ? handleSelectChange1 : handleSelectChange2)}
                                data={props[field.name]}
                                header={field.fieldToShow}
                                state={field.name === "recipient" ? selectState3 : (field.name === "supervisor" ? selectState1 : selectState2)}
                            />
                        }

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
        <Drawer anchor="right" onClose={handleClose} open={open}>
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

    const name = localStorage.getItem('full_name');

    return (
        <>
            {props.from === 'employees' &&
                <span onClick={handleClickOpen} className={styles.task + " " + (!props.task.priority ? styles.priority : "") + " " + (name != props.task.creator ? styles.notMyTask : "" )}>
                    {children}
                </span>
            }
            {props.from !== 'employees' &&
                <TableCell sx={{ ...tableStyling }} onClick={handleClickOpen}>
                    {children}
                </TableCell>
            }
            <SimpleDialog
                open={open}
                onClose={handleClose}
                task={props.task}
                headers={props.headers}
                supervisor={props.supervisor}
                projectCode={props.projectCode}
                recipient={props.supervisor}
            />
        </>
    );
}
