import React from 'react';
import Lottie from 'lottie-react';
import sunriseAnim from "../Animations/sunriseAnim.json";
import SunriseAnimLottie from './SunriseAnimLottie';
import '../styles/lotty.css';

const Layout = ({ children, newStopFrame, setNewStopFrame }) => (
  <div className="curr-page">
    <div className="background-animation">
      <SunriseAnimLottie newStopFrame={newStopFrame} setNewStopFrame={setNewStopFrame}/>
    </div>
    {children}
  </div>
);

export default Layout;