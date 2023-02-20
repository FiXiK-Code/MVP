import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { useNavigate } from "react-router-dom";

function Copyright(props) {
    return (
        <Typography variant="body2" color="text.secondary" align="center" {...props}>
            {'Copyright © '}
            <Link color="inherit" href="/tasks">
                SHIFT.STUDIO
            </Link>{' '}
            {new Date().getFullYear()}
            {'.'}
        </Typography>
    );
}


const theme = createTheme({
    typography: {
        fontFamily: [
            'Mulish',
            'Roboto',
            '"Helvetica Neue"',
            'Arial',
            'sans-serif'
        ].join(','),
    }
});

export default function SignIn() {
    let navigate = useNavigate();

    React.useEffect(() => {
        if (localStorage.getItem('access_token')) {
            navigate("/tasks");
        }
    }, []);
    

    const [error, setError] = React.useState(false);

    const handleSubmit = (event) => {
        console.log('yes');
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        fetch("/api/token", {
            method: "post",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            //make sure to serialize your JSON body
            body: JSON.stringify({
                UserName: data.get('username'),
                Password: data.get('password')
            })
        })
            .then((response => {
                if (response.status !== 200) {
                    throw new Error(response.status)
                }
                return response.json()
            }
            ))
            .then(data => {
                console.log(data);
                localStorage.setItem('access_token', data.access_token);
                localStorage.setItem('username', data.user_login);
                localStorage.setItem('user_id', data.user_id);
                localStorage.setItem('full_name', data.user_name);
                localStorage.setItem('role', data.user_role);
                navigate("/tasks");
            })
            .catch(error => {
                console.log(error);
                setError(true);
            });
    };

    const handleInput = (event) => {
        setError(false);
    }


    return (
        <ThemeProvider theme={theme}>
            <Container component="main" maxWidth="xs">
                <CssBaseline />
                <Box
                    sx={{
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                    }}
                >
                    <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
                        <LockOutlinedIcon />
                    </Avatar>
                    <Typography component="h1" variant="h5">
                        Войти в систему
                    </Typography>
                    <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            id="username"
                            label="Логин"
                            name="username"
                            autoComplete="username"
                            autoFocus
                            error={error}
                            onInput={handleInput}
                        />
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="password"
                            label="Пароль"
                            type="password"
                            id="password"
                            autoComplete="current-password"
                            error={error}
                            onInput={handleInput}
                        />
                        <FormControlLabel
                            control={<Checkbox value="remember" color="primary" />}
                            label="Запомнить меня"
                        />

                        <Button
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2 }}
                            type="submit"
                        >
                            Войти в систему
                        </Button>
                        <Grid container>
                            <Grid item xs>
                                <Link href="#" variant="body2">
                                    Забыли пароль?
                                </Link>
                            </Grid>
                            <Grid item>
                                <Link href="#" variant="body2">
                                    {"Зарегистрироваться"}
                                </Link>
                            </Grid>
                        </Grid>
                    </Box>
                </Box>
                <Copyright sx={{ mt: 8, mb: 4 }} />
            </Container>
        </ThemeProvider>
    );
}
