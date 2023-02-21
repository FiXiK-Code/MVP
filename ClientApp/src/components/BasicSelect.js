import * as React from 'react';
import Box from '@mui/material/Box';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';

export default function BasicSelect(props) {
    return (
        <Box sx={{ minWidth: 120 }}>
            <FormControl fullWidth>
                <InputLabel id="demo-simple-select-label">Тип</InputLabel>
                <Select
                    labelId="demo-simple-select-label"
                    id="demo-simple-select"
                    value={props.type}
                    label="Тип"
                    onChange={props.handleChange}
                >
                    <MenuItem value={1}>Задача</MenuItem>
                    <MenuItem value={2}>Проект</MenuItem>
                </Select>
            </FormControl>
        </Box>
    );
}
