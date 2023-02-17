import * as React from 'react';
import DialogTitle from '@mui/material/DialogTitle';
import Drawer from '@mui/material/Drawer';
import Box from '@mui/material/Box';
import IconButton from '@mui/material/IconButton';
import FormGroup from '@mui/material/FormGroup';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';


function SimpleDialog(props) {
    const { onClose, selectedValue, open, handler, fields } = props;

    // впихнуть хэндлер в чекбокс и потом настроить хэндлер

    const handleClose = () => {
        onClose(selectedValue);
    };

    const [state, setState] = React.useState([]);

    React.useEffect(() => {
        setState(props.fields);
        console.log("Drawer: new props", props.fields);
    }, [props.fields])

    return (
        <Drawer anchor="right" onClose={handleClose} open={open} maxWidth="sm">
            <DialogTitle>Настройка столбцов</DialogTitle>
            <Box sx={{
                padding: 2
            }}>
                <FormGroup>
                    {state.map((field, index) =>
                        <FormControlLabel key={index} control={<Checkbox checked={field.show} />} value={field.name} label={field.title} name={index} onChange={handler} />
                    )}
                </FormGroup>
            </Box>
        </Drawer>
    );
}

export default function TableSettingsModal(props) {

    const { tableSettingsHandler, headers } = props;

    const [open, setOpen] = React.useState(false);
    const [stateHeaders, setStateHeaders] = React.useState([]);

    React.useEffect(() => {
        console.log("TableSettingsModal: new props", props.headers);
        setStateHeaders(props.headers);
    }, [props.headers]);



    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = (value) => {
        setOpen(false);
    };

    return (
        <>
            <IconButton aria-label="filter" onClick={handleClickOpen}>
                <MoreVertIcon />
            </IconButton>
            <SimpleDialog
                open={open}
                onClose={handleClose}
                handler={tableSettingsHandler}
                fields={stateHeaders}
            />
        </>

    );
}
