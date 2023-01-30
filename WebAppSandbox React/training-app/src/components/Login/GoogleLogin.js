import React from 'react';

const GoogleLogin = () => {
  return (
    <React.Fragment>
      <div
        id='g_id_onload'
        data-client_id='244658728792-9jpa8bht2tprd3rkoo1gh7jo2uf97fhf.apps.googleusercontent.com'
        data-context='signin'
        data-ux_mode='popup'
        data-login_uri='http://localhost:3000'
        data-auto_prompt='false'
      ></div>

      <div
        className='g_id_signin'
        data-type='standard'
        data-shape='pill'
        data-theme='filled_black'
        data-text='signin_with'
        data-size='medium'
        data-logo_alignment='left'
      ></div>
    </React.Fragment>
  );
};

export default GoogleLogin;
