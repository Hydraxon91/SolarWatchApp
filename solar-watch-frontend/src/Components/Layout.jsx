import React from 'react';
import SunriseAnimLottie from './SunriseAnimLottie';
import Navbar from './Navbar';
import '../styles/lotty.css';

const Layout = ({ children, newStopFrame, setNewStopFrame, logout, user }) => (
  <div className="curr-page">
      <Navbar logout={logout} user={user}/>
      <SunriseAnimLottie newStopFrame={newStopFrame} setNewStopFrame={setNewStopFrame}/>
    {children}
  </div>
);

export default Layout;