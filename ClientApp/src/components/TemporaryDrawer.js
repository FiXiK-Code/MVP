import * as React from 'react';
import Box from '@mui/material/Box';
import Drawer from '@mui/material/Drawer';
import Button from '@mui/material/Button';
import List from '@mui/material/List';
import Divider from '@mui/material/Divider';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import InboxIcon from '@mui/icons-material/MoveToInbox';
import MailIcon from '@mui/icons-material/Mail';

export function TableSettingsDrawer(props) {

    const { onClose, selectedValue, open, handler } = props;

    return (
        <TemporaryDrawer>
            <Box sx={{
                padding: 2
            }}>
                <Typography component="h2" variant="h2">
                    Настройка столбцов
                </Typography>
                <FormGroup>
                    {fields.map((field) =>
                        <>
                            <FormControlLabel control={<Checkbox defaultChecked />} value={field.name} label={field.title} onInput={handler} />
                        </>
                    )}
                </FormGroup>
            </Box>
        </TemporaryDrawer>
    )
}

export default function TemporaryDrawer(props) {
    const { children } = props;

    const [state, setState] = React.useState({
        top: false,
        left: false,
        bottom: false,
        right: false,
    });

    const toggleDrawer =
        (anchor, open) =>
            (event) => {
                if (
                    event.type === 'keydown' &&
                    ((event).key === 'Tab' ||
                        (event).key === 'Shift')
                ) {
                    return;
                }

                setState({ ...state, [anchor]: open });
            };

    return (
        <div>
            <Button onClick={toggleDrawer(anchor, true)}>{anchor}</Button>
            <Drawer
                anchor={anchor}
                open={state[anchor]}
                onClose={toggleDrawer(anchor, false)}
            >
                {children}
            </Drawer>

        </div>
    );
}
