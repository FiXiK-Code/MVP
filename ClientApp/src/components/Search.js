import React, { Component } from 'react';
import styles from './Search.module.scss';
import FormGroup from '@mui/material/FormGroup';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import IconButton from '@mui/material/IconButton';
import FilterListIcon from '@mui/icons-material/FilterList';
import TableSettingsModal from './TableSettingsModal';

export class Search extends Component {

    constructor(props) {
        super(props);

        this.state = {
            searchTerm: ""
        };

        this.handleInput = this.handleInput.bind(this);
    }

    handleInput(e) {
        this.props.handleInput(e);
    }

    static displayName = Search.name;

    render() {
        return (
            <>
                <Stack spacing={1} direction="row">
                    <form onSubmit={this.props.handleSubmit} style={{ width: "100%" }}>
                        <FormGroup>
                            <TextField onChange={this.handleInput} className={styles.searchField} id="search-field" label="Поиск..." variant="filled" />
                        </FormGroup>
                    </form>
                    <IconButton aria-label="more">
                        <FilterListIcon />
                    </IconButton>
                    <TableSettingsModal tableSettingsHandler={this.props.tableSettingsHandler} headers={this.props.headers} />
                </Stack>
            </>
        );
    }
}
