import { useContext } from 'react';

import axios from 'axios';

import AuthContext from '../../context/auth-context';

const clientId = '244658728792-9jpa8bht2tprd3rkoo1gh7jo2uf97fhf.apps.googleusercontent.com';

const GoogleLogin = (props) => {
  const authCtx = useContext(AuthContext);

  function handleCredentialResponse(response) {
    console.log('Google response', response);
    console.log('Encoded JWT ID token: ' + response.credential);

    let headers = new Headers();

    headers.append('Content-Type', 'application/json');
    headers.append('Accept', 'application/json');

    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: {
        idToken: response.credential,
      },
    };

    const client = axios.create({
      baseURL: 'https://localhost:7066',
    });

    client
      .post('/user/authenticate', requestOptions.body, requestOptions.headers)
      .then((response) => {
        console.log('response', response);

        authCtx.isLoggedIn = true;
        authCtx.email = response.body.email;
        authCtx.onLogin();

        props.onSuccess();
      })
      .catch((error) => {
        console.log(error);

        // authCtx.isLoggedIn = true;
        // authCtx.email = "fake email";
        // authCtx.onLogin();
      });
  }
  window.onload = function () {
    window.google.accounts.id.initialize({
      client_id: clientId,
      callback: handleCredentialResponse,
    });
    window.google.accounts.id.renderButton(
      document.getElementById('buttonDiv'),
      { theme: 'outline', size: 'large' }, // customization attributes
    );
    window.google.accounts.id.prompt(); // also display the One Tap dialog
  };

  return <div id='buttonDiv'></div>;
};

export default GoogleLogin;
