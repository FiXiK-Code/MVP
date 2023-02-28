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
        padding: "6px 10px",
        textAlign: "center"
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
            setLength(props.tasks.staffs.length + 1);
        }

        if (props.tasks.projects) {
            setLength(props.tasks.projects.length + 1);
        }
        
        console.log('update');
    }, [props.tasks]);

    let tableBody;
    if (search) {
        tableBody = <TableBody>
            <TaskGroup editHandler={props.editHandler} title={`Результаты поиска по запросу ${search}`} tasks={tasks.done} isOpen={true} colNum={length} headers={stateHeaders} />
        </TableBody>
    } else {
        tableBody =
            <TableBody>
                <TaskGroup editHandler={props.editHandler} title="Выполненные задачи" tasks={tasks.completed} isOpen={false} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} />
                <TaskGroup editHandler={props.editHandler} title="Задачи на сегодня" tasks={tasks.today} isOpen={true} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} />
                <TaskGroup editHandler={props.editHandler} title="Предстоящие задачи" tasks={tasks.future} isOpen={false} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} />
            </TableBody>
    }

    

    let tableHeader;
    if (props.tasks.staffs) {
        // если это вкладка сотрудники
        tableHeader = <TableRow>
            <TableCell sx={{ ...tableStyling }}>Дата</TableCell>
            {props.tasks.staffs.map((header) =>
                <TableCell sx={{ ...tableStyling }} key={header.name}>{header.name}<br />{header.post}</TableCell>
            )}
        </TableRow>

        tableBody = <TableBody>
            <TaskGroupEmployee editHandler={props.editHandler} title="Выполненные задачи" tasks={tasks.completed} isOpen={false} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} staffs={tasks.staffs} />
            <TaskGroupEmployee editHandler={props.editHandler} title="Задачи на сегодня" tasks={tasks.today} isOpen={true} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} staffs={tasks.staffs} />
            <TaskGroupEmployee editHandler={props.editHandler} title="Предстоящие задачи" tasks={tasks.future} isOpen={false} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} staffs={tasks.staffs} />
        </TableBody>
    } else if (props.tasks.projects) {
        // если это вкладка проекты
        tableHeader = <TableRow>
            <TableCell sx={{ ...tableStyling }}>Дата</TableCell>
            {props.tasks.projects.map((header) =>
                <TableCell sx={{ ...tableStyling }} key={header.code}>{header.code}<br />{header.plannedFinishDate}</TableCell>
            )}
        </TableRow>

        tableBody = <TableBody>
            <TaskGroupEmployee editHandler={props.editHandler} title="Выполненные задачи" tasks={tasks.completed} isOpen={false} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} projects={tasks.projects} />
            <TaskGroupEmployee editHandler={props.editHandler} title="Задачи на сегодня" tasks={tasks.today} isOpen={true} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} projects={tasks.projects} />
            <TaskGroupEmployee editHandler={props.editHandler} title="Предстоящие задачи" tasks={tasks.future} isOpen={false} colNum={length} headers={stateHeaders} supervisor={props.supervisor} projectCode={props.projectCode} recipient={props.supervisor} projects={tasks.projects} />
        </TableBody>
    } else {
        // если это вкладка задач или другая вкладка
        tableHeader = <TableRow>
            {headers.map((header) =>
                header.show &&
                <TableCell sx={{ ...tableStyling }} key={header.name}>{header.title}</TableCell>
            )}
        </TableRow>
    }

    return (
        <TableContainer sx={{ maxHeight: 'calc(100vh - 290px)'}} component={Paper}>
            <Table stickyHeader size="small" aria-label="sticky collapsible table">
                <TableHead>
                    { tableHeader }
                </TableHead>
                { tableBody }
            </Table>
        </TableContainer>
    );
}
