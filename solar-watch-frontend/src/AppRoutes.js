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

function AppRoutes({ setUser, cookies, setNewStopFrame, user }) {
  const location = useLocation();

  useEffect(() => {
    switch (location.pathname) {
      case '/':
        setNewStopFrame(middayFrame);
        break;
      case '/solar-watch':
        setNewStopFrame(sunriseFrame);
        break;
      case '/login':
        setNewStopFrame(nightFrame);
        break;
      case '/registration':
        setNewStopFrame(sunsetFrame);
        break;
      default:
        setNewStopFrame(182);
    }
  }, [location.pathname, setNewStopFrame]);

  return (
        <Routes>
          <Route exact path="/" element={<FrontPage setUser={setUser} cookies={cookies} user={user}/>} />
          <Route path="/registration" element={<RegisterPage/>} />
          <Route path="/login" element={<LoginPage setUser={setUser} cookies={cookies} />} />
          <Route path="/solar-watch" element={<SolarWatchPage cookies={cookies}/>} />
        </Routes>
  );
}

export default AppRoutes;