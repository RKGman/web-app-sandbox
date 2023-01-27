import React, { useState, useEffect } from 'react';
// import { useGoogleLogout } from 'react-google-login';

import Login from './components/Login/Login';
import Home from './components/Home/Home';
import MainHeader from './components/MainHeader/MainHeader';

// const clientId = '244658728792-9jpa8bht2tprd3rkoo1gh7jo2uf97fhf.apps.googleusercontent.com';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isLoggedInWithGoogle, setIsLoggedInWithGoogle] = useState(false);

  useEffect(() => {
    const storedUserLoggedInInformation = localStorage.getItem('isLoggedIn');

    if (storedUserLoggedInInformation === '1') {
      setIsLoggedIn(true);
    }
  }, []);

  // const { signOut } = useGoogleLogout({
  //   clientId,
  //   onLogoutSuccess,
  //   onFailure
  // });

  // const onLogoutSuccess = () => {
  //   console.log('google logout successful');
  // };

  // const onFailure = () => {
  //   console.log('failed to logout of google');
  // };

  const loginHandler = (loginParams) => {
    // TODO: Check email or password

    localStorage.setItem('isLoggedIn', '1');
    setIsLoggedIn(true);

    if (loginParams.IsLoggedInWithGoogle) {
      setIsLoggedInWithGoogle(true);
    }
  };

  const logoutHandler = () => {
    if (isLoggedInWithGoogle) {
      setIsLoggedInWithGoogle(false);
      // signOut();
    } else {
      setIsLoggedIn(false);
    }

    localStorage.removeItem('isLoggedIn');
  };

  return (
    <React.Fragment>
      <MainHeader isAuthenticated={isLoggedIn} onLogout={logoutHandler} />
      <main>
        {!isLoggedIn && <Login onLogin={loginHandler} isLoggedIn={isLoggedIn}/>}
        {isLoggedIn && <Home onLogout={logoutHandler} />}
      </main>
    </React.Fragment>
  );
}

export default App;
