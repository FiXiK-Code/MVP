import * as React from 'react';
import Box from '@mui/material/Box';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import { fetchWithAuth } from '../utils.js';
import CustomSnackbar from "./CustomSnackbar";

export default function StatusSelect(props) {
    const { taskId } = props;
    const [status, setStatus] = React.useState('');
    const [color, setColor] = React.useState('#FFFFFF');
    const [messageOpen, setMessageOpen] = React.useState(false);
    const [message, setMessage] = React.useState("");
    const [messageType, setMessageType] = React.useState("");
    const statuses = ["Создана", "В работе", "На паузе", "Выполнена"];

    React.useEffect(() => {
        console.log(props.status);
        if (typeof props.status !== 'undefined') {
            setStatus(props.status);
            if (props.status === "В работе") {
                setColor('#CEFDED')
            }
            if (props.status === "На паузе") {
                setColor('#FFCCDD')
            }
            if (props.status === "Выполнена" || props.status === "Создана") {
                setColor('#FFFFFF')
            }
        }
    }, [props.status])

    const sendTaskStatus = async (target) => {
        const response = await fetchWithAuth('/api/PutTasksStatus', 'put', {
            id: taskId,
            status: target.value
        });
        console.log(response);
        if (response.statusCode >= 200 && response.statusCode < 400) {
            // if success
            setMessageOpen(true);
            setMessage(response.value.message);
            setMessageType("success");
            if (target.value === "Выполнена") {
                props.setParentState(taskId, response.value.type);
            }
            return true;
        } else {
            // if error
            setMessageOpen(true);
            setMessage(response.value.message);
            setMessageType("error");
            return false;
        }
    }

    const handleChange = async (event) => {
        console.log(event);
        
        if (await sendTaskStatus(event.target)) {
            setStatus(event.target.value);
            if (event.target.value === "В работе") {
                setColor('#CEFDED')
            }
            if (event.target.value === "На паузе") {
                setColor('#FFCCDD')
            }
            if (event.target.value === "Выполнена" || event.target.value === "Создана") {
                setColor('#FFFFFF')
            }
        } else {

        }

    };

    return (
        <Box sx={{ minWidth: 150 }}>
            <FormControl fullWidth>
                <InputLabel id="demo-simple-select-label"></InputLabel>
                <Select
                    style={{backgroundColor: color} }
                    labelId="demo-simple-select-label"
                    id="demo-simple-select"
                    value={ status } 
                    label=""
                    onChange={handleChange}
                >
                    <MenuItem value={"Создана"}>Создана</MenuItem>
                    <MenuItem value={"В работе"}>В работе</MenuItem>
                    <MenuItem value={"На паузе"}>На паузе</MenuItem>
                    <MenuItem value={"Выполнена"}>Выполнена</MenuItem>
                </Select>
            </FormControl>
            <CustomSnackbar severity={ messageType } open={messageOpen} setOpen={ setMessageOpen } message={ message }  />
        </Box>
    );
}
