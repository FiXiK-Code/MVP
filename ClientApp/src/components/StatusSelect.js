import * as React from 'react';
import Box from '@mui/material/Box';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import { fetchWithAuth } from '../utils.js';

export default function StatusSelect(props) {
    const { taskId } = props;
    const [status, setStatus] = React.useState('');
    const [color, setColor] = React.useState('#FFFFFF');
    const statuses = ["Создана", "В работе", "На паузе", "Выполнена"];

    React.useEffect(() => {
        console.log(props.status);
        if (typeof props.status !== 'undefined') {
            setStatus(props.status);
        }
    }, [props.status])

    const sendTaskStatus = async (target) => {
        fetchWithAuth('/api/PutTasksStatus', 'put', {
            id: taskId,
            status: status
        });
    }

    const handleChange = (event) => {
        setStatus(event.target.value);
        sendTaskStatus(event.target);

        if (event.target.value === "В работе") {
            setColor('#CEFDED')
        }
        if (event.target.value === "На паузе") {
            setColor('#FFCCDD')
        }
        if (event.target.value === "Выполнена" || event.target.value === "Создана") {
            setColor('#FFFFFF')
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
        </Box>
    );
}
