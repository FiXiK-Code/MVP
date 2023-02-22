import * as React from 'react';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { TaskGroup } from './TaskGroup';
import { TaskGroupEmployee } from './TaskGroupEmployee';


export default function CollapsibleTable(props) {
    const { tasks, headers, search } = props;

    const tableStyling = {
        padding: "6px 10px"
    };

    const [stateHeaders, setStateHeaders] = React.useState([]);
    const [length, setLength] = React.useState(13);

    React.useEffect(() => {
        setStateHeaders(props.headers);
        let counter = 0;
        props.headers.map((header) => {
            if (header.show) counter++;
        })
        setLength(counter);
        console.log('update');


    }, [props.headers]);

    React.useEffect(() => {
        if (props.tasks.staffs) {
            setLength(props.tasks.staffs.length);
        }
        
        console.log('update');
    }, [props.tasks]);

    let tableBody;
    if (search) {
        tableBody = <TableBody>
            <TaskGroup title={`Результаты поиска по запросу ${search}`} tasks={tasks.done} isOpen={true} colNum={length} headers={stateHeaders} />
        </TableBody>
    } else {
        tableBody =
            <TableBody>
                <TaskGroup title="Выполненные задачи" tasks={tasks.completed} isOpen={false} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} />
                <TaskGroup title="Задачи на сегодня" tasks={tasks.today} isOpen={true} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} />
                <TaskGroup title="Предстоящие задачи" tasks={tasks.upcoming} isOpen={false} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} />
            </TableBody>
    }

    

    let tableHeader;
    if (props.tasks.staffs) {
        // если это вкладка сотрудники
        console.log('staffs!', tasks.staffs);
        tableHeader = <TableRow>
            {props.tasks.staffs.map((header) =>
                <TableCell sx={{ ...tableStyling }} key={header.name}>{header.name}<br />{header.post}</TableCell>
            )}
        </TableRow>

        tableBody = <TableBody>
            <TaskGroupEmployee title="Выполненные задачи" tasks={tasks.completed} isOpen={false} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} staffs={tasks.staffs} />
            <TaskGroupEmployee title="Задачи на сегодня" tasks={tasks.today} isOpen={true} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} staffs={tasks.staffs} />
            <TaskGroupEmployee title="Предстоящие задачи" tasks={tasks.upcoming} isOpen={false} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} staffs={tasks.staffs} />
        </TableBody>
    } else {
        tableHeader = <TableRow>
            {headers.map((header) =>
                header.show &&
                <TableCell sx={{ ...tableStyling }} key={header.name}>{header.title}</TableCell>
            )}
        </TableRow>
    }

    return (
        <TableContainer component={Paper}>
            <Table size="small" aria-label="collapsible table">
                <TableHead>
                    { tableHeader }
                </TableHead>
                { tableBody }
            </Table>
        </TableContainer>
    );
}
