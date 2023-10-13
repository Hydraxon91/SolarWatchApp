import React, { useEffect, useRef, useState } from 'react';
import Lottie from 'lottie-react';
import sunriseAnim from "../Animations/sunriseAnim.json";

const sunriseFrame = 100;
const middayFrame = 162;
const nightFrame = 364;
const sunsetFrame = 50;

const SunriseAnimLottie = ({newStopFrame, setNewStopFrame}) => {
    const [stopFrame, setStopFrame] = useState(middayFrame);
    // const [newStopFrame, setNewStopFrame] = useState(middayFrame);
    const [dayNightCycle, setDayNightCycle] = useState(false);
    const lottieRef = useRef(null);

    const SetSunset = () => {
        setNewStopFrame(sunsetFrame);
    };

    const SetSunrise = () => {
        setNewStopFrame(sunriseFrame);
    };

    const SetNight = () => {
        setNewStopFrame(nightFrame);
    };

    const SetMidday = () => {
        setNewStopFrame(middayFrame);
    };

    useEffect(() => {
        //console.log(dayNightCycle);
        if (lottieRef.current) {
            const animationInterval = setInterval(() => {
                const currentFrame = lottieRef.current.animationItem.currentFrame;
                var test = false; //This prevents the second else if from running while UseState updates daynightcycle
                console.log(dayNightCycle);
                if (currentFrame >=100 && newStopFrame === sunriseFrame) {
                    console.log(`currFrame: ${currentFrame}, stopFrame: ${newStopFrame}`);
                    console.log("You want to circle around");
                    setDayNightCycle(true);
                    test = true; //This prevents the second else if from running while UseState updates daynightcycle
                    setStopFrame(nightFrame);
                }
                else {
                    console.log("You don't want to circle around");
                    setStopFrame(newStopFrame);
                    setDayNightCycle(false);
                }
                //console.log(`currFrame: ${currentFrame}, stopFrame: ${stopFrame}`);
                if (dayNightCycle && Math.abs(currentFrame - stopFrame) <= 3.5) {
                    console.log("should cycle around");
                    setStopFrame(sunriseFrame);
                    lottieRef.current.goToAndPlay(0, true);
                    setDayNightCycle(false);
                    clearInterval(animationInterval);
                }
                else if (!dayNightCycle && Math.abs(currentFrame - stopFrame) <= 2.5 && !test){
                    console.log("reached destination");
                    lottieRef.current.pause();
                    clearInterval(animationInterval);
                    setDayNightCycle(false);
                }
                
            }, 100);
            return () => clearInterval(animationInterval);
        }
    }, [lottieRef, stopFrame, dayNightCycle, newStopFrame]);

    useEffect(() => {
        if (lottieRef.current && lottieRef.current.animationItem) {
            const currentFrame = lottieRef.current.animationItem.currentFrame;

            if (currentFrame < stopFrame) {
                // console.log("going up ran");
                lottieRef.current.setDirection(1); // Play forward
                lottieRef.current.goToAndPlay(currentFrame, true);
            } else if (currentFrame > stopFrame) {
                // console.log("going down ran");
                lottieRef.current.setDirection(-1); // Play backward
                lottieRef.current.goToAndPlay(currentFrame, true);
            }
        }
    }, [lottieRef, stopFrame]);

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
            <button onClick={SetMidday}>Set midday</button>
        </div>
    );
};

export default SunriseAnimLottie;