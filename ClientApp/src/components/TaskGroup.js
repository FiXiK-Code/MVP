import * as React from 'react';
import IconButton from '@mui/material/IconButton';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import TaskViewModal from './TaskViewModal';
import StatusSelect from './StatusSelect';

function CollapsedTasks(props) {
    const { display, tasks, headers } = props;

    const [stateHeaders, setStateHeaders] = React.useState([]);
    // const [stateTasks, setStateTasks] = React.useState([]);

    React.useEffect(() => {
        setStateHeaders(props.headers);
        console.log('CollapsedTasks: headers', props.headers);
        // setStateTasks(props.tasks);
        console.log('CollapsedTasks: tasks', props.tasks);
    }, [props.headers, props.tasks]);

    const tableStyling = {
        padding: "6px 10px"
    };

    let contents = (
        (typeof props.tasks !== 'undefined') && display
        &&
        <>
            {props.tasks.map((task) =>
                <TableRow sx={{ backgroundColor: task.priorityRaw === -1 ? "#71FACA" : "#FFFFFF" }}>
                    {stateHeaders.map((header) =>
                        header.show &&
                        <>
                            {header.name === "status" ?
                                <TableCell sx={{ ...tableStyling }}>
                                    <StatusSelect taskId={task.id} status={task.status} />
                                </TableCell>

                                :
                                <TaskViewModal editHandler={props.editHandler} headers={props.headers} task={task} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor}  >
                                    {task[header.name]}
                                </TaskViewModal>}
                        </>

                    )}
                </TableRow>
            )}
        </>
    );

    return (
        <>{contents}
        </>
    );
}

export function TaskGroup(props) {
    const { title, tasks, colNum, headers, isOpen } = props;
    const [open, setOpen] = React.useState(isOpen);

    const [stateHeaders, setStateHeaders] = React.useState([]);
    // const [stateTasks, setStateTasks] = React.useState([]);

    React.useEffect(() => {
        setStateHeaders(props.headers);
        setOpen(props.isOpen);

        // setStateTasks(props.tasks);
    }, [props.headers, props.isOpen])

    return (
        <React.Fragment>
            <TableRow sx={{
                '& > *': {
                    borderBottom: 'unset',
                    backgroundColor: '#E1E1FB'
                }

            }}>
                <TableCell colSpan={colNum}>
                    <IconButton
                        aria-label="expand row"
                        size="small"
                        onClick={() => setOpen(!open)}
                    >
                        {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
                    </IconButton>
                    <span>{title}</span>
                </TableCell>

            </TableRow>
            <CollapsedTasks editHandler={props.editHandler} display={open} tasks={tasks} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} />
        </React.Fragment>
    );
}
