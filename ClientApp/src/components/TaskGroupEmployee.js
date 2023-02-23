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
import styles from './TaskGroupEmployee.module.scss';

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
        padding: "6px 10px",
        textAlign: "center"
    };

    let contents = (
        (typeof props.tasks !== 'undefined') && display
        &&
        <>
            {Object.keys(tasks).map((key, i) =>
                <TableRow key={i}>
                    <TableCell>{key}</TableCell>
                    {props.staffs.map((staff, index) =>
                        <TableCell sx={{ ...tableStyling }} key={index}>
                            <span className={styles.taskContainer}>

                                {tasks[key].hasOwnProperty([staff.name]) && tasks[key][staff.name].map((task, j) =>
                                    <TaskViewModal key={j} from="employees" headers={props.headers} task={task} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor}>
                                        {task.desc}
                                    </TaskViewModal>
                                )}
                            </span>
                            {!tasks[key].hasOwnProperty([staff.name]) && <>-</>}
                        </TableCell>
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

    let data = [];

    if (tasks && tasks.length) {
        console.log('tasks is', tasks);
        let result = groupByDate(tasks);
        for (let key in result) {
            result[key] = groupBySupervisor(result[key]);
        }
        console.log('result is', result);
        data = result;
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
            <CollapsedTasks display={open} tasks={data} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} staffs={props.staffs} />
        </React.Fragment>
    );
}
