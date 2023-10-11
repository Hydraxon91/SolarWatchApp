import React, { useState, useEffect } from "react";
import ReactDOM from 'react-dom';
import './index.css';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter, createBrowserRouter, RouterProvider, Route } from 'react-router-dom';
import RegisterPage from './Pages/RegisterPage';
import FrontPage from './Pages/FrontPage';
import LoginPage from './Pages/LoginPage';
import SolarWatchPage from "./Pages/SolarWatchPage";
import Cookies from 'universal-cookie';
import jwt from 'jwt-decode';

const App = () => {
  const cookies = new Cookies();
  const [user, setUser] = useState(null);

  useEffect(() => {
    const token = cookies.get("jwt_authorization");
    if (token &&!user) {
      // Decode and set the user here based on the token
      const decoded = decodeToken(token);
      setUser(decoded);
    }
  }, [cookies, user]);

  const decodeToken = (token) =>{
    return jwt(token);
  }

  const router = createBrowserRouter([
    {
      path: "/",
      errorElement: <FrontPage />,
      children: [
        {
          path: "/",
          element: <FrontPage cookies={cookies} setUser={setUser} user={user}/>,
        },
        {
          path: "/registration",
          element: <RegisterPage />,
        },
        {
          path: "/login",
          element: <LoginPage cookies={cookies} setUser={setUser} user={user} />
        },
        {
          path: "/solar-watch",
          element: <SolarWatchPage cookies={cookies}/>
        }
      ]
    }
  ]);

  return (
    <React.StrictMode>
      <RouterProvider router={router} />
    </React.StrictMode>
  );
};

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(<App />);
reportWebVitals();