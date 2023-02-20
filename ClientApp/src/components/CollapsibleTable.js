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
    });

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
                                <TaskViewModal headers={ props.headers } task={ task } >
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

function TaskGroup(props) {
    const { title, tasks, colNum, headers, isOpen } = props;
    const [open, setOpen] = React.useState(isOpen);

    const [stateHeaders, setStateHeaders] = React.useState([]);
    // const [stateTasks, setStateTasks] = React.useState([]);

    React.useEffect(() => {
        setStateHeaders(props.headers);
        setOpen(props.isOpen);

        // setStateTasks(props.tasks);
    })

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
            <CollapsedTasks display={open} tasks={tasks} headers={stateHeaders} />
        </React.Fragment>
    );
}

export default function CollapsibleTable(props) {
    const { tasks, headers, search } = props;

    const tableStyling = {
        padding: "6px 10px"
    };

    const [stateHeaders, setStateHeaders] = React.useState([]);
    const [length, setLength] = React.useState(13);
    /* const [stateTasks, setStateTasks] = React.useState({
         done: [],
         today: [],
         upcoming: [],
     });*/

    React.useEffect(() => {
        setStateHeaders(props.headers);
        let counter = 0;
        props.headers.map((header) => {
            if (header.show) counter++;
        })
        setLength(counter);
        console.log('update');
        // setStateTasks(props.tasks);
        // console.log('CollapsibleTable: tasks', props.tasks);
    });

    let tableBody;
    if (search) {
        tableBody = <TableBody>
            <TaskGroup title={`Результаты поиска по запросу ${search}`} tasks={tasks.done} isOpen={true} colNum={length} headers={stateHeaders} />
        </TableBody>
    } else {
        tableBody =
            <TableBody>
                <TaskGroup title="Выполненные задачи" tasks={tasks.completed} isOpen={false} colNum={length} headers={stateHeaders} />
                <TaskGroup title="Задачи на сегодня" tasks={tasks.today} isOpen={true} colNum={length} headers={stateHeaders} />
                <TaskGroup title="Предстоящие задачи" tasks={tasks.upcoming} isOpen={false} colNum={length} headers={stateHeaders} />
            </TableBody>
    }

    return (
        <TableContainer component={Paper}>
            <Table size="small" aria-label="collapsible table">
                <TableHead>
                    <TableRow>
                        {headers.map((header) =>
                            header.show &&
                            <TableCell sx={{ ...tableStyling }} key={header.name}>{header.title}</TableCell>
                        )}
                    </TableRow>
                </TableHead>
                { tableBody }
            </Table>
        </TableContainer>
    );
}
