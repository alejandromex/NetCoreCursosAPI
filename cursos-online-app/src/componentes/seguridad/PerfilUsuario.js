import React, {useEffect, useState} from 'react';
import style from '../Tool/Style';
import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import { actualizarUsuario, obtenerUsuarioActual } from '../../actions/UsuarioAction';

const PerfilUsuario = () =>{

    const [usuario, setUsuario] = useState({
        nombreCompleto: '',
        email: '',
        password: '',
        confirmarPassword: '',
        username: ''
    });

    const ingresarValoresMemoria = (e) =>{
        const{name, value} = e.target;
        setUsuario(anterior => ({
            ...anterior,
            [name] : value
        }));
    }

    useEffect(() => {
        obtenerUsuarioActual().then(response =>{
            console.log(response);
            setUsuario(response.data);

        });
    }, []);

    const btnActualizarPerfil = (e) => {
        e.preventDefault();
        actualizarUsuario(usuario).then(response =>{
            console.log(response);
            window.localStorage.setItem("token_seguridad", response.data.token);
        });
    }



    return(
        <Container component="main" maxWidth="md" justify="center">
            <div style={style.paper}>
                <Typography component="h1" variant="h5">
                    Perfil de usuario
                </Typography>
            </div>

            <form style={style.form}>
                <Grid container spacing={2}>
                    <Grid item md={12} xs={12}>
                        <TextField name="nombreCompleto" value={usuario.nombreCompleto} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese nombre y apellidos"/>
                    </Grid>

                    <Grid item md={6} xs={12}>
                        <TextField name="username" value={usuario.username} onChange={ingresarValoresMemoria} type="text" variant="outlined" fullWidth label="Ingrese su UserName"/>
                    </Grid>

                    <Grid item md={6} xs={12}>
                        <TextField name="email" value={usuario.email} onChange={ingresarValoresMemoria} type="email" variant="outlined" fullWidth label="Ingrese Email"/>
                    </Grid>

                    <Grid item md={6} xs={12}>
                        <TextField name="password" value={usuario.password} onChange={ingresarValoresMemoria} type="password" variant="outlined" fullWidth label="Ingrese password"/>
                    </Grid>

                    <Grid item md={6} xs={12}>
                        <TextField name="confirmarPassword" value={usuario.confirmarPassword} onChange={ingresarValoresMemoria} type="password" variant="outlined" fullWidth label="Confirmar password"/>
                    </Grid>

                </Grid>

                <Grid container spacing={2} justify="center">
                    <Grid item xs={12} md={5}>
                        <Button type="submit" onClick={btnActualizarPerfil} variant="contained" fullWidth size="large" color="primary" style={style.submit}>
                            Guardar Datos    
                        </Button>    
                    </Grid> 
                </Grid>
            </form>
        </Container>
    );
}

export default PerfilUsuario;