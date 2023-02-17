import * as React from 'react';
import Box from '@mui/material/Box';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import { fetchWithAuth } from '../utils.js';

export default function StatusSelect(props) {
    const { taskId } = props;
    const [status, setStatus] = React.useState(2);
    const [color, setColor] = React.useState('#FFFFFF');
    const statuses = ["В работе", "На паузе", "Выполнена"];

    React.useEffect(() => {
        setStatus(props.status);
    }, [props.status])

    const sendTaskStatus = async (target) => {
        fetchWithAuth('/api/PutTasksStatus', 'put', {
            id: taskId,
            status: statuses[target.value]
        });
    }

    const handleChange = (event) => {
        setStatus(event.target.value);
        sendTaskStatus(event.target);

        if (event.target.value === 0) {
            setColor('#CEFDED')
        }
        if (event.target.value === 1) {
            setColor('#FFCCDD')
        }
        if (event.target.value === 2) {
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
                    value={status}
                    label=""
                    onChange={handleChange}
                >
                    <MenuItem value={0}>В работе</MenuItem>
                    <MenuItem value={1}>На паузе</MenuItem>
                    <MenuItem value={2}>Выполнена</MenuItem>
                </Select>
            </FormControl>
        </Box>
    );
}
