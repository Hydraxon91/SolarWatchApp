import React, { useState, useEffect } from 'react';
import { Route, Routes, useLocation } from 'react-router-dom';
import FrontPage from './Pages/FrontPage';
import RegisterPage from './Pages/RegisterPage';
import LoginPage from './Pages/LoginPage';
import SolarWatchPage from './Pages/SolarWatchPage';

const sunriseFrame = 100;
const middayFrame = 162;
const nightFrame = 364;
const sunsetFrame = 50;

function AppRoutes({ setUser, cookies, setNewStopFrame, user, logout}) {
  const location = useLocation();

  useEffect(() => {
    switch (location.pathname) {
      case '/':
        // setNewStopFrame(sunriseFrame);
        break;
      case '/solar-watch':
        // setNewStopFrame(sunriseFrame);
        break;
      case '/login':
        setNewStopFrame(sunriseFrame);
        break;
      case '/registration':
        setNewStopFrame(nightFrame);
        break;
      default:
        setNewStopFrame(sunriseFrame);
    }
  }, [location.pathname, setNewStopFrame]);

  return (
        <Routes>
          <Route exact path="/" element={<FrontPage setUser={setUser} cookies={cookies} user={user} logout={logout}/>} />
          <Route path="/registration" element={<RegisterPage/>} />
          <Route path="/login" element={<LoginPage setUser={setUser} cookies={cookies} />} />
          <Route path="/solar-watch" element={<SolarWatchPage cookies={cookies} setNewStopFrame={setNewStopFrame}/>} />
        </Routes>
  );
}

export default AppRoutes;