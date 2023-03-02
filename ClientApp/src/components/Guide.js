import React, { Component } from 'react';
import { fetchWithAuth, Unauthorized } from '../utils';
import Paper from '@mui/material/Paper';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';

const tableStyling = { textAlign: "center" };

export class Guide extends Component {
    static displayName = Guide.name;

    constructor(props) {
        super(props);
        this.state = {
            auth: true,
            loading: true,
            data: {}
        };

        this.renderTasksTable = this.renderTasksTable.bind(this);
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    

    renderTasksTable(data) {
        return (
            <>
                <h2>Сотрудники</h2>
                <TableContainer sx={{ maxHeight: 'calc(100vh - 290px)', marginBottom: '50px' }} component={Paper}>
                    <Table stickyHeader aria-label="table">
                        <TableHead>
                            <TableRow>
                                <TableCell sx={{ ...tableStyling }}>
                                    Отдел
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    Роль
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    Должность
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    ФИО
                                </TableCell>
                            </TableRow>
                        </TableHead>
                        {data.managementDepartment.array.map((employee, key) => <>
                            <TableRow key={key}>
                                {key === 0 &&
                                    <TableCell sx={{ ...tableStyling }} rowSpan={data.managementDepartment.array.length}>
                                        {data.managementDepartment.name}
                                    </TableCell>
                                }
                                <TableCell sx={{ ...tableStyling }}>
                                    {employee.role}
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    {employee.post}
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    {employee.name}
                                </TableCell>
                            </TableRow>
                        </>
                        )}
                        {data.designDepartment.array.map((employee, key) => <>
                            <TableRow key={key}>
                                {key === 0 &&
                                    <TableCell sx={{ ...tableStyling }} rowSpan={data.designDepartment.array.length}>
                                        {data.designDepartment.name}
                                    </TableCell>
                                }
                                <TableCell sx={{ ...tableStyling }}>
                                    {employee.role}
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    {employee.post}
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    {employee.name}
                                </TableCell>
                            </TableRow>
                        </>
                        )}
                        {data.researchDepartment.array.map((employee, key) => <>
                            <TableRow key={key}>
                                {key === 0 &&
                                    <TableCell sx={{ ...tableStyling }} rowSpan={data.researchDepartment.array.length}>
                                        {data.researchDepartment.name}
                                    </TableCell>
                                }
                                <TableCell sx={{ ...tableStyling }}>
                                    {employee.role}
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    {employee.post}
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    {employee.name}
                                </TableCell>
                            </TableRow>
                        </>
                        )}
                    </Table>
                </TableContainer>
                <h2>Роли</h2>
                <TableContainer component={Paper} sx={{marginBottom: '50px'} }>
                    <Table aria-label="table">
                        <TableHead>
                            <TableRow>
                                <TableCell sx={{ ...tableStyling }}>
                                    Роль
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    От кого принимает задачи
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    Кому ставит задачи
                                </TableCell>
                            </TableRow>
                        </TableHead>
                        {data.companyRoleStruct.map((role, key) => <>
                            <TableRow key={key}>
                                <TableCell sx={{ ...tableStyling }}>
                                    {role.role}
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    {role.supervisor}
                                </TableCell>
                                <TableCell sx={{ ...tableStyling }}>
                                    {role.resipient}
                                </TableCell>
                            </TableRow>
                        </>
                        )}
                    </Table>
                </TableContainer>
            </>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Загрузка данных...</em></p>
            : this.renderTasksTable(this.state.data);


        return (
            <div>
                <Unauthorized auth={this.state.auth} />
                <h1>Справочник</h1>
                {contents}
            </div>
        );
    }

    async populateWeatherData() {
        const response = await fetchWithAuth("/api/Getguide");
        console.log(response);
        if (response.status === 401) {
            this.setState({
                auth: false
            })
        }

        this.setState({ data: response.value, loading: false });
    }
}
