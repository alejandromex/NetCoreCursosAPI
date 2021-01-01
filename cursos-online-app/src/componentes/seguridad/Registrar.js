import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import style from '../Tool/Style';
import { registrarUsuario } from '../../actions/UsuarioAction';

import React, {useState} from 'react';





const RegistrarUsuario = () =>{

    const[usuario, setUsuario] = useState({
       NombreCompleto : '',
       Email: '',
       Password: '',
       ConfirmarPassword: '',
       UserName: '' 
    });

    function ingresarValoresMemoria(e)
    {

        const{name, value} = e.target;
        setUsuario(anterior => ( {
            ...anterior,
            [name] : value
        }));
    }


    const registrarUsuariobtn = (e) =>{
        e.preventDefault();
        registrarUsuario(usuario).then(response =>{
            window.localStorage.setItem("token_seguridad",response.data.token);
        });
        
    }



    return(
        <div>
            <Container component="main">
                <div style={style.paper}>

                    <Typography component="h1" variant="h5">
                        Registro de Usuario
                    </Typography>

                </div>
                    <form style={style.form} method="post">
                        <Grid container spacing={2}>

                            <Grid item xs={12} md={12}>
                                <TextField name="NombreCompleto" value={usuario.NombreCompleto} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese su nombre y apellidos"/>
                            </Grid>

                            <Grid item xs={12} md={6}>
                                <TextField name="Email" value={usuario.Email} onChange={ingresarValoresMemoria} type="email" variant="outlined" fullWidth label="Ingrese su email"/>
                            </Grid>

                            <Grid item xs={12} md={6}>
                                <TextField name="UserName" value={usuario.UserName} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese su nombre de Usuario"/>
                            </Grid>

                            <Grid item xs={12} md={6}>
                                <TextField name="Password" value={usuario.Password} onChange={ingresarValoresMemoria} type="password" variant="outlined" fullWidth label="Ingrese su Contraseña"/>
                            </Grid>

                            <Grid item xs={12} md={6}>
                                <TextField name="ConfirmarPassword" value={usuario.ConfirmarPassword} onChange={ingresarValoresMemoria} type="password" variant="outlined" fullWidth label="Confirme su Contraseña"/>
                            </Grid>

                        </Grid>

                        <Grid container justify="center">
                            <Grid item xs={12} md={6}>
                                <Button type="submit" onClick={registrarUsuariobtn} fullWidth variant="contained" color="primary" size="large" style={style.submit}>
                                    Registrar
                                </Button>
                            </Grid>
                        </Grid>
                    </form>

            </Container>
        </div>
    );
}

export default RegistrarUsuario;