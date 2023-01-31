import React, { useContext, useState, useEffect, useReducer, useRef } from 'react';

import classes from './Login.module.css';

import Card from '../UI/Card/Card';
import Button from '../UI/Button/Button';
import Input from '../UI/Input/Input';
import AuthContext from '../../context/auth-context';
import GoogleLogin from './GoogleLogin';

const userInput = 'USER_INPUT';
const inputBlur = 'INPUT_BLUR';

const emailReducer = (state, action) => {
  if (action.type === userInput) {
    return { value: action.val, isValid: action.val.includes('@') };
  }

  if (action.type === inputBlur) {
    return { value: state.value, isValid: state.value.includes('@') };
  }
  return { value: '', isValid: false };
};

const passwordReducer = (state, action) => {
  if (action.type === userInput) {
    return { value: action.val, isValid: action.val.trim().length > 6 };
  }

  if (action.type === inputBlur) {
    return { value: state.value, isValid: state.value.trim().length > 6 };
  }
  return { value: '', isValid: false };
};

const Login = () => {
  const [formIsValid, setFormIsValid] = useState(false);

  const [emailState, dispatchEmail] = useReducer(emailReducer, {
    value: '',
    isValid: undefined,
  });

  const [passwordState, dispatchPassword] = useReducer(passwordReducer, {
    value: '',
    isValid: undefined,
  });

  const authCtx = useContext(AuthContext);

  const emailInputRef = useRef();
  const passwordInputRef = useRef();

  useEffect(() => {
    console.log('use effect');

    return () => {
      console.log('use effect cleanup');
    };
  }, []);

  const { isValid: emailIsValid } = emailState; // object destructuring. creating alias 'emailIsValid' for property 'isValid'
  const { isValid: passwordIsValid } = passwordState;

  useEffect(() => {
    const identifier = setTimeout(() => {
      setFormIsValid(emailIsValid && passwordIsValid);
    }, 500);

    return () => {
      console.log('CLEANUP');
      clearTimeout(identifier);
    };
  }, [emailIsValid, passwordIsValid]); // Only using the properties as useEffect dependencies (an object should NEVER be a dependency here because anytime any property of the object changes, the annon. arrow function will run)

  const emailChangeHandler = (event) => {
    dispatchEmail({ type: userInput, val: event.target.value });

    setFormIsValid(emailState.value.includes('@') && passwordState.isValid);
  };

  const passwordChangeHandler = (event) => {
    dispatchPassword({ type: userInput, val: event.target.value });

    setFormIsValid(emailState.isValid && passwordState.isValid);
  };

  const validateEmailHandler = () => {
    dispatchEmail({ type: inputBlur });
  };

  const validatePasswordHandler = () => {
    dispatchPassword({ type: inputBlur });
  };

  const submitHandler = (event) => {
    event.preventDefault();
    if (formIsValid) {
      authCtx.onLogin(emailState.value, passwordState.value);
    } else if (!emailIsValid) {
      emailInputRef.current.focus();
    } else {
      passwordInputRef.current.focus();
    }
  };

  return (
    <Card className={classes.login}>
      <form onSubmit={submitHandler}>
        <Input
          ref={emailInputRef}
          id='email'
          label='E-Mail'
          type='email'
          isValid={emailIsValid}
          value={emailState.value}
          onChange={emailChangeHandler}
          onBlur={validateEmailHandler}
        />
        <Input
          ref={passwordInputRef}
          id='password'
          label='Password'
          type='password'
          isValid={passwordIsValid}
          value={passwordState.value}
          onChange={passwordChangeHandler}
          onBlur={validatePasswordHandler}
        />
        <div className={classes.actions}>
          <Button type='submit' className={classes.btn}>
            Login
          </Button>
        </div>
        <div className={classes.actions}>
          <GoogleLogin />
        </div>
      </form>
    </Card>
  );
};

export default Login;
