import React, { useState, useEffect } from 'react';
import "../styles/solarinfoelement.css";
import AnimatedClock from './AnimatedClock';

const animFramesObject = {
    sunrise: 100,
    noon: 182,
    sunset: 290
  };

export default function SolarInfoElement({setNewStopFrame, solarData, solarType}){
    
    const [elementClass, setElementClass] = useState(null);
    const [animFrame, setAnimFrame] = useState(null);

    useEffect(()=>{
        if (solarData) {
            setElementClass(solarType);
            setAnimFrame(animFramesObject[solarType]);
        }
    },[solarData])

    const handleMouseEnter = () => {
        setNewStopFrame(animFrame);
      };
    
      const handleMouseLeave = () => {
        setNewStopFrame(182);
      };

    return (
        <div 
            className={`solar-info-element ${elementClass}`}
            onMouseEnter={handleMouseEnter}
            // onMouseLeave={handleMouseLeave}
        >
            <div className={`solar-image ${solarType}`}></div>
            <p className='solar-time'>{solarType}</p>
            <AnimatedClock className='animated-clock' time={solarData} solarType={solarType} />
        </div>
    )
}