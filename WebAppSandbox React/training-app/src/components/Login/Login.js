import React, { useState, useEffect } from 'react';

import Card from '../UI/Card/Card';
import classes from './Login.module.css';
import Button from '../UI/Button/Button';

import GoogleLogin from 'react-google-login';

// import { useGoogleLogin } from 'react-google-login';
import { gapi } from 'gapi-script';

const clientId = '244658728792-9jpa8bht2tprd3rkoo1gh7jo2uf97fhf.apps.googleusercontent.com'; // TODO: Read this from a keyvault/secret (?)

const Login = (props) => {
  const [enteredEmail, setEnteredEmail] = useState('');
  const [emailIsValid, setEmailIsValid] = useState();
  const [enteredPassword, setEnteredPassword] = useState('');
  const [passwordIsValid, setPasswordIsValid] = useState();
  const [formIsValid, setFormIsValid] = useState(false);

  useEffect(() => {
    setFormIsValid(enteredEmail.includes('@') && enteredPassword.trim().length > 6);
  }, [enteredEmail, enteredPassword]);

  useEffect(() => {
    const initClient = () => {
      gapi.client.init({
        clientId: clientId,
        scope: '',
      });
    };
    gapi.load('client:auth2', initClient);
  }), [props.isLoggedIn];

  const emailChangeHandler = (event) => {
    setEnteredEmail(event.target.value);
  };

  // const { signin } = useGoogleLogin({
  //   onSuccess,
  //   onFailure,
  //   clientId,
  //   isSignedIn: true
  // })

  const onSuccess = (res) => {
    console.log('success:', res);
    setEnteredEmail(res.profileObj.email);

    const arg = {
      email: enteredEmail,
      password: '',
      isLoggedInWithGoogle: true
    }

    props.onLogin(arg);
  };

  const onFailure = (err) => {
    console.log('failed:', err);
  };

  const passwordChangeHandler = (event) => {
    setEnteredPassword(event.target.value);
  };

  const validateEmailHandler = () => {
    setEmailIsValid(enteredEmail.includes('@'));
  };

  const validatePasswordHandler = () => {
    setPasswordIsValid(enteredPassword.trim().length > 6);
  };

  const submitHandler = (event) => {
    console.log(event);
    event.preventDefault();
    props.onLogin(enteredEmail, enteredPassword);
  };

  return (
    <Card className={classes.login}>
      <form onSubmit={submitHandler}>
        <div className={`${classes.control} ${emailIsValid === false ? classes.invalid : ''}`}>
          <label htmlFor='email'>E-Mail</label>
          <input
            type='email'
            id='email'
            value={enteredEmail}
            onChange={emailChangeHandler}
            onBlur={validateEmailHandler}
          />
        </div>
        <div className={`${classes.control} ${passwordIsValid === false ? classes.invalid : ''}`}>
          <label htmlFor='password'>Password</label>
          <input
            type='password'
            id='password'
            value={enteredPassword}
            onChange={passwordChangeHandler}
            onBlur={validatePasswordHandler}
          />
        </div>
        <div className={classes.actions}>
          <Button type='submit' className={classes.btn} disabled={!formIsValid}>
            Login
          </Button>
        </div>
        <br />
        {/* <div className={classes.actions}>
          <Button onClick={signin} type='submit' className={classes.btn} disabled={!formIsValid}>
             Login with Google
          </Button>
        </div> */}
        <div className={classes.actions}>
          <GoogleLogin
            clientId={clientId}
            buttonText='Sign in with Google'
            onSuccess={onSuccess}
            onFailure={onFailure}
            cookiePolicy={'single_host_origin'}
            isSignedIn={true}
          />
        </div>
      </form>
    </Card>
  );
};

export default Login;
