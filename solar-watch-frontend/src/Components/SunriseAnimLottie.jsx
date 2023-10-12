import React, { useEffect, useRef, useState } from 'react';
import Lottie, { LottieRefCurrentProps } from 'lottie-react';
// import LottieView from 'lottie-react-native';
import sunriseAnim from "../Animations/sunriseAnim.json";

const sunriseFrame = 100;
const middayFrame = 142;
const nightFrame = 364;


const SunriseAnimLottie = () => {
    
    const [stopFrame, setStopFrame] = useState(middayFrame);
    const [goingUp, setGoingUp] = useState(true);
    const [dayNightCycle, setDayNightCycle] = useState(false);
    const lottieRef = useRef(null);

   const SetSunset = () =>{
        setDayNightCycle(false);
        setGoingUp(false);
        lottieRef.current.setDirection(-1);
        setStopFrame(0);
        lottieRef.current.play();
   }
   const SetSunrise = () =>{
        var current = lottieRef.current.animationItem.currentFrame;
        setGoingUp(true);
        lottieRef.current.setDirection(1);
        if (current>80) {
            setDayNightCycle(true);
            setStopFrame(nightFrame);
            lottieRef.current.goToAndPlay(current, true);
        }
        else{
            setStopFrame(sunriseFrame);
            lottieRef.current.goToAndPlay(0, true);
        }
        setStopFrame(sunriseFrame)
    }
    const SetNight = () =>{
        setDayNightCycle(false);
        setGoingUp(true);
        lottieRef.current.setDirection(1);
        setStopFrame(nightFrame);
        lottieRef.current.goToAndPlay(lottieRef.current.animationItem.currentFrame, true); 
    }
   
    useEffect(() => {
        if (lottieRef.current) {

            // console.log(lottieRef.current);
            const animationInterval = setInterval(() => {
                const currentFrame = lottieRef.current.animationItem.currentFrame;
                if (goingUp) {
                    if ((!dayNightCycle) && Math.abs(currentFrame - stopFrame) <= 1.5) {
                        lottieRef.current.pause();
                        clearInterval(animationInterval);
                    } else if (dayNightCycle && Math.abs(currentFrame - stopFrame) <= 3.5) {
                        setStopFrame(sunriseFrame);
                        lottieRef.current.goToAndPlay(0, true);
                        setDayNightCycle(false);
                        clearInterval(animationInterval);
                    }
                } else if (!goingUp && !dayNightCycle && Math.abs(stopFrame - currentFrame) <= 1.5) {
                    lottieRef.current.pause();
                    clearInterval(animationInterval);
                }
            }, 100);
            // console.log(lottieRef.current.getDuration());
        return () => clearInterval(animationInterval);

        }
    }, [lottieRef, stopFrame, dayNightCycle]);

    return (
        <div>
            <Lottie
                animationData={sunriseAnim}
                lottieRef={lottieRef}
                loop={false}
            />
            <button onClick={SetSunset}>Set sunset</button>
            <button onClick={SetSunrise}>Set sunrise</button>
            <button onClick={SetNight}>Set night</button>
        </div>
    );
};

export default SunriseAnimLottie;