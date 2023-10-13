import React from 'react';
import SunriseAnimLottie from './SunriseAnimLottie';
import '../styles/lotty.css';

const Layout = ({ children, newStopFrame, setNewStopFrame }) => (
  <div className="curr-page">
      <SunriseAnimLottie newStopFrame={newStopFrame} setNewStopFrame={setNewStopFrame}/>
    {children}
  </div>
);

export default Layout;