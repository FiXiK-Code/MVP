import React, { Component } from 'react';
import CollapsibleTable from './CollapsibleTable';
import { fetchWithAuth, Unauthorized } from '../utils';

export class Guide extends Component {
    static displayName = Guide.name;

    constructor(props) {
        super(props);
        this.state = {
            auth: true,
            loading: true
        };

        this.renderTasksTable = this.renderTasksTable.bind(this);
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    renderTasksTable(data) {
        return (
            <h1>ok</h1>
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

        let data = response.value;

        // this.setState({ tasks: data, headers: headers, loading: false });
    }
}
