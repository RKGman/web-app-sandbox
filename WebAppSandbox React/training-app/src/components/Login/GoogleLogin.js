import React from 'react';

import googleOneTap from 'google-one-tap';

import Button from '../UI/Button/Button';

import classes from './Login.module.css';

const GoogleLogin = () => {
  const options = {
    client_id: '___CLIENT_ID___', // required
    auto_select: false, // optional
    cancel_on_tap_outside: false, // optional
    context: 'signin', // optional
  };

  googleOneTap(options, (response) => {
    // Send response to server
    console.log(response);
  });

  return (
    <React.Fragment>
      <p>Login Below With Google</p>

      <Button className={classes.btn} onClick={googleOneTap}>
        Google Login
      </Button>
    </React.Fragment>
  );
};


export default GoogleLogin;
