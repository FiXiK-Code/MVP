import React, { Component } from 'react';
import styles from './Search.module.scss';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import IconButton from '@mui/material/IconButton';
import FilterListIcon from '@mui/icons-material/FilterList';
import TableSettingsModal from './TableSettingsModal';

export class Search extends Component {

    constructor(props) {
        super(props);

        this.state = {
            headers: []
        };
    }

    componentWillReceiveProps() {
        this.setState({
            headers: this.props.headers
        })
        console.log("Search: new props", this.props.headers);
    }


    static displayName = Search.name;

    render() {
        return (
            <>
                <Stack spacing={ 1 } direction="row">
                    <TextField className={styles.searchField} id="search-field" label="Поиск..." variant="filled" />
                    <IconButton aria-label="more">
                        <FilterListIcon />
                    </IconButton>
                    <TableSettingsModal tableSettingsHandler={this.props.tableSettingsHandler} headers={ this.state.headers } />
                </Stack>
            </>
        );
    }
}
