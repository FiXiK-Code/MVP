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
                <TableRow sx={{ backgroundColor: task.priority == 0 ? "#71FACA" : "#FFFFFF" }}>
                    {stateHeaders.map((header) =>
                        header.show &&
                        <>
                            {header.name === "status" ?
                                <TableCell sx={{ ...tableStyling }}>
                                    <StatusSelect taskId={task.id} status={task.status} />
                                </TableCell>

                                :
                                <TaskViewModal headers={props.headers} task={task} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor}  >
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

function groupBy(key) {
    return function group(array) {
        return array.reduce((acc, obj) => {
            const property = obj[key];
            acc[property] = acc[property] || [];
            acc[property].push(obj);
            return acc;
        }, {});
    };
}

const groupByDate = groupBy("date");
const groupBySupervisor = groupBy("supervisor");

export function TaskGroupEmployee(props) {
    const { title, tasks, colNum, headers, isOpen, staffs } = props;
    const [open, setOpen] = React.useState(isOpen);

    const [stateHeaders, setStateHeaders] = React.useState([]);

    React.useEffect(() => {
        setStateHeaders(props.headers);
        setOpen(props.isOpen);

    }, [props.headers, props.isOpen])

    console.log(tasks);

    const data = [];
    const meta = [];

    if (tasks && tasks.length) {
        console.log('tasks is', tasks);
        let result = groupByDate(tasks);
        for (let key in result) {
            result[key] = groupBySupervisor(result[key]);
        }
        console.log('result is', result);

        for (let i = 0; i < tasks.length; i++) {
            data[i] = [];
            for (let j = 0; j < staffs.length; j++) {
                data[i][j] = [];
            }
        }
    }

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
            <CollapsedTasks display={open} tasks={tasks} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} />
        </React.Fragment>
    );
}
