import { Avatar, Button, Container, TextField, Typography } from '@material-ui/core';
import React, { useState } from 'react'
import style from '../Tool/Style';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import { loginUsuario } from '../../actions/UsuarioAction';

const Login = () =>{

    const[usuario, setUsuario] = useState({
        Email: "",
        Password: "" 
    });

    const ingresarValoresMemoria = (e) =>{
        const{name, value} = e.target;
        setUsuario(anterior => ({
            ...anterior,
            [name]:  value
        }));
    }

    const login = (e) =>{
        e.preventDefault();
        console.log(usuario);
        loginUsuario(usuario).then(response =>{
            console.log(response);
            window.localStorage.setItem("token_seguridad",response.data.token);
        });
    }

    return(
        <Container maxWidth="xs">
            <div style={style.paper}>
                <Avatar style={style.prueba}>
                    <LockOutlinedIcon style={style.icon}/>
                </Avatar>
                <Typography component="h1" variant="h5">
                    Login de usuario
                </Typography>
                <form style={style.form}>
                    <TextField variant="outlined" name="Email" value={usuario.Email}  onChange={ingresarValoresMemoria} label="Ingrese username" fullWidth/>
                    <TextField variant="outlined" name="Password" value={usuario.Password}  onChange={ingresarValoresMemoria} type="password" label="Ingrese su contraseña" fullWidth margin="normal"/>
                    <Button type="submit" onClick={login} variant="contained" color="primary" style={style.submit} fullWidth>
                        Iniciar Sesion
                    </Button>
                </form>
            </div>
        </Container>
    );
}


export default Login;