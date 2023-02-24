import * as React from 'react';
import Button from '@mui/material/Button';
import DialogTitle from '@mui/material/DialogTitle';
import Drawer from '@mui/material/Drawer';
import FormGroup from '@mui/material/FormGroup';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import BasicSelect from './BasicSelect';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import { fetchWithAuth, getCurrentDate, getProjectHeaders, getServerTimeFromLocale } from '../utils.js'
import CustomSnackbar from "./CustomSnackbar";

function AddSelect(props) {
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
    const projectHeaders = [

    ];

    let headers;

    const { onClose, selectedValue, open } = props;

    const handleClose = () => {
        onClose(selectedValue);
    };

    const [data, setData] = React.useState({});

    const [type, setType] = React.useState(1);

    const [selectState1, setSelectState1] = React.useState("");
    const [selectState2, setSelectState2] = React.useState("");
    const [selectState3, setSelectState3] = React.useState("");

    const [messageOpen, setMessageOpen] = React.useState(false);
    const [message, setMessage] = React.useState("");
    const [messageType, setMessageType] = React.useState("");

    const handleTextChange = (event) => {
        let label = event.label;
        let value = event.target.value;
        if (label == "priority") {
            value = parseInt(value);
        }

        const dateTimeFields = ["dedline", "start", "finish", "plannedFinishDate"]

        // если поле типа датавремя, то учтем часовой пояс
        if (dateTimeFields.includes(label)) {
            value = getServerTimeFromLocale(value);
        } 
        setData(prevState => ({
            ...prevState,
            [label]: value
        }

        ))
    };

    React.useEffect(() => {
        setType(parseInt(props.type));
    }, [props.type])

    const handleSelectChange = (event) => {
        setType(event.target.value)
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

    const handleSubmit = async (event) => {
        event.preventDefault();
        let url;
        if (type === 1) {
            url = '/api/PostTasks';
        } else {      
            url = '/api/PostProj';
        }
        const response = await fetchWithAuth(url, 'post', data);
        if (response.statusCode >= 200 && response.statusCode < 400) {
            // if success
            setMessageOpen(true);
            setMessage(response.value.message);
            setMessageType("success");
            props.addHandler(response.value.type, response.value.value);
            setTimeout(() => {
                setMessageOpen(false);
                handleClose();
            }, 2000)
            return true;
        } else {
            // if error
            setMessageOpen(true);
            setMessage(response.value);
            setMessageType("error");
            return false;
        }
    }

    let title;
    if (type == 1) {
        title = "Создание задачи";
        headers = props.headers;
    } else {
        title = "Создание проекта";
        headers = getProjectHeaders();
    }

    return (
        <Drawer anchor="right" onClose={handleClose} open={open}>
            <DialogTitle>{title}</DialogTitle>
            <Box sx={{
                padding: 2,
                minWidth: "400px"
            }}>

                <FormGroup>
                    <Stack spacing={2} component="form" onSubmit={handleSubmit}>
                        <BasicSelect handleChange={handleSelectChange} type={type} />
                        {headers.map((field) => field.createAvailability &&
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
                                {field.type === "number" &&
                                    <TextField id={field.name} type="number" label={field.title} variant="outlined" onChange={(e) => {
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
                        <Button variant="contained" type="submit">Создать</Button>
                    </Stack>
                </FormGroup>

            </Box>
            <CustomSnackbar severity={messageType} open={messageOpen} setOpen={setMessageOpen} message={message} />
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
            <Button style={{ minWidth: 180 }} variant="contained" onClick={handleClickOpen}>{props.title}</Button>
            <SimpleDialog
                type={props.type}
                open={open}
                onClose={handleClose}
                headers={props.headers}
                supervisor={props.supervisor}
                projectCode={props.projectCode}
                recipient={props.supervisor}
                addHandler={props.addHandler }
            />
        </>
    );
}
