import React, { useEffect, useRef, useState } from 'react';
import Lottie from 'lottie-react';
import sunriseAnim from "../Animations/sunriseAnim.json";
import '../styles/lotty.css';

const sunriseFrame = 100;
const middayFrame = 162;
const nightFrame = 364;
const sunsetFrame = 50;

const SunriseAnimLottie = ({newStopFrame, setNewStopFrame}) => {
    const [stopFrame, setStopFrame] = useState(middayFrame);
    const [dayNightCycle, setDayNightCycle] = useState(false);
    const lottieRef = useRef(null);

    useEffect(() => {
        if (lottieRef?.current && lottieRef.current.animationItem) {
            const animationInterval = setInterval(() => {
                const currentFrame = lottieRef.current.animationItem.currentFrame;
                var test = false; //This prevents the second else if from running while UseState updates daynightcycle
                if (currentFrame >=100 && newStopFrame === sunriseFrame) {
                    setDayNightCycle(true);
                    test = true; //This prevents the second else if from running while UseState updates daynightcycle
                    setStopFrame(nightFrame);
                }
                else {
                    setStopFrame(newStopFrame);
                    setDayNightCycle(false);
                }
                if (dayNightCycle && Math.abs(currentFrame - stopFrame) <= 4) {
                    setStopFrame(sunriseFrame);
                    lottieRef.current.goToAndPlay(0, true);
                    setDayNightCycle(false);
                    clearInterval(animationInterval);
                }
                else if (!dayNightCycle && Math.abs(currentFrame - stopFrame) <= 4 && !test){
                    lottieRef.current.pause();
                    clearInterval(animationInterval);
                    setDayNightCycle(false);
                }
                
            }, 90);
            return () => clearInterval(animationInterval);
        }
    }, [lottieRef, stopFrame, dayNightCycle, newStopFrame]);

    useEffect(() => {
        if (lottieRef.current && lottieRef.current.animationItem) {
            const currentFrame = lottieRef.current.animationItem.currentFrame;
            lottieRef.current.setSpeed(2);
            if (currentFrame < stopFrame) {
                lottieRef.current.setDirection(1); // Play forward
                lottieRef.current.goToAndPlay(currentFrame, true);
            } else if (currentFrame > stopFrame) {
                lottieRef.current.setDirection(-1); // Play backward
                lottieRef.current.goToAndPlay(currentFrame, true);
            }
        }
    }, [lottieRef, stopFrame]);

    return (
        <div className='background-animation-container'>
            <Lottie
                className='background-animation'
                animationData={sunriseAnim}
                lottieRef={lottieRef}
                loop={false}
                // preserveAspectRatio="xMinYMin slice"
            />
        </div>
    );
};

export default SunriseAnimLottie;